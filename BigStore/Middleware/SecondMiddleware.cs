namespace BigStore.Middleware
{
    public class SecondMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Path == "/xxx")
            {
                var dataToFirstMiddleware = context.Items["DataFirstMiddelware"];
                if (dataToFirstMiddleware != null)
                {
                    context.Response.Headers.Add("Secondmilldeware", "Ban khong duoc phep truy cap "+ dataToFirstMiddleware);
                }
                await context.Response.WriteAsync("Khong duoc phep try cap");
            } else
            {
                await next(context);
            }
        }
    }
}
