using Business.Config;
using Business.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Constant;
using static Business.Utils.ResponseFormat;
using static Business.Constants.ResponseMsg;
using Business.Service.ChannelRecordService;
using DataAccess.Models.ChannelCrawlModel;
using System.Threading.Tasks;
using System;
using DataAccess.Models.ChannelRecordModel;

namespace WebAPI.Controllers
{
    [Route(ApiPath.RECORD_PATH)]
    [ApiController]
    [CustomAuth(RoleAuthorize.ROLE_ADMIN)]
    public class RecordController : ControllerBase
    {
        private readonly IChannelRecordService _channelRecordService;

        public RecordController(IChannelRecordService channelRecordService)
        {
            _channelRecordService = channelRecordService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRecordById(int id)
        {
            try
            {
                var result = await _channelRecordService.GetById(id);
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
        public async Task<IActionResult> InsertRecord([FromForm] InsertChannelRecordDto dto)
        {
            try
            {
                var result = await _channelRecordService.Insert(dto);
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
        public async Task<IActionResult> UpdateRecord(int id, [FromForm] UpdateChannelRecordDto dto)
        {
            try
            {
                var result = await _channelRecordService.Update(id, dto);
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
                var result = await _channelRecordService.Delete(id);
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
    }
}
