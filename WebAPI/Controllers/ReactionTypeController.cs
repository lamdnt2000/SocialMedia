using Business.Service.ReactionTypeService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Constant;
using Business.Config;
using Business.Constants;
using DataAccess.Models.ReactionTypeModel;
using static Business.Utils.ResponseFormat;
using static Business.Constants.ResponseMsg;

namespace WebAPI.Controllers
{
    [Route(ApiPath.REACTIONTYPE_PATH)]
    [ApiController]
    [CustomAuth(RoleAuthorize.ROLE_ADMIN)]
    public class ReactionTypeController : ControllerBase
    {
        private readonly IReactionTypeService _reactionTypeService;

        public ReactionTypeController(IReactionTypeService reactionTypeService)
        {
            _reactionTypeService = reactionTypeService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryId(int id)
        {
            try
            {
                var result = await _reactionTypeService.GetById(id);
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

        [HttpPost()]
        public async Task<IActionResult> InsertCategoryId([FromForm] InsertReactionType dto)
        {
            try
            {
                var result = await _reactionTypeService.Insert(dto);
                return JsonResponse(200, SUCCESS, new { id = result });
            }
            catch (Exception e)
            {

                if (e.Message.Contains(DUPLICATED))
                {
                    return JsonResponse(400, DUPLICATED, e.Message);
                }
                if (e.Message.Contains(NOT_FOUND))
                {
                    return JsonResponse(400, NOT_FOUND, e.Message);
                }
                return JsonResponse(401, UNAUTHORIZE, e.Message);

            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromForm] UpdateReactionTypeDto dto)
        {
            try
            {
                var result = await _reactionTypeService.Update(id, dto);
                return JsonResponse(200, SUCCESS, new { id = result });
            }
            catch (Exception e)
            {

                if (e.Message.Contains(DUPLICATED))
                {
                    return JsonResponse(400, DUPLICATED, e.Message);
                }
                if (e.Message.Contains(NOT_FOUND))
                {
                    return JsonResponse(400, NOT_FOUND, e.Message);
                }

                return JsonResponse(401, UNAUTHORIZE, e.Message);

            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryById(int id)
        {
            try
            {
                var result = await _reactionTypeService.Delete(id);
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
