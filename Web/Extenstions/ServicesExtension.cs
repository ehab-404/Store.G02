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
    public static class ServicesExtension
    {
        public static IServiceCollection RegisterAllServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddInfrastructureServices(configuration);

            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;

            }).AddEntityFrameworkStores<StoreIdentityDbContext>();

            services.AddApplicationServices(configuration);

            services.ConfigureServices(configuration);

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
                });
            });

            return services;
        }


        private static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure Validateion Error using ModelState (to get errors) 
            services.Configure<ApiBehaviorOptions>(config =>
            {
                config.InvalidModelStateResponseFactory = actionContext =>
                {
                    // ModelState is someething like this:
                    // {
                    //      "FieldName" : {
                    //          AttemptedValue = "value from request",
                    //          RawValue = "raw input",
                    //          Errors = [list of ModelError],
                    //          ValidationState = Valid / Invalid
                    //      }
                    // }
                    var errors = actionContext.ModelState.Where(m => m.Value.Errors.Any()).Select(m => new ValidationError()
                    {
                        Field = m.Key,
                        Errors = m.Value.Errors.Select(e => e.ErrorMessage)
                    });

                    var response = new ValidationErrorResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(response);
                };
            });

            services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var jwtOptions = configuration.GetSection("JwtOptions").Get<JwtOptions>();

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                };
            });

            return services;
        }
    }
}

