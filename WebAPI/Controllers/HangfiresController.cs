using Business.Config;
using Business.Constants;
using Business.ScheduleService;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Hangfire;
using static Business.Utils.ResponseFormat;
using static Business.Constants.ResponseMsg;
using Business.Service.ChannelCrawlService;
using Business.SignalR;
using Microsoft.AspNetCore.SignalR;
using Hangfire.Storage;
using System.Linq;
using Business.Utils;
using Business.Service.HangfireService;
using DataAccess.Models.ChannelCrawlModel;
using System.Collections.Generic;
using DataAccess.Entities;
using Firebase.Auth;

namespace WebAPI.Controllers
{
    [Route(Constant.ApiPath.HANGFIRE_PATH)]
    [ApiController]
    [CustomAuth(RoleAuthorize.ROLE_ADMIN + "," + RoleAuthorize.ROLE_MEMBER)]
    public class HangfiresController : ControllerBase
    {
        private readonly IScheduleSocial _scheduleSocial;
        private readonly IChannelCrawlService _channelCrawlService;
        private readonly IHangfireService _hangfireService;

        public HangfiresController(IScheduleSocial scheduleSocial, IChannelCrawlService channelCrawlService, IHangfireService hangfireService)
        {
            _scheduleSocial = scheduleSocial;
            _channelCrawlService = channelCrawlService;
            _hangfireService = hangfireService;
        }

        [HttpPost("/TokenSchedule")]
        public async Task<IActionResult> CreateScheduleUpdateToken()
        {
            try
            {
                RecurringJob.AddOrUpdate("Token", () => _scheduleSocial.TokenSchedule(), Cron.HourInterval(4));

                return JsonResponse(200, SUCCESS, "");
            }
            catch (Exception e)
            {

                if (e.Message.Contains(NOT_FOUND))
                {
                    return JsonResponse(400, NOT_FOUND, e.Message);
                }
                return JsonResponse(401, UNAUTHORIZE, e.Message);

            }
        }

        [HttpPost("channels/{id}")]
        public async Task<IActionResult> CreateScheduleChannel(int id)
        {
            try
            {
                var result = await _channelCrawlService.GetById(id);
                if (result == null)
                {
                    return JsonResponse(400, NOT_FOUND, "");
                }
                var user = result.Username;
                if (user == null)
                {
                    user = result.Cid;
                }


                RecurringJob.AddOrUpdate(user, () => _scheduleSocial.UpdateChannelJob(result.PlatformId, user, id), "0 0 * * 6");
                return JsonResponse(200, SUCCESS, "");

               

            }
            catch (Exception e)
            {

                if (e.Message.Contains(NOT_FOUND))
                {
                    return JsonResponse(400, NOT_FOUND, e.Message);
                }
                return JsonResponse(401, UNAUTHORIZE, e.Message);

            }
        }

        [HttpPost("channels")]
        public async Task<IActionResult> CreateScheduleChannels([FromBody] List<int> ids)
        {
            try
            {
                var result = await _hangfireService.CreateSchedule(ids);
                if (result == null)
                {
                    return JsonResponse(400, NOT_FOUND, "");
                }
               foreach (ChannelCrawl c in result)
                {
                    RecurringJob.AddOrUpdate(c.Username, () => _scheduleSocial.UpdateChannelJob(c.PlatformId, c.Username, c.Id), "0 0 * * 6");
                }
                return JsonResponse(200, SUCCESS, "");

               

            }
            catch (Exception e)
            {

                if (e.Message.Contains(NOT_FOUND))
                {
                    return JsonResponse(400, NOT_FOUND, e.Message);
                }
                return JsonResponse(401, UNAUTHORIZE, e.Message);

            }
        }


        [HttpGet]
        public async Task<IActionResult> GetAllCurrentJob()
        {
            try
            {
                var result = _hangfireService.GetCurrentConcurrentJob();
                return JsonResponse(200, SUCCESS, result);


            }
            catch (Exception e)
            {

                if (e.Message.Contains(NOT_FOUND))
                {
                    return JsonResponse(400, NOT_FOUND, e.Message);
                }
                return JsonResponse(401, UNAUTHORIZE, e.Message);

            }
        }

        [HttpPut("{jobId}")]
        public async Task<IActionResult> TriggerJobId(string jobId)
        {
            try
            {
                var result = _hangfireService.TriggerJob(jobId);
                return JsonResponse(200, SUCCESS, result);

            }
            catch (Exception e)
            {

                if (e.Message.Contains(NOT_FOUND))
                {
                    return JsonResponse(400, NOT_FOUND, e.Message);
                }
                if (e.Message.Contains(EXCUSED))
                {
                    return JsonResponse(400, EXCUSED, e.Message);
                }
                return JsonResponse(401, UNAUTHORIZE, e.Message);

            }
        }
        
        [HttpDelete("{jobId}")]
        public async Task<IActionResult> DeleteJob(string jobId)
        {
            try
            {
                _hangfireService.DeleteJob(jobId);
                return JsonResponse(200, SUCCESS, "");

            }
            catch (Exception e)
            {

                if (e.Message.Contains(NOT_FOUND))
                {
                    return JsonResponse(400, NOT_FOUND, e.Message);
                }
                if (e.Message.Contains(EXCUSED))
                {
                    return JsonResponse(400, EXCUSED, e.Message);
                }
                return JsonResponse(401, UNAUTHORIZE, e.Message);

            }
        }

        [HttpGet("channels")]
        public async Task<IActionResult> GetAllChannelReadySchedule([FromQuery]HangfireChannelFilter filter)
        {
            try
            {
                var result = await _hangfireService.GetAllChannelSchedule(filter);
                return JsonResponse(200, SUCCESS, result);


            }
            catch (Exception e)
            {

                if (e.Message.Contains(NOT_FOUND))
                {
                    return JsonResponse(400, NOT_FOUND, e.Message);
                }
                return JsonResponse(401, UNAUTHORIZE, e.Message);

            }
        }
    }
}
