using Business.Config;
using Business.Constants;
using Business.Service.OfferService;
using Business.Service.SubscriptionService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Constant;
using static Business.Utils.ResponseFormat;
using static Business.Constants.ResponseMsg;
using DataAccess.Models.OfferModel;
using DataAccess.Models.SubscriptionModel;

namespace WebAPI.Controllers
{
    [Route(ApiPath.OFFER_PATH)]
    [ApiController]
    [CustomAuth(RoleAuthorize.ROLE_MEMBER)]
    public class OfferController : ControllerBase
    {
        private readonly IOfferService _offerService;
        private readonly ISubscriptionService _subscriptionService;

        public OfferController(IOfferService offerService, ISubscriptionService subscriptionService)
        {
            _offerService = offerService;
            _subscriptionService = subscriptionService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOfferId(int id)
        {
            try
            {
                var result = await _offerService.GetById(id);
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
        public async Task<IActionResult> GetAll([FromQuery] OfferPaging paging)
        {
            try
            {


                var result = await _offerService.SearchAsync(paging);
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


        [HttpGet("Subscriptions")]
        public async Task<IActionResult> GetAllSubscription([FromQuery] SubscriptionPaging paging)
        {
            try
            {


                var result = await _subscriptionService.SearchAsync(paging);
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
        public async Task<IActionResult> InsertOfferId([FromForm] InsertOfferDto dto)
        {
            try
            {
                var result = await _offerService.Insert(dto);
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
        public async Task<IActionResult> UpdateOffer(int id, [FromForm] UpdateOfferDto dto)
        {
            try
            {
                var result = await _offerService.Update(id, dto);
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
        public async Task<IActionResult> DeleteOfferById(int id)
        {
            try
            {
                var result = await _offerService.Delete(id);
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
        [Route("{id}/Subscritptions")]
        public async Task<IActionResult> GetCollectionSubscritption(int id)
        {
            try
            {
                var result = await _offerService.GetById(id);
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
