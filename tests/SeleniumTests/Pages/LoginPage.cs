using OpenQA.Selenium;

namespace SeleniumTests.Pages;

public sealed class LoginPage(IWebDriver driver, TimeSpan explicitWait)
    : BasePage(driver, explicitWait)
{
    private static readonly By Username = By.Id("userName");
    private static readonly By Password = By.Id("password");
    private static readonly By LoginButton = By.Id("login");
    private static readonly By LoggedInUsername = By.Id("userName-value");

    public void Open(string baseUrl) => NavigateTo(new Uri(new Uri(baseUrl), "login").ToString());

    public void Login(string username, string password)
    {
        WaitUntilVisible(Username).SendKeys(username);
        WaitUntilVisible(Password).SendKeys(password);
        WaitUntilVisible(LoginButton).Click();
        WaitUntilVisible(LoggedInUsername);
    }
}
