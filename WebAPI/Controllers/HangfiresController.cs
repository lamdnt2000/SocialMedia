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

namespace WebAPI.Controllers
{
    [Route(Constant.ApiPath.HANGFIRE_PATH)]
    [ApiController]
    [CustomAuth(RoleAuthorize.ROLE_ADMIN)]
    public class HangfiresController : ControllerBase
    {
        private readonly IScheduleSocial _scheduleSocial;
        private readonly IChannelCrawlService _channelCrawlService;
        

        public HangfiresController(IScheduleSocial scheduleSocial, IChannelCrawlService channelCrawlService
            )
        {
            _scheduleSocial = scheduleSocial;
            _channelCrawlService = channelCrawlService;
            
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
        
        [HttpPost("/{id}")]
        public async Task<IActionResult> CreateScheduleChannel(int id)
        {
            try
            {
                var result = await _channelCrawlService.GetById(id);
                var user = result.Username;
                if (user == null)
                {
                    user = result.Cid;
                }
                if (result != null)
                {
                    
                    _scheduleSocial.CreateScheduleUpdateChannel(result.PlatformId, user, id);
                    return JsonResponse(200, SUCCESS, "");
                }
                return JsonResponse(400, NOT_FOUND, "");

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
     /*   [HttpPost]
        public async Task<IActionResult> TestSendMesage(string message, string fcm)
        {
            try
            {
                await _scheduleSocial.SendAsync(fcm);
                return JsonResponse(200, NOT_FOUND, "");

            }
            catch (Exception e)
            {

                if (e.Message.Contains(NOT_FOUND))
                {
                    return JsonResponse(400, NOT_FOUND, e.Message);
                }
                return JsonResponse(401, UNAUTHORIZE, e.Message);

            }
        }*/
       
    }
}
