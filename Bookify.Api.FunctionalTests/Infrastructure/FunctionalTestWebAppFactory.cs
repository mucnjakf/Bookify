using System.Net.Http.Json;
using Bookify.Api.FunctionalTests.Users;
using Bookify.Application.Abstractions.Data;
using Bookify.Infrastructure.Authentication;
using Bookify.Infrastructure.Dapper;
using Bookify.Infrastructure.EfCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Testcontainers.Keycloak;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;

namespace Bookify.Api.FunctionalTests.Infrastructure;

public sealed class FunctionalTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder("postgres:17.2")
        .WithDatabase("bookify")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    private readonly RedisContainer _redisContainer = new RedisBuilder("redis:latest").Build();

    private readonly KeycloakContainer _keycloakContainer = new KeycloakBuilder("quay.io/keycloak/keycloak:latest")
        .WithResourceMapping(
            new FileInfo(".files/bookify-realm-export.json"),
            new FileInfo("/opt/keycloak/data/import/realm.json"))
        .WithCommand("--import-realm")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<ApplicationDbContext>>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options
                    .UseNpgsql(_postgreSqlContainer.GetConnectionString())
                    .UseSnakeCaseNamingConvention());

            services.RemoveAll<ISqlConnectionFactory>();

            services.AddSingleton<ISqlConnectionFactory>(_ =>
                new SqlConnectionFactory(_postgreSqlContainer.GetConnectionString()));

            services.Configure<RedisCacheOptions>(options =>
                options.Configuration = _redisContainer.GetConnectionString());

            string? keycloakAddress = _keycloakContainer.GetBaseAddress();

            services.Configure<KeycloakOptions>(options =>
            {
                options.AdminUrl = $"{keycloakAddress}admin/realms/bookify/";
                options.TokenUrl = $"{keycloakAddress}realms/bookify/protocol/openid-connect/token";
            });

            services.Configure<AuthenticationOptions>(options =>
            {
                options.Issuer = $"{keycloakAddress}realms/bookify";
                options.MetadataUrl = $"{keycloakAddress}realms/bookify/.well-known/openid-configuration";
            });
        });
    }

    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();
        await _redisContainer.StartAsync();
        await _keycloakContainer.StartAsync();

        await InitializeTestUserAsync();
    }

    public new async Task DisposeAsync()
    {
        await _postgreSqlContainer.StopAsync();
        await _redisContainer.StopAsync();
        await _keycloakContainer.StopAsync();
    }

    private async Task InitializeTestUserAsync()
    {
        HttpClient httpClient = CreateClient();

        await httpClient.PostAsJsonAsync("api/auth/register", UserData.RegisterTestUserRequest);
    }
}