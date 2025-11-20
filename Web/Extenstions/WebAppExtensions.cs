using Domain.Contracts;
using  Web.Middlewares;

namespace Web.Extenstions
{
    public static class WebAppExtensions
    {
        public static async Task<WebApplication> ConfigureMiddlewares (this WebApplication app)
        {
            await app.InititalizeDatabaseAsync();

            app.ConfigureCustomeMiddlewares();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowAll");

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            return app;
        }

        private static WebApplication ConfigureCustomeMiddlewares(this WebApplication app)
        {
            app.UseMiddleware<GlobalErrorHandlingMiddleware>();
            return app;
        }

        private static async Task<WebApplication> InititalizeDatabaseAsync(this WebApplication app)
        {
            var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            await dbInitializer.InitializeAsync();
            await dbInitializer.InitializeIdentityAsync();

            return app;
        }
    }
}
