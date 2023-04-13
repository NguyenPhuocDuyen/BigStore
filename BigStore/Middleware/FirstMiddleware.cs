namespace BigStore.Middleware
{
    public class FirstMiddleware
    {
        private readonly RequestDelegate _next;

        public FirstMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Items.Add("DataFirstMiddelware", "Day la du lieu tu FirstMiddleware");
            await _next(context);
        }
    }
}
