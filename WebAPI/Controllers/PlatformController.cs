using Business.Config;
using Business.Constants;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using WebAPI.Constant;
using static Business.Utils.ResponseFormat;
using static Business.Constants.ResponseMsg;
using Business.Service.PlatformService;
using DataAccess.Models.PlatFormModel;
using Business.Repository.CategoryRepo;
using Business.Service.CategoryService;
using DataAccess.Models.CategoryModel;
using Business.Service.ReactionTypeService;
using DataAccess.Models.ReactionTypeModel;

namespace WebAPI.Controllers
{
    [Route(ApiPath.PLATFORM_PATH)]
    [ApiController]
    
    public class PlatformController : ControllerBase
    {
        private readonly IPlatformService _platformService;
        private readonly ICategoryService _categoryService;
        private readonly IReactionTypeService _reactionTypeService;
        public PlatformController(IPlatformService platformService, ICategoryService categoryService, IReactionTypeService reactionTypeService)
        {
            _platformService = platformService;
            _categoryService = categoryService;
            _reactionTypeService = reactionTypeService;
        }

        [CustomAuth(RoleAuthorize.ROLE_ADMIN + "," + RoleAuthorize.ROLE_MEMBER)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlatformById(int id)
        {
            try
            {


                var result = await _platformService.GetById(id);
                if (result != null)
                {
                    result.Categories = null;
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
        [HttpGet]
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
        }

        [CustomAuth(RoleAuthorize.ROLE_ADMIN + "," + RoleAuthorize.ROLE_MEMBER)]
        [HttpGet("categories")]
        public async Task<IActionResult> GetAllCategory([FromQuery] CategoryPaging paging)
        {
            try
            {


                var result = await _categoryService.SearchAsync(paging);
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
        [HttpGet("reactiontypes")]
        public async Task<IActionResult> GetAllReactionType([FromQuery] ReactionTypePaging paging)
        {
            try
            {


                var result = await _reactionTypeService.SearchAsync(paging);
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
        public async Task<IActionResult> InsertPlatformId([FromForm] InsertPlatformDto dto)
        {
            try
            {
                var result = await _platformService.Insert(dto);
                return JsonResponse(201, INSERT_SUCCESS, new { id = result });
            }
            catch (Exception e)
            {

                if (e.Message.Contains(DUPLICATED))
                {
                    return JsonResponse(400, INSERT_FAILED, e.Message);
                }
                return JsonResponse(401, UNAUTHORIZE, e.Message);

            }
        }

        [CustomAuth(RoleAuthorize.ROLE_ADMIN)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlatfrom(int id, [FromForm] UpdatePlatformDto dto)
        {
            try
            {
                var result = await _platformService.Update(id, dto);
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

                return JsonResponse(401, UNAUTHORIZE, e.Message);

            }
        }

        [CustomAuth(RoleAuthorize.ROLE_ADMIN)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlatformById(int id)
        {
            try
            {
                var result = await _platformService.Delete(id);
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
        [HttpGet]
        [Route("{id}/categories")]
        public async Task<IActionResult> GetCollectionCategory(int id)
        {
            try
            {
                var result = await _platformService.GetById(id, true);
                return JsonResponse(200, SUCCESS, result);
            }
            catch (Exception e)
            {

                if (e.Message.Contains(NOT_FOUND))
                {
                    return JsonResponse(400, NOT_FOUND, "");
                }
                return JsonResponse(401, UNAUTHORIZE, "");

            }
        }
    }
}
