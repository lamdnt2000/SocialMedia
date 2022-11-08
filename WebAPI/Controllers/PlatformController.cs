﻿using Business.Config;
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

namespace WebAPI.Controllers
{
    [Route(ApiPath.PLATFORM_PATH)]
    [ApiController]
    [CustomAuth(RoleAuthorize.ROLE_ADMIN)]
    public class PlatformController : ControllerBase
    {
        private readonly IPlatformService _platformService;
        private readonly ICategoryService _categoryService;

        public PlatformController(IPlatformService platformService, ICategoryService categoryService)
        {
            _platformService = platformService;
            _categoryService = categoryService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlatformById(int id)
        {
            try
            {


                var result = await _platformService.GetById(id);
                result.Categories = null;
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

        [HttpPost()]
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