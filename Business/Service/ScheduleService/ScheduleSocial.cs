using Business.Config;
using Business.Repository.UserRepo;
using Business.Service;
using Business.Service.ScheduleService;
using Business.SignalR;
using DataAccess.Entities;
using DataAccess.Enum;
using DataAccess.Repository.NotificationRepo;
using DataAccess.Repository.UserTypeRepo;
using Google.Api.Gax;
using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Business.Schedule.RegexUtil;
using static Business.Schedule.TriggerUtil;
namespace Business.ScheduleService
{
    public class ScheduleSocial : BaseService, IScheduleSocial
    {
        private readonly string CRAWLER = "Crawler.jar";
        private readonly string PARSER = "Parser.jar";
        public static ConnectionCache<string> CacheFetchUrl = new ConnectionCache<string>();
        private readonly INotificationRepository _notificationRepository;
        private readonly IHubContext<NotificationHub> _hubContext;
        public static string UserId { get; set; }
        public static string User { get; set; }
        public ScheduleSocial(IHttpContextAccessor httpContextAccessor
            , IUserRepository userRepository
            , IUserTypeRepository userTypeRepository
            , IHubContext<NotificationHub> hubContext
            , INotificationRepository notificationRepository) : base(httpContextAccessor, userRepository, userTypeRepository)
        {
            _hubContext = hubContext;
            _notificationRepository = notificationRepository;
        }

        public bool FetchChannelJobAsync(string url, string id)
        {

            string jobId = "";
            var items = ValidateUrl(url);
            var platfrom = items.Item1;
            var user = items.Item2;
            UserId = id;
            User = user;
            var key = platfrom.Substring(0, 1) + ":" + items.Item2;
            if (CacheFetchUrl.GetConnections(key).Contains(id))
            {
                throw new Exception("Channel already request. Wait notification");
            }
            CacheFetchUrl.Add(key, id);
            if (CacheFetchUrl.GetConnections(key).Count() == 1)
            {
                switch (platfrom)
                {
                    case "FACEBOOK":
                        jobId = BackgroundJob.Enqueue(() => FacebookFetchAsync(null, user, id));
                        BackgroundJob.ContinueJobWith(jobId, () => FacebookParserData(null, user, 0, id));
                        break;
                    case "YOUTUBE":
                        jobId = BackgroundJob.Enqueue(() => YoutubeFetchAsync(null, user, id));
                        BackgroundJob.ContinueJobWith(jobId, () => YoutubeParserData(null, user, 0, id));
                        break;
                    case "TIKTOK":
                        jobId = BackgroundJob.Enqueue(() => TiktokFetchAsync(null, user, id));
                        BackgroundJob.ContinueJobWith(jobId, () => TiktokParserData(null, user, 0, id));
                        break;
                    default:
                        return false;
                }
            }


            return true;

        }


        public (string, string) ValidateUrl(string url)
        {
            var result = RegexPlatformAndUser(url);
            if (result.Item1 == null)
            {
                throw new Exception("Invalid url");
            }
            return result;
        }

        [DisplayName("Schedule Update Data User: {1}")]
        [DisableConcurrentExecution(60)]
        public void UpdateChannelJob(int platformId, string user, int id)
        {
            var jobId = "";
            switch (platformId)
            {
                case 1:
                    jobId = BackgroundJob.Enqueue(() => YoutubeFetchAsync(null, user, null));
                    BackgroundJob.ContinueJobWith(jobId, () => YoutubeParserData(null, user, id, null));
                    break;
                case 2:
                    jobId = BackgroundJob.Enqueue(() => FacebookFetchAsync(null, user, null));
                    BackgroundJob.ContinueJobWith(jobId, () => FacebookParserData(null, user, id, null));
                    break;
                case 3:
                    jobId = BackgroundJob.Enqueue(() => TiktokFetchAsync(null, user, null));
                    BackgroundJob.ContinueJobWith(jobId, () => TiktokParserData(null, user, id, null));
                    break;


            }
        }






        public void TokenSchedule()
        {
            UpdateToken();
        }



        [Queue("facebook")]
        [DisplayName("Fetch Facebook User: {1}")]
        [JobStatusFilter]
        [AutomaticRetry(Attempts = 2, LogEvents = true, OnAttemptsExceeded = AttemptsExceededAction.Fail, DelaysInSeconds = new int[] { 2, 2 })]

        public async Task FacebookFetchAsync(PerformContext context, string user, string id)
        {
            var progress = context.WriteProgressBar(10);
            context.SetTextColor(ConsoleTextColor.Green);
            context.WriteLine("Running ...");
            FetchProcessAsync("F", user, id);
            progress.SetValue(90);
            Thread.Sleep(500);
            progress.SetValue(100);
            context.WriteLine("SUCCESS");
        }

        [Queue("youtube")]
        [DisplayName("Fetch Youtube User: {1}")]
        [JobStatusFilter]
        [AutomaticRetry(Attempts = 2, LogEvents = true, OnAttemptsExceeded = AttemptsExceededAction.Fail, DelaysInSeconds = new int[] { 2, 2 })]
        public async Task YoutubeFetchAsync(PerformContext context, string user, string id)
        {
            var progress = context.WriteProgressBar(10);
            context.SetTextColor(ConsoleTextColor.Green);
            context.WriteLine("Running ...");
            FetchProcessAsync("Y", user, id);
            progress.SetValue(90);
            Thread.Sleep(500);
            progress.SetValue(100);
            context.WriteLine("SUCCESS");
        }
        [Queue("tiktok")]
        [DisplayName("Fetch Tiktok User: {1}")]
        [JobStatusFilter]
        [AutomaticRetry(Attempts = 3, LogEvents = true, OnAttemptsExceeded = AttemptsExceededAction.Fail, DelaysInSeconds = new int[] { 4, 4 ,4 })]
        public async Task TiktokFetchAsync(PerformContext context, string user, string id)
        {
            var progress = context.WriteProgressBar(10);
            context.SetTextColor(ConsoleTextColor.Green);
            context.WriteLine("Running ...");
            FetchProcessAsync("T", user, id);
            progress.SetValue(90);
            Thread.Sleep(500);
            progress.SetValue(100);
            context.WriteLine("SUCCESS");
        }

