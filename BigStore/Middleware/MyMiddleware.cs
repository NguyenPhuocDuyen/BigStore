namespace BigStore.Middleware
{
    public static class MyMiddleware
    {
        public static void UseMyMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<FirstMiddleware>();
            //app.UseMiddleware<SecondMiddleware>();
        }
    }
}
