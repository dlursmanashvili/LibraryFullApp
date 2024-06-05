namespace Interface
{
    public static class UseSwaggerExtensions
    {
        public static IApplicationBuilder StartSwagger(this IApplicationBuilder app, string name, string url = "/api/swagger/v1/swagger.json")
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint(url, name));
            return app;
        }
    }
}
