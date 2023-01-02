using Microsoft.AspNetCore.Mvc;
using static Business.Utils.ResponseFormat;
using static Business.Constants.ResponseMsg;
using WebAPI.Constant;
using Business.Config;
using Business.Constants;
using Business.Service.ChannelCrawlService;
using System.Threading.Tasks;
using System;
using DataAccess.Models.ChannelCrawlModel;
using Business.Schedule;
using Hangfire;
using Business.Utils;
using AutoFilterer.Types;
using Business.ScheduleService;
using DataAccess.Models.ChannelCrawlModel.CompareModel;

namespace WebAPI.Controllers
{
    [Route(ApiPath.CHANNEL_PATH)]
    [ApiController]

    public class ChannelController : ControllerBase
    {
        private readonly IChannelCrawlService _channelCrawlService;
        private readonly IScheduleSocial _scheduleSocial;

        public ChannelController(IChannelCrawlService channelCrawlService, IScheduleSocial schedule)
        {
            _channelCrawlService = channelCrawlService;
            _scheduleSocial = schedule;
        }

        [CustomAuth(RoleAuthorize.ROLE_ADMIN + "," + RoleAuthorize.ROLE_MEMBER)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetChannelById(int id)
        {
            try
            {
                var result = await _channelCrawlService.GetById(id);
                if (result != null)
                {

                    return JsonResponse(200, SUCCESS, result);
                }
                else
                {
                    return JsonResponse(400, NOT_FOUND, result);
                }
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
        [CustomAuth(RoleAuthorize.ROLE_ADMIN + "," + RoleAuthorize.ROLE_MEMBER)]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery] ChannelSearchFilter paging)
        {
            try
            {
                var result = await _channelCrawlService.SearchAsync(paging);
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

        [CustomAuth(RoleAuthorize.ROLE_ADMIN)]
        [HttpPost]
        public async Task<IActionResult> InsertChannel([FromBody] InsertChannelCrawlDto dto)
        {
            try
            {
                var result = await _channelCrawlService.Insert(dto);
                return JsonResponse(201, INSERT_SUCCESS, new { id = result });
            }
            catch (Exception e)
            {

                if (e.Message.Contains(DUPLICATED))
                {
                    return JsonResponse(400, INSERT_FAILED, e.Message);
                }
                if (e.Message.Contains(NOT_EXIST))
                {
                    return JsonResponse(400, NOT_EXIST, e.Message);
                }
                return JsonResponse(401, UNAUTHORIZE, e.Message);

            }
        }

        [CustomAuth(RoleAuthorize.ROLE_ADMIN)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateChannel(int id, [FromBody] UpdateChannelCrawlDto dto)
        {
            try
            {
                var result = await _channelCrawlService.Update(id, dto);
                return JsonResponse(200, UPDATE_SUCCESS, new { id = result });
            }
            catch (Exception e)
            {

                if (e.Message.Contains(DUPLICATED))
                {
                    return JsonResponse(400, UPDATE_FAILED, e.Message);
                }
                if (e.Message.Contains(NOT_FOUND))
                {
                    return JsonResponse(400, UPDATE_FAILED, e.Message);
                }
                if (e.Message.Contains(NOT_EXIST))
                {
                    return JsonResponse(400, NOT_EXIST, e.Message);
                }

                return JsonResponse(401, UNAUTHORIZE, e.Message);

            }
        }

        [CustomAuth(RoleAuthorize.ROLE_ADMIN)]
        [HttpPut]
        public async Task<IActionResult> AdminUpdateChannel([FromBody] ChannelCrawlDto dto)
        {
            try
            {
                var result = await _channelCrawlService.Update(dto);
                return JsonResponse(200, UPDATE_SUCCESS, new { id = result });
            }
            catch (Exception e)
            {

                if (e.Message.Contains(DUPLICATED))
                {
                    return JsonResponse(400, UPDATE_FAILED, e.Message);
                }
                if (e.Message.Contains(NOT_FOUND))
                {
                    return JsonResponse(400, UPDATE_FAILED, e.Message);
                }
                if (e.Message.Contains(NOT_EXIST))
                {
                    return JsonResponse(400, NOT_EXIST, e.Message);
                }

                return JsonResponse(401, UNAUTHORIZE, e.Message);

            }
        }

        [CustomAuth(RoleAuthorize.ROLE_ADMIN)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChannelById(int id)
        {
            try
            {
                var result = await _channelCrawlService.Delete(id);
                return JsonResponse(200, DELETE_SUCCESS, result);
            }
            catch (Exception e)
            {

                if (e.Message.Contains(NOT_FOUND))
                {
                    return JsonResponse(400, DELETE_FAILED, e.Message);
                }
                return JsonResponse(401, UNAUTHORIZE, e.Message);

            }
        }

        [CustomAuth(RoleAuthorize.ROLE_ADMIN + "," + RoleAuthorize.ROLE_MEMBER)]
        [HttpGet("statistic")]
        public async Task<IActionResult> StatisticChannelById([FromQuery] ChannelFilter filter)
        {
            try
            {
                var result = await _channelCrawlService.Statistic(filter);
                return JsonResponse(200, SUCCESS, result);
            }
            catch (Exception e)
            {

                if (e.Message.Contains(NOT_FOUND))
                {
                    return JsonResponse(400, DELETE_FAILED, e.Message);
                }
                return JsonResponse(401, UNAUTHORIZE, e.Message);

            }
        }

        [CustomAuth(RoleAuthorize.ROLE_ADMIN + "," + RoleAuthorize.ROLE_MEMBER)]
        [HttpGet("toppost/{id}")]
        public async Task<IActionResult> StatisticTopPostChannelById(int id)
        {
            try
            {
                var result = await _channelCrawlService.StatisticTopPost(id);
                return JsonResponse(200, SUCCESS, result);
            }
            catch (Exception e)
            {

                if (e.Message.Contains(NOT_FOUND))
                {
                    return JsonResponse(400, DELETE_FAILED, e.Message);
                }
                return JsonResponse(401, UNAUTHORIZE, e.Message);

            }
        }

        [CustomAuth(RoleAuthorize.ROLE_ADMIN + "," + RoleAuthorize.ROLE_MEMBER)]
        [HttpGet]
        public async Task<IActionResult> GetChannelByUrl(string url)
        {
            try
            {
              
                var userId = await _channelCrawlService.FindChannelByPlatformAndUserId(url);
                if (userId.Equals(""))
                {
                    var user = HttpContext.Items["User"];
                    var id = user.GetType().GetProperty("id")?.GetValue(user, null).ToString();
                    var result = _scheduleSocial.ValidateUrl(url);
                    var jobId = BackgroundJob.Enqueue(() => _scheduleSocial.FetchChannelJobAsync(result.Item1, result.Item2, id));
                    BackgroundJob.ContinueJobWith(jobId, () => _scheduleSocial.CreateChannelJobAsync(result.Item1, result.Item2, id));
                    return JsonResponse(200, NOT_FOUND, "Waiting loading data");
                }
                else
                {
                    var dateRange = DateUtil.GenerateDateInRange(1);
                    ChannelFilter filter = new ChannelFilter() { Username = userId, CreatedTime = new Range<DateTime> { Min = dateRange.Item1, Max = dateRange.Item2} };
                    
                   
                    var statistic = await _channelCrawlService.Statistic(filter);
                    return JsonResponse(200, SUCCESS, statistic);
                }
            }
            catch (Exception e)
            {

                if (e.Message.Contains(NOT_FOUND))
                {
                    return JsonResponse(400, DELETE_FAILED, e.Message);
                }
                return JsonResponse(401, UNAUTHORIZE, e.Message);

            }
        }


        [CustomAuth(RoleAuthorize.ROLE_MEMBER)]
        [HttpGet("compare")]
        public async Task<IActionResult> CompareChannel([FromQuery] CompareDto dto)
        {
            try
            {
                var result = await _channelCrawlService.CompareChannel(dto);
                return JsonResponse(200, SUCCESS, result);
            }
            catch (Exception e)
            {

                if (e.Message.Contains(NOT_FOUND))
                {
                    return JsonResponse(400, DELETE_FAILED, e.Message);
                }
                return JsonResponse(401, UNAUTHORIZE, e.Message);

            }
        }
    }
}
