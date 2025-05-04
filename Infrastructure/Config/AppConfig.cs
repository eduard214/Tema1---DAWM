using Infrastructure.Config.Models;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Config;

public class AppConfig
{
    public static bool ConsoleLogQueries = true;
    public static ConnectionStringSettings? ConnectionStrings { get; set; }

    public static void Init(IConfiguration configuration)
    {
        Configure(configuration);
    }

    private static void Configure(IConfiguration configuration)
    {
        ConnectionStrings = configuration.GetSection("ConnectionStrings").Get<ConnectionStringSettings>();
    }
}