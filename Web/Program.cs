
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Data.Contexts;
using Domain.Contracts;
using Persistence.Repositories;
using Services.Mapping.Products;
using Services.Abstractions;
using Services;
using Services.Specification;
using Web.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using Shared.ErrorModels;
using Web.Extenstions;

namespace Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.RegisterAllServices(builder.Configuration);




            var app = builder.Build();

            await app.ConfigureMiddlewares();

            await app.ConfigureMiddlewares();

            app.Run();
        }
    }
}
