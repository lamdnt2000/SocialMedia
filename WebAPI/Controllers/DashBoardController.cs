using Microsoft.AspNetCore.Mvc;
using static Business.Utils.ResponseFormat;
using static Business.Constants.ResponseMsg;
using Business.Service.DashboardService;
using Business.Config;
using Business.Constants;
using System.Threading.Tasks;
using System;
using DataAccess.Models.LoginUser;
using Hangfire;
using Hangfire.Storage;

namespace WebAPI.Controllers
{
    [Route(Constant.ApiPath.DASHBOARD_PATH)]
    [ApiController]
    public class DashBoardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashBoardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [CustomAuth(RoleAuthorize.ROLE_ADMIN)]
        [HttpGet("admin")]
        public async Task<IActionResult> GetAdminDashboard()
        {
            try
            {
                var result = await _dashboardService.ShowDashBoardAdmin();
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

        [CustomAuth(RoleAuthorize.ROLE_MEMBER)]
        [HttpGet("user")]
        public async Task<IActionResult> GetUserDashboard()
        {
            try
            {
                var result = await _dashboardService.ShowDashboardUser();
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

        [CustomAuth(RoleAuthorize.ROLE_ADMIN)]
        [HttpGet("admin/user")]
        public async Task<IActionResult> SearchUserAsync([FromQuery] UserPaging paging)
        {
            try
            {
                var result = await _dashboardService.SearchUserAsync(paging);
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
    }
}
