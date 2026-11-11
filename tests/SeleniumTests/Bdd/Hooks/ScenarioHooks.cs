using NUnit.Framework;
using Allure.Net.Commons;
using Reqnroll;
using SeleniumTests.Configuration;
using SeleniumTests.Drivers;
using SeleniumTests.Infrastructure;
using SeleniumTests.Pages;

namespace SeleniumTests.Bdd.Hooks;

[Binding]
public sealed class ScenarioHooks
{
    private readonly ScenarioContext _scenarioContext;
    private readonly ScenarioState _state;

    public ScenarioHooks(ScenarioContext scenarioContext, ScenarioState state)
    {
        _scenarioContext = scenarioContext;
        _state = state;
    }

    [BeforeScenario]
    public void StartBrowser()
    {
        _state.Settings = TestSettingsLoader.Load();
        _state.Logger = new TestLogger();

        var factory = new WebDriverFactory(_state.Logger.Create<WebDriverFactory>());
        _state.Driver = factory.Create(_state.Settings);
        _state.Page = new ExamplePage(
            _state.Driver,
            TimeSpan.FromSeconds(_state.Settings.ExplicitWaitSeconds));
    }

    [AfterScenario]
    public void StopBrowser()
    {
        try
        {
            if (_scenarioContext.TestError is not null && _state.Driver is not null && _state.Settings is not null)
            {
                var path = ScreenshotCapture.Capture(
                    _state.Driver,
                    _state.Settings.ScreenshotDirectory,
                    _scenarioContext.ScenarioInfo.Title);

                TestContext.AddTestAttachment(path, "Screenshot captured after scenario failure");
                AllureApi.AddAttachment("Failure screenshot", "image/png", path);
            }
        }
        finally
        {
            _state.Driver?.Quit();
            _state.Driver?.Dispose();
            _state.Logger?.Dispose();
        }
    }
}
