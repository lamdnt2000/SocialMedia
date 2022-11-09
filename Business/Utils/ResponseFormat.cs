using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Business.Utils
{
    public static class ResponseFormat
    {
        public static JsonResult JsonResponse(int statusCode, string message, Object data)
        {
            Object obj;
            var options = new JsonSerializerOptions
            {
                IncludeFields = true,
                IgnoreNullValues = true,
              
            };
            if (data != null)
            {
                obj = new { statusCode = statusCode, message = message, data = data };
            }
            else
            {
                obj = new { statusCode = statusCode, message = message };
            }
            return new JsonResult(obj, options) { StatusCode = statusCode };
        }

        public static JsonResult ErrorResponse(string detail)
        {
            return JsonResponse(500, "Internal Server Error!", !string.IsNullOrEmpty(detail) ? new { detail = detail } : null);
        }
    }
}
