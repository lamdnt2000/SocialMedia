using Business.Config;
using Business.Constants;
using Business.Service.WatchlistService;
using DataAccess.Models.WatchlistModel;
using Microsoft.AspNetCore.Mvc;
using static Business.Utils.ResponseFormat;
using static Business.Constants.ResponseMsg;
using System.Threading.Tasks;
using System;
using Business.Utils;
using DataAccess.Models.Pagination;
using System.Collections.Generic;
using DataAccess;
using AutoFilterer.Extensions;
using System.Linq;
using DataAccess.Models.PlatFormModel;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Controllers
{

    [Route(Constant.ApiPath.WATCHLIST_PATH)]
    [ApiController]
    [CustomAuth(RoleAuthorize.ROLE_MEMBER)]
    public class WatchlistController : ControllerBase
    {
        private readonly IWatchlistService _watchlistService;

        public WatchlistController(IWatchlistService watchlistService)
        {
            _watchlistService = watchlistService;

        }

        [HttpPost]
        public async Task<IActionResult> InsertWatchlist([FromBody] InsertWatchlistDto dto)
        {
            try
            {
                var result = await _watchlistService.Insert(dto);
                return JsonResponse(201, INSERT_SUCCESS, new { id = result });
            }
            catch (Exception e)
            {

                if (e.Message.Contains(DUPLICATED))
                {
                    return JsonResponse(400, INSERT_FAILED, e.Message);
                }
                if (e.Message.Contains(NOT_FOUND))
                {
                    return JsonResponse(400, INSERT_FAILED, e.Message);
                }
                return JsonResponse(401, UNAUTHORIZE, e.Message);

            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWatchlistById(int id)
        {
            try
            {
                var result = await _watchlistService.Delete(id);
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
        public async Task<IActionResult> SearchAsync(string name,[Required] int platformId, [FromQuery] WatchlistPaging paging)
        {

            try
            {
                var result = await _watchlistService.SearchAsync(name, platformId, paging);
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
