
using CoriaToDo.API.Data;
using CoriaToDo.API.Feature.Todo.Services;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace CoriaToDo.API
{
    public class Program
    {
        private const string CORS_FILTER_NAME = "AllowMyOrigin";

        public static void Main(string[] args)
        {
            // Load environment variables from .env file (if it exists)
            DotEnv.Load();

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            ConfigureDependencyInjection(builder.Services);
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ToDoDbContext>(options =>
               options.UseNpgsql(ChangeDbHostNameIfNeeded(builder.Configuration.GetConnectionString("PostgresDefaultConnection"))));

            builder.Services.AddAutoMapper(typeof(Program).Assembly);

            builder.Services.AddCors(c =>
            {
                c.AddPolicy(CORS_FILTER_NAME, option =>
                {
                    option.WithOrigins(builder.Configuration.GetValue<string>("FrontEndHost"))
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            MigrateDb(app);

            app.UseCors(CORS_FILTER_NAME);

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        private static void ConfigureDependencyInjection(IServiceCollection services)
        {
            services.AddSingleton<SessionContext>();
            services.AddHttpContextAccessor();
        }

        public static string ChangeDbHostNameIfNeeded(string connString)
        {
            var dbHost = Environment.GetEnvironmentVariable("DB_HOST_NAME");
            if (!string.IsNullOrEmpty(dbHost))
            {
                connString = connString.Replace("localhost", dbHost);
            }
            return connString;
        }

        static void MigrateDb(WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            scope.ServiceProvider.GetService<ToDoDbContext>().Database.Migrate();
        }
    }
}