using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using SeleniumTests.Configuration;
using SeleniumTests.Drivers;

namespace SeleniumTests.Tests;

[TestFixture]
public sealed class WebDriverFactoryTests
{
    [Test]
    public void CreateOptionsForHeadlessChromeReturnsConfiguredChromeOptions()
    {
        var options = WebDriverFactory.CreateOptions(new TestSettings
        {
            Browser = "Chrome",
            Headless = true,
        });

        Assert.That(options, Is.TypeOf<ChromeOptions>());
        Assert.That(((ChromeOptions)options).Arguments, Does.Contain("--headless=new"));
    }

    [Test]
    public void CreateOptionsForVisibleEdgeReturnsConfiguredEdgeOptions()
    {
        var options = WebDriverFactory.CreateOptions(new TestSettings
        {
            Browser = "Edge",
            Headless = false,
        });

        Assert.That(options, Is.TypeOf<EdgeOptions>());
        Assert.That(((EdgeOptions)options).Arguments, Does.Not.Contain("--headless=new"));
    }
}
