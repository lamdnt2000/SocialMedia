using Business.SignalR;
using Business.Utils;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using static Business.Schedule.RegexUtil;
using static Business.Schedule.TriggerUtil;
namespace Business.ScheduleService
{
    public class ScheduleSocial : IScheduleSocial
    {
        private readonly string CRAWLER = "Crawler.jar";
        private readonly string PARSER = "Parser.jar";
        private readonly IHubContext<NotificationHub> _hubContext;

        public ScheduleSocial(IHttpContextAccessor httpContextAccessor,
            IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [AutomaticRetry(Attempts = 2, LogEvents = true, OnAttemptsExceeded = AttemptsExceededAction.Fail)]

        public async Task FetchChannelJobAsync(string platform, string user, string id)
        {
         
            var fetchResult = CreateRequest(platform, user, CRAWLER);
            if (fetchResult.ToLower().Contains("success"))
            {
                if (id != null)
                {
                    await SendAsync(id, "Get Data Success. Waiting upload into database");
                }
            }
            else
            {
                throw new Exception(fetchResult);
            }

        }
        [AutomaticRetry(Attempts = 2, LogEvents = true, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public async Task  CreateChannelJobAsync(string platform, string user, string id)
        {
            var fetchResult = CreateRequest(platform, user, PARSER);
            fetchResult = fetchResult.Replace("\r", "").Replace("\n", "");
            if (Int32.TryParse(fetchResult, out int result))
            {
                //RecurringJob.AddOrUpdate(user, () => UpdateChannelJob(platform, user, result), Cron.MinuteInterval(30));
                var dateRange = DateUtil.GenerateDateInRange(1);
                await SendAsync(id,"Data is ready");
            }
            else
            {
                throw new Exception(fetchResult);
            }
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

        public void UpdateChannelJob(string platform, string user, int id)
        {
            var jobId = BackgroundJob.Enqueue(() => FetchChannelJobAsync(platform, user, null));
            BackgroundJob.ContinueJobWith(jobId, () => UpdateRequest(platform, user, PARSER, id));
        }


       

        public async Task SendAsync(string id,string message)
        {

            await _hubContext.Clients.Group(id).SendAsync("IndividualMsg", message);
        }

        public void TokenSchedule()
        {
            UpdateToken();
        }

        public void CreateScheduleUpdateChannel(int platformId, string user, int id)
        {
            string platform;
            switch (platformId)
            {
                case 1:
                    platform = "Y";
                    break;
                case 2:
                    platform = "F";
                    break;
                case 3:
                    platform = "T";
                    break;
                default:
                    platform = "";
                    break;
            }
            RecurringJob.AddOrUpdate(user, () => UpdateChannelJob(platform, user, id), Cron.MinuteInterval(30));
        }
    }
}
