using Store.Codex.APIs.Middlewares;
using Store.Codex.Repository.Data.Contexts;
using Store.Codex.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Store.Codex.Core.Entities.Identity;
using Store.Codex.Repository.Identity.Contexts;
using Store.Codex.Repository.Identity.DataSeed;

namespace Store.Codex.APIs.Helper
{
    public static class ConfigureMiddlewares
    {

        public static async Task<WebApplication> ConfigureMiddlewareAsync(this WebApplication app)
        {
            await ApplyMigrationsAsync(app);
            ConfigureSwagger(app);
            ConfigureUserDefinedMiddleware(app);
            ConfigureErrorHandling(app);
            ConfigureHttpPipeline(app);

            return app;
        }

        private static async Task ApplyMigrationsAsync(this WebApplication app)
        {
            //StoreDbContext context = new StoreDbContext(); // there is an error here because i have to make an object from StoreDbContext so i will ask him in a different way
            //context.Database.MigrateAsync(); // this line apply any migrations that is not applied yet and if the db does not exist it creates it (Update Databaes By the code)

            using var scope = app.Services.CreateScope(); // here i make a container from the services that work by scope lifetime
                                                          // and it is unmanaged resource to i used using (mean i need to close it after operation
                                                          // and it does not close by default) -> AddDbContext<StoreDbContext> by default is scope

            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<StoreDbContext>();
            var Identitycontext = services.GetRequiredService<StoreIdentityDbContext>();
            var userManager = services.GetRequiredService<UserManager<AppUser>>();
            //await context.Database.MigrateAsync(); // it applies the migrations in DB and create DB if it is not existed .... but what if it is existed so try and catch

            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                await context.Database.MigrateAsync();
                await StoreDbContextSeed.SeedAsync(context);
                await Identitycontext.Database.MigrateAsync();
                await StoreIdentityDbContextSeed.SeedAppUserAsync(userManager);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>(); // we can make a message by print but it will appear on the command but use logger is better for make the message more expressed
                // and determine the name of the class that i make the logger in it and it is here called program not the Main because Main is a function not a class because the class is the responisble to make the console screen

                logger.LogError(ex, "There are problems during apply migrations !");

            }
        }

        private static void ConfigureSwagger(this WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
        }

        private static void ConfigureUserDefinedMiddleware(this WebApplication app)
        {
            app.UseMiddleware<ExceptionMiddleware>(); // Here i Configure User-Defined MiddleWare [ExceptionMiddleware]
        }        

        private static void ConfigureErrorHandling(this WebApplication app)
        {
            app.UseStatusCodePagesWithReExecute("/error/{0}"); // here a built-in middleware taht redirect the user to this action when write an invalid route
            // the {0} valud of the code 0 -> first item 
        }        

        private static void ConfigureHttpPipeline(this WebApplication app)
        {
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();
        }
    }
}