        [Queue("fparser")]
        [DisplayName("Insert Facebook User: {1} Into DB")]
        [JobStatusFilter]
        [AutomaticRetry(Attempts = 1, LogEvents = true, OnAttemptsExceeded = AttemptsExceededAction.Fail, DelaysInSeconds = new int[] { 2 })]
        public async Task FacebookParserData(PerformContext context, string user, int cid, string id)
        {

            var progress = context.WriteProgressBar(10);
            context.SetTextColor(ConsoleTextColor.Green);
            context.WriteLine("Running ...");
            if (cid == 0)
            {
                await ParseProcessAsync("F", user, id);

            }
            else
            {
                UpdateRequest("F", user, PARSER, cid);
            }
            progress.SetValue(90);
            Thread.Sleep(500);
            progress.SetValue(100);
            context.WriteLine("SUCCESS");
        }

        [Queue("yparser")]
        [DisplayName("Insert Youtube User: {1} Into DB")]
        [JobStatusFilter]
        [AutomaticRetry(Attempts = 1, LogEvents = true, OnAttemptsExceeded = AttemptsExceededAction.Fail, DelaysInSeconds = new int[] { 2 })]
        public async Task YoutubeParserData(PerformContext context, string user, int cid, string id)
        {

            var progress = context.WriteProgressBar(10);
            context.SetTextColor(ConsoleTextColor.Green);
            context.WriteLine("Running ...");
            if (cid == 0)
            {
                await ParseProcessAsync("Y", user, id);
            }
            else
            {
                UpdateRequest("Y", user, PARSER, cid);
            }
            progress.SetValue(90);
            Thread.Sleep(500);
            progress.SetValue(100);
            context.WriteLine("SUCCESS");
        }
        [Queue("tparser")]
        [DisplayName("Insert Tiktok User: {1} Into DB")]
        [JobStatusFilter]
        [AutomaticRetry(Attempts = 1, LogEvents = true, OnAttemptsExceeded = AttemptsExceededAction.Fail, DelaysInSeconds = new int[] { 2 })]
        public async Task TiktokParserData(PerformContext context, string user, int cid, string id)
        {

            var progress = context.WriteProgressBar(10);
            context.SetTextColor(ConsoleTextColor.Green);
            context.WriteLine("Running ...");
            if (cid == 0)
            {
                await ParseProcessAsync("T", user, id);
            }
            else
            {
                UpdateRequest("T", user, PARSER, cid);
            }
            progress.SetValue(90);
            Thread.Sleep(500);
            progress.SetValue(100);
            context.WriteLine("SUCCESS");
        }

        private void FetchProcessAsync(string platform, string user, string id)
        {
            var fetchResult = CreateRequest(platform, user, CRAWLER);

            if (!fetchResult.ToLower().Contains("success"))
            {
                throw new Exception(fetchResult);

            }


        }

        private async Task ParseProcessAsync(string platform, string user, string id)
        {
            var fetchResult = CreateRequest(platform, user, PARSER);
            fetchResult = fetchResult.Replace("\r", "").Replace("\n", "");
            if (Int32.TryParse(fetchResult, out int result))
            {
                int platformId = 0;
                switch (platform)
                {
                    case "Y":
                        platformId = 1;
                        break;
                    case "F":
                        platformId = 2;
                        break;
                    case "T":
                        platformId = 3;
                        break;
                }
                RecurringJob.AddOrUpdate(((EnumPlatform)platformId).ToString() + ": " + user, () => UpdateChannelJob(platformId, user, result), "0 0 * * 6");



            }
            else
            {

                throw new Exception(fetchResult);
            }
        }

        [Queue("default")]
        [DisplayName("Send notification")]
        [AutomaticRetry(Attempts = 1, LogEvents = true, OnAttemptsExceeded = AttemptsExceededAction.Fail, DelaysInSeconds = new int[] { 3 })]

        public async Task SendNotificationAsync(string key, Notification notification)
        {
            var notifications = new List<Notification>();
            if (CacheFetchUrl.GetConnections(key).Count() == 0)
            {
                throw new Exception("Invalid connection pool");
            }
            foreach (string id in CacheFetchUrl.GetConnections(key))
            {

                var noti = new Notification()
                {
                    IsClick = false,
                    Message = notification.Message,
                    TimeStamp = DateTime.Now,
                    Status = notification.Status,
                    UserId = Convert.ToInt32(id),
                    Type = notification.Type,
                    Options = notification.Options,
                };
                notifications.Add(noti);


            }
            await _notificationRepository.UpdateRange(notifications);
            foreach (string id in CacheFetchUrl.GetConnections(key))
            {
                var connectionId = NotificationHub.GetConnectionIdFromUserId(id);
                if (connectionId != null)
                {
                    await _hubContext.Clients.Client(connectionId).SendAsync("IndividualMsg", 1);
                }
            }
            CacheFetchUrl.Remove(key);
        }
    }
}
