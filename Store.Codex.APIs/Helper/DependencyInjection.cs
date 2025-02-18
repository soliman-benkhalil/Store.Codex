using Microsoft.EntityFrameworkCore;
using Store.Codex.Core.Services.Contract;
using Store.Codex.Core;
using Store.Codex.Repository;
using Store.Codex.Repository.Data.Contexts;
using Store.Codex.Service.Services.Products;
using Store.Codex.Core.Mapping;
using Microsoft.AspNetCore.Mvc;
using Store.Codex.APIs.Errors;
using Store.Codex.Core.Repositories.Contract;
using Store.Codex.Repository.Repositories;
using Store.Codex.Core.Mapping.Basket;
using StackExchange.Redis;
using Store.Codex.Service.Services.Basket;
using Store.Codex.Service.Caches;
using Store.Codex.Repository.Identity.Contexts;
using Microsoft.AspNetCore.Identity;
using Store.Codex.Core.Entities.Identity;
using Store.Codex.Service.Tokens;
using Store.Codex.Service.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Store.Codex.Core.Mapping.Auth;
using Store.Codex.Core.Mapping.Orders;
using Store.Codex.Service.Orders;
using Store.Codex.Service.Payments;

namespace Store.Codex.APIs.Helper
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddDependency(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddBuiltInService();
            services.AddSwaggerService();
            services.AddDbContextService(configuration);
            services.AddUserDefinedService();
            services.AddAutoMapperService(configuration);
            services.ConfigureInvalidModelStateResponseService();
            services.AddRedisService(configuration);
            services.AddIdentityService();
            services.AddAuthenticationService(configuration);

            return services;

        }

        private static IServiceCollection AddBuiltInService(this IServiceCollection  services)
        {
            services.AddControllers();
            return services;
        }

        private static IServiceCollection AddSwaggerService(this IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer(); // it is to let the swagger to c all endpoints in the all controllers   
            services.AddSwaggerGen
                ( S => S.CustomSchemaIds(type => type.FullName) // To avoid the nameing conflict between the addres between the auth AddressDto and Order AddressDto
                );
            return services;
        }

        private static IServiceCollection AddDbContextService(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddDbContext<StoreDbContext>(optoin =>
            {
                optoin.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddDbContext<StoreIdentityDbContext>(optoin =>
            {
                optoin.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
            });
            return services;
        }

        private static IServiceCollection AddUserDefinedService(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBasketRepository,BasketRepository>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<ICacheService, CachedService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService, PaymentService>();
            return services;
        }

        private static IServiceCollection AddAutoMapperService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(M => M.AddProfile(new ProductProfile(configuration)));
            services.AddAutoMapper(M => M.AddProfile(new BasketProfile()));
            services.AddAutoMapper(M => M.AddProfile(new AuthProfile()));
            services.AddAutoMapper(M => M.AddProfile(new OrderProfile(configuration)));
            

            return services;
        }

        private static IServiceCollection ConfigureInvalidModelStateResponseService(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>  // ApiBehaviorOptions -> it represents the behavior response of api
            {
                options.InvalidModelStateResponseFactory = (ActionContext) => // InvalidModelStateResponseFactory -> it is the object that is responsible of the shape of any error related with the model state
                {
                    var errors = ActionContext.ModelState.Where(P => P.Value.Errors.Count() > 0) // condtion where there are errors or not
                                            .SelectMany(P => P.Value.Errors)
                                            .Select(E => E.ErrorMessage)
                                            .ToArray();
                    var response = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(response); // it is the solution here for the error got by this line BadRequest(response)
                    // BadRequest(response) -> we have problem with the type of badrequest we cant use badrequest as a method
                    // and that is why we used it before because we used it in a controller that inherit from the controller base and it is not happened here 
                };
            });
            return services;
        }

        private static IServiceCollection AddRedisService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionMultiplexer>
            (
                (serviceProvider) =>
                {
                    var connect = configuration.GetConnectionString("Redis");

                    return ConnectionMultiplexer.Connect(connect);
                }
            );
            return services;
        }


        private static IServiceCollection AddIdentityService(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, IdentityRole>()
                    .AddEntityFrameworkStores<StoreIdentityDbContext>(); // Allow Dependency Injection Fro All Identity Built-in Services
            return services;
        }

        private static IServiceCollection AddAuthenticationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
            });
            return services;
        }
    }
}
