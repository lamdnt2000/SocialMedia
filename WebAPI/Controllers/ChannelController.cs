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

namespace WebAPI.Controllers
{
    [Route(ApiPath.CHANNEL_PATH)]
    [ApiController]
    [CustomAuth(RoleAuthorize.ROLE_ADMIN)]
    public class ChannelController : ControllerBase
    {
        private readonly IChannelCrawlService _channelCrawlService;
        private readonly IScheduleSocial _scheduleSocial;

        public ChannelController(IChannelCrawlService channelCrawlService, IScheduleSocial schedule)
        {
            _channelCrawlService = channelCrawlService;
            _scheduleSocial = schedule;
        }

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

        /*[HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PlatformPaging paging)
        {
            try
            {


                var result = await _platformService.SearchAsync(paging);
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
        }*/

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

        [HttpGet]
        public async Task<IActionResult> GetChannelByUrl([FromQuery] string url)
        {
            try
            {
                var channelId = await _channelCrawlService.FindChannelByPlatformAndUserId(url);
                if (channelId == 0)
                {
                    var result = _scheduleSocial.ValidateUrl(url);
                    var jobId = BackgroundJob.Enqueue(() => _scheduleSocial.FetchChannelJob(result.Item1, result.Item2));
                    BackgroundJob.ContinueJobWith(jobId, () => _scheduleSocial.CreateChannelJob(result.Item1, result.Item2));
                    return JsonResponse(200, NOT_FOUND, "Waiting loading data");
                }
                else
                {
                    var dateRange = DateUtil.GenerateDateInRange(1);
                    ChannelFilter filter = new ChannelFilter() { Id = channelId, CreatedTime = new Range<DateTime> { Min = dateRange.Item1, Max = dateRange.Item2} };
                    
                   
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
    }
}
