using CRM.domain.Model;
using Serilog.Context;

namespace CRM.Middleware
{
    public class LogContextMiddleware
    {
        private readonly RequestDelegate _next;

        public LogContextMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userIdClaims = context.User.Claims.FirstOrDefault(c => c.Type == "UserId");
                if (userIdClaims != null)
                {
                    var userId = userIdClaims.Value;
                    using (LogContext.PushProperty("UserId", userId))
                    {
                        await _next(context);
                    }
                    return;
                }
                else
                {
                    using (LogContext.PushProperty("UserId", "Guest"))
                    {
                        await _next(context);
                    }
                    return;
                }
            }
            await _next(context);
        }
    }
}
