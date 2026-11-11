using Microsoft.Extensions.Logging;

namespace SeleniumTests.Infrastructure;

public sealed class TestLogger : IDisposable
{
    private readonly ILoggerFactory _factory = LoggerFactory.Create(builder =>
    {
        builder.SetMinimumLevel(LogLevel.Information);
        builder.AddJsonConsole(options =>
        {
            options.IncludeScopes = true;
            options.TimestampFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";
            options.UseUtcTimestamp = true;
        });
    });

    public ILogger<T> Create<T>() => _factory.CreateLogger<T>();

    public void Dispose() => _factory.Dispose();
}
