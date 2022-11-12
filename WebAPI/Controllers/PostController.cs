using Business.Config;
using Business.Constants;
using DataAccess.Models.ChannelCrawlModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using WebAPI.Constant;
using Business.Service.PostService;
using static Business.Utils.ResponseFormat;
using static Business.Constants.ResponseMsg;
using DataAccess.Models.PostCrawlModel;

namespace WebAPI.Controllers
{
    [Route(ApiPath.POST_PATH)]
    [ApiController]
    [CustomAuth(RoleAuthorize.ROLE_ADMIN)]
    public class PostController : ControllerBase
    {
        private readonly IPostCrawlService _postCrawlService;

        public PostController(IPostCrawlService postCrawlService)
        {
            _postCrawlService = postCrawlService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            try
            {
                var result = await _postCrawlService.GetById(id);
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
        public async Task<IActionResult> InsertPost([FromForm] InsertPostCrawlDto dto)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _postCrawlService.Insert(dto);
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
        public async Task<IActionResult> UpdatePost(int id, [FromForm] UpdatePostCrawlDto dto)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _postCrawlService.Update(id, dto);
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
        public async Task<IActionResult> DeletePostById(int id)
        {
            try
            {
                var result = await _postCrawlService.Delete(id);
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
