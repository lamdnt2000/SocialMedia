using DataAccess.Entities;
using DataAccess.Repository.UserTypeRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Utils;
using DataAccess.Models.FeatureModel;
using DataAccess.Models.FeaturePlanModel;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Routing;

namespace Business.Config
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IDistributedCache _cache;

        private List<string> ValidAction = new List<string>() { "StatisticChannelById", "GetChannelByUrl", "CompareChannel", "StatisticTopPostChannelById" };
        public RateLimitingMiddleware(RequestDelegate next,
                                      IDistributedCache cache
                                      )
        {
            this._next = next;
            this._cache = cache;

        }

        public async Task InvokeAsync(HttpContext context, IUserTypeRepository userTypeRepository)
        {
            var currendId = context.User.Claims.First().Value;
            var path = context.Request.Path;
            var router = context.GetRouteData();
            var data = await GetCurrentFeatureCache(context, currendId, userTypeRepository);
            if (path.Value.Contains("/v1/channels"))
            {
                var action = router.Values["action"].ToString();
                bool flag = false;
                if (ValidAction.Contains(action))
                {

                    if (action.Equals("StatisticChannelById"))
                    {
                        var platfromId = Convert.ToInt32(context.Request.Query["platform"].ToString());
                        var quota = GetQuota(data, "DAILYSEARCH");
                        if (quota > 0)
                        {
                            switch (platfromId)
                            {
                                case 1:
                                    flag = CheckValidField(data, "YOUTUBE");
                                    break;
                                case 2:
                                    flag = CheckValidField(data, "FACEBOOK");
                                    break;
                                case 3:
                                    flag = CheckValidField(data, "TIKTOK");
                                    break;
                                default:
                                    break;
                            }
                        }


                    }
                    else if (action.Equals("GetChannelByUrl"))
                    {
                        var url = context.Request.Query["url"].ToString();
                        var quota = GetQuota(data, "MONTHREQUEST");

                        if (quota > 0)
                        {
                            if (url.IndexOf("youtube") > 0)
                            {
                                flag = CheckValidField(data, "YOUTUBE");
                            }
                            else if (url.IndexOf("facebook") > 0)
                            {
                                flag = CheckValidField(data, "FACEBOOK");
                            }
                            else if (url.IndexOf("tiktok") > 0)
                            {
                                flag = CheckValidField(data, "TIKTOK");
                            }
                        }

                    }
                    else if (action.Equals("CompareChannel"))
                    {
                        var quota = GetQuota(data, "DAILYSEARCH");
                        if (quota > 0)
                        {
                            if (CheckValidField(data, "COMPARE"))
                            {
                                switch (Convert.ToInt32(context.Request.Query["platform"].ToString()))
                                {
                                    case 1:
                                        flag = CheckValidField(data, "YOUTUBE");
                                        break;
                                    case 2:
                                        flag = CheckValidField(data, "FACEBOOK");
                                        break;
                                    case 3:
                                        flag = CheckValidField(data, "TIKTOK");
                                        break;
                                    default:
                                        break;
                                }
                            };
                        }
                    }
                    else if (action.Equals("StatisticTopPostChannelById"))
                    {
                        var quota = GetQuota(data, "DAILYSEARCH");
                        if (quota > 0)
                        {
                            flag = CheckValidField(data, "TOPPOST");
                        }
                    }
                }
                if (flag)
                {
                    await _next(context);
                    
                    return;
                }
                await context.Response.WriteAsJsonAsync(new { StatusCode = 429, Message = "Your quota reach Limit or feature not support." });


            }
            // read the LimitRequest attribute from the endpoint

            // Check whether the request violates the rate limit policy



        }

        private async Task<byte[]> GetCurrentFeature(HttpContext context, string uid, IUserTypeRepository userTypeRepository)
        {
            int id = Convert.ToInt32(uid);
            var userType = await userTypeRepository.Get(x => x.UserId == id);
            if (userType.DateEnd.CompareTo(DateTime.Now) == -1)
            {
                await context.Response.WriteAsJsonAsync(new { StatusCode = 429, Message = "Plan Expries time. Please purchase new" });
            }
            return SerializationUtil.ToByteArray(userType.Feature);
        }

        private async Task<Dictionary<string, FeatureStatistic>> GetCurrentFeatureCache(HttpContext context, string uid, IUserTypeRepository userTypeRepository)
        {
            var result = await _cache.GetAsync(uid);
    
            if (result == null)
            {
                var data = await GetCurrentFeature(context, uid, userTypeRepository);
                var options = new DistributedCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromDays(1));
                await _cache.SetAsync(uid, data, options);

                return SerializationUtil.FromString(SerializationUtil.FromByteArray<string>(data));
            }
            var json = SerializationUtil.FromByteArray<string>(result);
            return SerializationUtil.FromString(SerializationUtil.FromByteArray<string>(result));
        }

        private bool CheckValidField(Dictionary<string, FeatureStatistic> data, string key)
        {
            var feature = data.GetValueOrDefault(key);
            return (feature != null) ? feature.Valid : false;
        }

        private int GetQuota(Dictionary<string, FeatureStatistic> data, string key)
        {
            var feature = data.GetValueOrDefault(key);
            return (feature != null) ? feature.Quota : 0;
        }
    }
}
