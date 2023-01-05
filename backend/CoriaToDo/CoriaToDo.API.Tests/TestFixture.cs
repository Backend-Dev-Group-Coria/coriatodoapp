using System.Net.Http;
using CoriaToDo.API.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CoriaToDo.API.Tests
{
    public class TestFixture : IDisposable
    {
        HttpClient _httpClient;
        ToDoDbContext _dbContext;
        IConfigurationRoot _configurationRoot;

        public TestFixture()
        {
            _configurationRoot = InitializeConfiguration();
            _httpClient = CreateHttpClient(_configurationRoot);
            _dbContext = GetDatabaseContext(_configurationRoot);
        }

        public ToDoDbContext DbContext { get { return _dbContext; } }
        public HttpClient HttpClient { get { return _httpClient; } }

        private ToDoDbContext GetDatabaseContext(IConfigurationRoot configurationRoot)
        {
            var options = new DbContextOptionsBuilder<ToDoDbContext>()
                .UseNpgsql(Program.ChangeDbHostNameIfNeeded(configurationRoot.GetConnectionString("PostgresDefaultConnection"))).Options;
            var dbContext = new ToDoDbContext(options);
            dbContext.Database.Migrate();
            return dbContext;
        }

        private HttpClient CreateHttpClient(IConfigurationRoot configurationRoot)
        {
            var application = new WebApplicationFactory<Program>()
                    .WithWebHostBuilder(builder =>
                    {
                        builder.UseEnvironment(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Test");
                        builder.UseConfiguration(configurationRoot);
                    });
            return application.CreateClient();
        }

        private static IConfigurationRoot InitializeConfiguration()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Test";
            DotEnv.Load();
            //Console.WriteLine(environment);
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            //configurationBuilder.AddJsonFile($"appsettings.json");
            configurationBuilder.AddJsonFile($"appsettings.{environment}.json");
            configurationBuilder.AddEnvironmentVariables();
            var config = configurationBuilder.Build();
            return config;
        }

        public virtual void Dispose()
        {
            _dbContext.ToDoItems.RemoveRange(_dbContext.ToDoItems);
            _dbContext.SaveChanges();
            _httpClient.Dispose();
            _dbContext.Dispose();
        }
    }
}