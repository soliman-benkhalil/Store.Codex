
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Store.Codex.APIs.Errors;
using Store.Codex.APIs.Helper;
using Store.Codex.APIs.Middlewares;
using Store.Codex.Core;
using Store.Codex.Core.Mapping;
using Store.Codex.Core.Services.Contract;
using Store.Codex.Repository;
using Store.Codex.Repository.Data;
using Store.Codex.Repository.Data.Contexts;
using Store.Codex.Service.Services.Products;

namespace Store.Codex.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDependency(builder.Configuration);

            var app = builder.Build();

            await app.ConfigureMiddlewareAsync();

            app.Run();
        }
    }
}
