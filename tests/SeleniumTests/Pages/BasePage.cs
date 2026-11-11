using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTests.Pages;

public abstract class BasePage(IWebDriver driver, TimeSpan explicitWait)
{
    protected IWebDriver Driver { get; } = driver;

    protected WebDriverWait Wait { get; } = new(driver, explicitWait);

    public string Title => Driver.Title;

    public void NavigateTo(string url) => Driver.Navigate().GoToUrl(url);

    protected IWebElement WaitUntilVisible(By locator) => Wait.Until(currentDriver =>
    {
        var element = currentDriver.FindElement(locator);
        return element.Displayed ? element : null;
    })!;
}
