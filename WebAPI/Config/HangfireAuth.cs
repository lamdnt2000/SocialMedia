using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace WebAPI.Config
{
    public class HangfireAuth : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            return httpContext.User.Identity.IsAuthenticated;
        }
    }
}
