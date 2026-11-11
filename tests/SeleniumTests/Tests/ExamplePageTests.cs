using NSubstitute;
using OpenQA.Selenium;
using SeleniumTests.Infrastructure;
using SeleniumTests.Pages;

namespace SeleniumTests.Tests;

[TestFixture]
[Category("UI")]
public sealed class ExamplePageTests : SeleniumTestBase
{
    [Test]
    public void ConfiguredTargetDisplaysHtmlContent()
    {
        var page = new ExamplePage(Driver, TimeSpan.FromSeconds(Settings.ExplicitWaitSeconds));

        page.NavigateTo(Settings.TargetUrl);

        Assert.Multiple((Action)(() =>
        {
            Assert.That(page.Title, Is.Not.Empty);
            Assert.That(page.BodyText, Is.Not.Empty);
        }));
    }
}

[TestFixture]
public sealed class ExamplePageUnitTests
{
    [Test]
    public void BodyTextUsesTheVisibleBodyElement()
    {
        var driver = Substitute.For<IWebDriver>();
        var body = Substitute.For<IWebElement>();
        body.Displayed.Returns(true);
        body.Text.Returns("Rendered content");
        driver.FindElement(Arg.Any<By>()).Returns(body);

        var page = new ExamplePage(driver, TimeSpan.FromMilliseconds(100));

        Assert.That(page.BodyText, Is.EqualTo("Rendered content"));
    }
}
