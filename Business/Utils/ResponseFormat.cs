using Microsoft.AspNetCore.Mvc;
using System;

namespace Business.Utils
{
    public static class ResponseFormat
    {
        public static JsonResult JsonResponse(int statusCode, string message, Object data)
        {
            Object obj;
            if (data != null)
            {
                obj = new { statusCode = statusCode, message = message, data = data };
            }
            else
            {
                obj = new { statusCode = statusCode, message = message };
            }
            return new JsonResult(obj) { StatusCode = statusCode };
        }

        public static JsonResult ErrorResponse(string detail)
        {
            return JsonResponse(500, "Internal Server Error!", !string.IsNullOrEmpty(detail) ? new { detail = detail } : null);
        }
    }
}
