﻿using Business.Config;
using Business.Constants;
using Business.Service.TransactiondepositService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Constant;
using static Business.Utils.ResponseFormat;
using static Business.Constants.ResponseMsg;
using DataAccess.Models.TransectionDepositModel;

namespace WebAPI.Controllers
{
    [Route(ApiPath.TRANSACTIONDEPOSIT_PATH)]
    [ApiController]
    [CustomAuth(RoleAuthorize.ROLE_MEMBER)]
    public class TransactionDepositController : ControllerBase
    {
        private readonly ITransactiondepositService _transactionDepositService;

        public TransactionDepositController(ITransactiondepositService transactionDepositService)
        {
            _transactionDepositService = transactionDepositService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryId(int id)
        {
            try
            {
                var result = await _transactionDepositService.GetById(id);
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


        [HttpPost()]
        public async Task<IActionResult> InsertCategoryId([FromForm] InsertTransactionDepositDto dto)
        {
            try
            {
                var result = await _transactionDepositService.Insert(dto);
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

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] TransactionDepositPaging paging)
        {
            try
            {
                var result = await _transactionDepositService.SearchAsync(paging);
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromForm] UpdateTransactionDepositDto dto)
        {
            try
            {
                var result = await _transactionDepositService.Update(id, dto);
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
        public async Task<IActionResult> DeleteCategoryById(int id)
        {
            try
            {
                var result = await _transactionDepositService.Delete(id);
                return JsonResponse(200, DELETE_SUCCESS, result);
            }
            catch (Exception e)
            {

                if (e.Message.Contains(DELETE_FAILED))
                {
                    return JsonResponse(400, DELETE_FAILED, e.Message);
                }
                return JsonResponse(401, UNAUTHORIZE, e.Message);

            }
        }
    }
}
