using AutoFilterer.Types;
using Business.ScheduleService;
using Business.Service.NotificationService;
using Business.SignalR;
using DataAccess.Entities;
using DataAccess.Models.ChannelCrawlModel;
using DataAccess.Repository.NotificationRepo;
using Firebase.Auth;
using Hangfire;
using Hangfire.Common;
using Hangfire.States;
using Hangfire.Storage;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Business.Service.ScheduleService
{
    public class JobStatusFilter : IApplyStateFilter
    {

        
        private readonly IScheduleSocial _scheduleSocial;

        public JobStatusFilter(IScheduleSocial scheduleSocial)
        {
           
            _scheduleSocial = scheduleSocial;
        }

        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            var c = context.NewState as FailedState;
            if (c?.Exception != null && c != null)
            {
                var methodName = context.Job.Method.Name;

                if (methodName.Contains("FetchAsync"))
                {

                    var user = context.Job.Args[1].ToString();
                    var userId = context.Job.Args[2];
                    if (userId != null)
                    {
                        var platForm = methodName.Split("FetchAsync")[0];
                        var key = platForm.Substring(0, 1) + ":" + user;
                       
                        var noti = new Notification()
                        {
                            Status = false,
                            Type = platForm,
                            Message = "Fetch data of user: " + user + " failed"
                        };
                       
                        
                        BackgroundJob.Enqueue(() => _scheduleSocial.SendNotificationAsync(key, noti));
                    }


                }
                else if (methodName.Contains("ParserData"))
                {
                    var user = context.Job.Args[1].ToString();
                    var userId = context.Job.Args[3];
                    if (userId != null)
                    {
                        var platForm = methodName.Split("ParserData")[0];
                        var noti = new Notification()
                        {
                            Status = false,
                            Type = platForm,
                            Message = "Data ready in database of: " + user + " failed"
                        };
                        var key = platForm.Substring(0, 1) + ":" + user;
                        BackgroundJob.Enqueue(() => _scheduleSocial.SendNotificationAsync(key, noti));
                    }
                }

            }
            else
            {
                var success = context.NewState as SucceededState;
                
                if (success != null)
                {
                    var successMethod = context.Job.Method.Name;
                    if (successMethod.Contains("ParserData"))
                    {
                        var user = context.Job.Args[1].ToString();
                        var userId = context.Job.Args[3];
                        if (userId != null)
                        {
                            var platForm = successMethod.Split("ParserData")[0];
                            var platformId = 0;
                            switch (platForm)
                            {
                                case "Facebook":
                                    platformId =2;
                                    break;
                                case "Youtube":
                                    platformId = 1;
                                    break;
                                case "Tiktok":
                                    platformId = 3;
                                    break;
                            }
                            ChannelFilter channelFilter = new ChannelFilter()
                            {
                                Username = user,
                                Platform = platformId,
                                CreatedTime = new Range<DateTime>() { Min = DateTime.Now.AddMonths(-6).Date, Max = DateTime.Now.Date }
                            };
                            var noti = new Notification()
                            {
                                Status = true,
                                Type = platForm,
                                Message = "Fetch data of: " + user + " success",
                                Options = Newtonsoft.Json.JsonConvert.SerializeObject(channelFilter)
                            };
                           
                            var key = platForm.Substring(0, 1) + ":" + user;
                            BackgroundJob.Enqueue(() => _scheduleSocial.SendNotificationAsync(key, noti));
                        }
                    }
                }
            }
        }

        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {

        }
    }
}
