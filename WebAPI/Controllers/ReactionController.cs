using Business.Config;
using Business.Constants;
using Business.Service.ReactionService;
using DataAccess.Models.PostCrawlModel;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using WebAPI.Constant;
using static Business.Utils.ResponseFormat;
using static Business.Constants.ResponseMsg;
using DataAccess.Models.ReactionModel;

namespace WebAPI.Controllers
{
    [Route(ApiPath.REACTION_PATH)]
    [ApiController]
    [CustomAuth(RoleAuthorize.ROLE_ADMIN)]
    public class ReactionController : ControllerBase
    {
        private readonly IReactionService _reactionService;

        public ReactionController(IReactionService reactionService)
        {
            _reactionService = reactionService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReactionById(int id)
        {
            try
            {
                var result = await _reactionService.GetById(id);
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


        [HttpPost]
        public async Task<IActionResult> InsertReaction([FromForm] InsertReactionDto dto)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _reactionService.Insert(dto);
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
        public async Task<IActionResult> UpdateReaction(int id, [FromForm] UpdateReactionDto dto)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _reactionService.Update(id, dto);
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
        public async Task<IActionResult> DeleteReactionById(int id)
        {
            try
            {
                var result = await _reactionService.Delete(id);
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
