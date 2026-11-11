using Microsoft.Extensions.Logging;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using SeleniumTests.Configuration;
using SeleniumTests.Drivers;

namespace SeleniumTests.Infrastructure;

public abstract class SeleniumTestBase : IDisposable
{
    private static readonly Action<ILogger, string, Exception?> LogScreenshotSaved =
        LoggerMessage.Define<string>(
            Microsoft.Extensions.Logging.LogLevel.Error,
            new EventId(2001, "TestFailed"),
            "Test failed. Screenshot saved to {ScreenshotPath}");

    private static readonly Action<ILogger, Exception?> LogScreenshotFailure =
        LoggerMessage.Define(
            Microsoft.Extensions.Logging.LogLevel.Error,
            new EventId(2002, "ScreenshotFailure"),
            "Unable to capture the failure screenshot.");

    private TestLogger? _testLogger;
    private ILogger<SeleniumTestBase>? _logger;

    protected IWebDriver Driver { get; private set; } = null!;

    protected TestSettings Settings { get; private set; } = null!;

    [SetUp]
    public void SetUp()
    {
        Settings = TestSettingsLoader.Load();
        _testLogger = new TestLogger();
        _logger = _testLogger.Create<SeleniumTestBase>();
        Driver = new WebDriverFactory(_testLogger.Create<WebDriverFactory>()).Create(Settings);
    }

    [TearDown]
    public void TearDown()
    {
        try
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                var path = ScreenshotCapture.Capture(
                    Driver,
                    Settings.ScreenshotDirectory,
                    TestContext.CurrentContext.Test.Name);
                TestContext.AddTestAttachment(path, "Screenshot captured after test failure");
                if (_logger is not null)
                {
                    LogScreenshotSaved(_logger, path, null);
                }
            }
        }
        catch (Exception exception)
        {
            if (_logger is not null)
            {
                LogScreenshotFailure(_logger, exception);
            }
        }
        finally
        {
            Driver?.Quit();
            Driver?.Dispose();
            Dispose();
        }
    }

    public void Dispose()
    {
        _testLogger?.Dispose();
        _testLogger = null;
        GC.SuppressFinalize(this);
    }
}
