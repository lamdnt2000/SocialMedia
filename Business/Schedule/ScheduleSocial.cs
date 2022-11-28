using Hangfire;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Business.Schedule.RegexUtil;
using static Business.Schedule.TriggerUtil;
namespace Business.Schedule
{
    public class ScheduleSocial : IScheduleSocial
    {
        private readonly string CRAWLER = "Crawler.jar";
        private readonly string PARSER = "Parser.jar";

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
        public void CreateChannelJob(string platform, string user)
        {
            var fetchResult = CreateRequest(platform, user, PARSER);
            fetchResult = fetchResult.Replace("\r", "").Replace("\n", "");
            if (Int32.TryParse(fetchResult, out int result))
            {
                RecurringJob.AddOrUpdate(user, () => UpdateChannelJob(platform, user, result), Cron.MinuteInterval(30));
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
    }
}
