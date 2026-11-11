using OpenQA.Selenium;

namespace SeleniumTests.Pages;

public sealed class BookDetailsPage(IWebDriver driver, TimeSpan explicitWait)
    : BasePage(driver, explicitWait)
{
    private static readonly By BookTitleLocator = By.XPath(
        "(//label[normalize-space()='Title :' or normalize-space()='Title:']/parent::div/following-sibling::div[1]//label)[1]");
    private static readonly By AddToCollectionButton = By.XPath("//button[normalize-space()='Add To Your Collection']");

    public string BookTitle => WaitUntilVisible(BookTitleLocator).Text;

    public string AddToCollection()
    {
        var addButton = WaitUntilVisible(AddToCollectionButton);
        ((IJavaScriptExecutor)Driver).ExecuteScript(
            "arguments[0].scrollIntoView({block: 'center', inline: 'nearest'});",
            addButton);

        try
        {
            addButton.Click();
        }
        catch (ElementClickInterceptedException)
        {
            // DemoQA has a fixed footer which can overlap the button at the bottom of the viewport.
            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", addButton);
        }

        var alert = Wait.Until(currentDriver =>
        {
            try
            {
                return currentDriver.SwitchTo().Alert();
            }
            catch (NoAlertPresentException)
            {
                return null;
            }
        });

        var message = alert!.Text;
        alert.Accept();
        return message ?? string.Empty;
    }
}
