using OpenQA.Selenium;

namespace SeleniumTests.Pages;

public sealed class BookStorePage(IWebDriver driver, TimeSpan explicitWait)
    : BasePage(driver, explicitWait)
{
    private static readonly By SearchBox = By.Id("searchBox");
    private static readonly By SearchButton = By.XPath("//input[@id='searchBox']/following-sibling::button");

    public void Open(string baseUrl)
    {
        NavigateTo(new Uri(new Uri(baseUrl), "books").ToString());
        WaitUntilVisible(SearchBox);
    }

    public void Search(string title)
    {
        var search = WaitUntilVisible(SearchBox);
        search.Clear();
        search.SendKeys(title);
        WaitUntilVisible(SearchButton).Click();

        WaitForBookLink(title);
    }

    public BookDetailsPage OpenBookDetails(string title)
    {
        var bookLink = WaitForBookLink(title);
        bookLink!.Click();

        return new BookDetailsPage(Driver, Wait.Timeout);
    }

    private IWebElement WaitForBookLink(string title) => Wait.Until(currentDriver => currentDriver.FindElements(
            By.XPath($"//a[normalize-space()={ToXPathLiteral(title)}]"))
        .FirstOrDefault(element => element.Displayed))!;

    private static string ToXPathLiteral(string value) =>
        value.Contains('\'')
            ? $"concat('{value.Replace("'", "',\"'\",'")}')"
            : $"'{value}'";
}
