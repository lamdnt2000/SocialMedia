using Microsoft.AspNetCore.Mvc.Filters;
using System;

using static Business.Utils.ResponseFormat;

namespace Business.Config
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomAuth : Attribute, IAuthorizationFilter
    {
        public string Roles;

        public CustomAuth(params string[] roles)
        {
            Roles = string.Join(",", roles);
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //get User info in context
            var account = context.HttpContext.Items["User"];
            if (account != null)
            {
                string role = account.GetType().GetProperty("role")?.GetValue(account, null)?.ToString();
                string[] arr = Roles.Split(",");
                bool isValid = false;
                foreach (var tmp in arr)
                {
                    if (tmp.Trim().ToLower().Equals(role?.ToLower()))
                    {
                        isValid = true;
                        break;
                    }
                }
                if (!isValid)
                {
                    context.Result = JsonResponse(403, "You don't have permission to access!", null);
                }
            }
        }
    }
}
