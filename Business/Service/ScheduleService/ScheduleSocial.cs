using Business.Utils;
using CorePush.Interfaces;
using DataAccess.Models.NotificationModel;
using Hangfire;
using System;
using System.Threading.Tasks;
using static Business.Schedule.RegexUtil;
using static Business.Schedule.TriggerUtil;
namespace Business.ScheduleService
{
    public class ScheduleSocial :  IScheduleSocial
    {
        private readonly string CRAWLER = "Crawler.jar";
        private readonly string PARSER = "Parser.jar";


        private readonly IFcmSender _fcmSender;

        public ScheduleSocial(IFcmSender fcmSender)
        {
            _fcmSender = fcmSender;
        }

        [AutomaticRetry(Attempts = 2, LogEvents = true, OnAttemptsExceeded = AttemptsExceededAction.Fail)]

        public void FetchChannelJob(string platform, string user)
        {
         
            var fetchResult = CreateRequest(platform, user, CRAWLER);
            if (fetchResult.ToLower().Contains("success"))
            {
                
            }
            else
            {
                throw new Exception(fetchResult);
            }

        }
        [AutomaticRetry(Attempts = 2, LogEvents = true, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public async Task  CreateChannelJobAsync(string platform, string user, string fcmToken)
        {
            var fetchResult = CreateRequest(platform, user, PARSER);
            fetchResult = fetchResult.Replace("\r", "").Replace("\n", "");
            if (Int32.TryParse(fetchResult, out int result))
            {
                RecurringJob.AddOrUpdate(user, () => UpdateChannelJob(platform, user, result), Cron.MinuteInterval(30));
                var dateRange = DateUtil.GenerateDateInRange(1);
                await SendAsync(fcmToken);
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
            var jobId = BackgroundJob.Enqueue(() => FetchChannelJob(platform, user));
            BackgroundJob.ContinueJobWith(jobId, () => UpdateRequest(platform, user, PARSER, id));
        }


       

        public async Task SendAsync(string fcmToken)
        {

            Notification notification = new Notification() { Data = new Notification.DataPlayload() { Message = "success" } };

            await _fcmSender.SendAsync(fcmToken, notification);
        }

    }
}
