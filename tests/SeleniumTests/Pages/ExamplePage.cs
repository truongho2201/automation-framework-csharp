using OpenQA.Selenium;

namespace SeleniumTests.Pages;

public sealed class ExamplePage(IWebDriver driver, TimeSpan explicitWait)
    : BasePage(driver, explicitWait)
{
    private static readonly By Body = By.TagName("body");

    public string BodyText => WaitUntilVisible(Body).Text;
}
