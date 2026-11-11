using SeleniumTests.Configuration;

namespace SeleniumTests.Tests;

[TestFixture]
public sealed class ConfigurationTests
{
    [Test]
    public void ValidateWithSupportedValuesDoesNotThrow()
    {
        var settings = new TestSettings();

        Assert.That((Action)settings.Validate, Throws.Nothing);
    }

    [Test]
    public void ValidateWithUnsupportedBrowserThrowsHelpfulError()
    {
        var settings = new TestSettings { Browser = "Firefox" };

        Assert.That(
            (Action)settings.Validate,
            Throws.InvalidOperationException.With.Message.Contains("Chrome or Edge"));
    }

    [Test]
    public void LoadReadsJsonSettings()
    {
        var settings = TestSettingsLoader.Load(
            Path.Combine(TestContext.CurrentContext.TestDirectory, "appsettings.json"));

        Assert.Multiple((Action)(() =>
        {
            Assert.That(settings.Browser, Is.EqualTo("Chrome"));
            Assert.That(settings.TargetUrl, Is.EqualTo("https://demoqa.com/"));
            Assert.That(settings.Username, Is.EqualTo("hodactruong"));
            Assert.That(settings.Headless, Is.False);
        }));
    }
}
