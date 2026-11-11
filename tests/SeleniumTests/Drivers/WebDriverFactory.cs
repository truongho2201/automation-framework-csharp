using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Remote;
using SeleniumTests.Configuration;

namespace SeleniumTests.Drivers;

public interface IWebDriverFactory
{
    IWebDriver Create(TestSettings settings);
}

public sealed class WebDriverFactory(ILogger<WebDriverFactory> logger) : IWebDriverFactory
{
    private static readonly Action<ILogger, string, bool, bool, Exception?> LogCreatingDriver =
        LoggerMessage.Define<string, bool, bool>(
            Microsoft.Extensions.Logging.LogLevel.Information,
            new EventId(1001, nameof(Create)),
            "Creating {Browser} WebDriver. Headless={Headless} Remote={Remote}");

    public IWebDriver Create(TestSettings settings)
    {
        settings.Validate();
        var options = CreateOptions(settings);

        LogCreatingDriver(
            logger,
            settings.Browser,
            settings.Headless,
            settings.RemoteWebDriverUrl is not null,
            null);

        IWebDriver driver = settings.RemoteWebDriverUrl is not null
            ? new RemoteWebDriver(new Uri(settings.RemoteWebDriverUrl), options)
            : CreateLocalDriver(settings.Browser, options);

        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(settings.ImplicitWaitSeconds);
        return driver;
    }

    public static DriverOptions CreateOptions(TestSettings settings)
    {
        settings.Validate();
        return settings.Browser switch
        {
            "Chrome" => CreateChromeOptions(settings.Headless),
            "Edge" => CreateEdgeOptions(settings.Headless),
            _ => throw new InvalidOperationException($"Unsupported browser '{settings.Browser}'."),
        };
    }

    private static ChromeOptions CreateChromeOptions(bool headless)
    {
        var options = new ChromeOptions();
        var profileDirectory = Path.Combine(Path.GetTempPath(), "SeleniumTests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(profileDirectory);

        options.AddArgument("--window-size=1440,1000");
        options.AddArgument($"--user-data-dir={profileDirectory}");
        options.AddArgument("--remote-debugging-pipe");
        options.AddArgument("--disable-dev-shm-usage");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-gpu");
        if (headless)
        {
            options.AddArgument("--headless=new");
        }

        return options;
    }

    private static EdgeOptions CreateEdgeOptions(bool headless)
    {
        var options = new EdgeOptions();
        var profileDirectory = Path.Combine(Path.GetTempPath(), "SeleniumTests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(profileDirectory);

        options.AddArgument("--window-size=1440,1000");
        options.AddArgument($"--user-data-dir={profileDirectory}");
        options.AddArgument("--remote-debugging-pipe");
        options.AddArgument("--disable-dev-shm-usage");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-gpu");
        if (headless)
        {
            options.AddArgument("--headless=new");
        }

        return options;
    }

    private static IWebDriver CreateLocalDriver(string browser, DriverOptions options) => browser switch
    {
        "Chrome" => new ChromeDriver((ChromeOptions)options),
        "Edge" => new EdgeDriver((EdgeOptions)options),
        _ => throw new InvalidOperationException($"Unsupported browser '{browser}'."),
    };
}
