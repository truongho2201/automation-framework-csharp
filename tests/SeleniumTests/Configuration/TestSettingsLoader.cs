using Microsoft.Extensions.Configuration;

namespace SeleniumTests.Configuration;

public static class TestSettingsLoader
{
    public static TestSettings Load(string? settingsPath = null)
    {
        settingsPath ??= Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        var fullPath = Path.GetFullPath(settingsPath);

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.GetDirectoryName(fullPath)!)
            .AddJsonFile(Path.GetFileName(fullPath), optional: false, reloadOnChange: false)
            .AddEnvironmentVariables(prefix: "SELENIUM_")
            .Build();

        var settings = configuration.GetRequiredSection("TestSettings").Get<TestSettings>()
            ?? throw new InvalidOperationException("The TestSettings section is missing.");

        settings = settings with
        {
            Browser = TestContext.Parameters.Get("Browser", settings.Browser),
            TargetUrl = TestContext.Parameters.Get("TargetUrl", settings.TargetUrl),
        };

        settings.Validate();
        return settings;
    }
}
