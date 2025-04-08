using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using WebApi;

namespace ProFix.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<WebApi.Program>

{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // TODO: Здесь можно заменить БД, настроить InMemory и т.д.
        });
    }
}
