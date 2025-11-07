using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using Persistence.Identity;
using Services;
using Shared;
using Shared.ErrorModels;
using Web.Middlewares;

namespace Web.Extenstions
{  
    public static class Extenstions
    {

        public static IServiceCollection RegisterAllServices(this IServiceCollection services , IConfiguration configuration)
        {
            // Add services to the container.


            services.RegisterBuiltInServices();

            services.RegisterSwaggerServices();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            services.AddOpenApi();




            services.AddInfrastructureServices(configuration);
            services.AddIdentityServices();


            services.AddApplicationServices(configuration);
          



            services.ConfigureServices();

            services.ConfigureJwtServices(configuration);







            return services;


        }






        private static IServiceCollection RegisterBuiltInServices(this IServiceCollection services)
        {


            services.AddControllers();

            return services;


        }

        private static IServiceCollection ConfigureJwtServices(this IServiceCollection services , IConfiguration configuration)
        {

            var jwtoptions = configuration.GetSection("JwtOptions").Get<JwtOptions>();

            services.AddAuthentication(

                options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;


                }
                ).AddJwtBearer(options =>


                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {

                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,

                        ValidIssuer = jwtoptions.issuer,
                        ValidAudience = jwtoptions.audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtoptions.SecretKey))

                    };


                }

                );





            return services;


        }


        private static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {


            services.AddIdentity<AppUser, IdentityRole>(

                options =>
                {

                    options.User.RequireUniqueEmail = true;

                }
                
                ).AddEntityFrameworkStores<StoreIdentityDbContext>();



            return services;


        }

        private static IServiceCollection RegisterSwaggerServices(this IServiceCollection services)
        {



            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;


        }
        
        private static IServiceCollection ConfigureServices(this IServiceCollection services)
        {



            services.Configure<ApiBehaviorOptions>(config =>
            {


                config.InvalidModelStateResponseFactory = (actionContext) =>
                {

                    var errors = actionContext.ModelState.Where(m => m.Value.Errors.Any())
                    .Select(m => new ValidationError()
                    {

                        FieldName = m.Key,
                        Errors = m.Value.Errors.Select(error => error.ErrorMessage)

                    });



                    var response = new ValidationErrorResponse()

                    {

                        Errors = errors

                    };



                    return new BadRequestObjectResult(response);
                };


            });





            return services;


        }




        public static async Task<WebApplication> ConfigureMiddlewares(this WebApplication app)
        {
           await app.InitializeDatabaseAsync();


            app.UseGlobalErrorHandling();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //   app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            //middleware of using static files 

            app.UseStaticFiles();

            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();



            return app;
        }



        private static async Task<WebApplication> InitializeDatabaseAsync(this WebApplication app) 
        {

            #region Initialize db
            // take object of type of scope service 
            using var scope = app.Services.CreateScope();
            // take object of type IDbInitializer across scope object

            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            //ask clr to create object of type of DbInitializer



            await dbInitializer.InitializeAsync();
            await dbInitializer.InitializeIdentityAsync();

            #endregion

            return app;

        }

        private static WebApplication UseGlobalErrorHandling(this WebApplication app)
        {


            app.UseMiddleware<GlobalErrorHandlingMiddleware>();
            return app;

        }


    }
}
