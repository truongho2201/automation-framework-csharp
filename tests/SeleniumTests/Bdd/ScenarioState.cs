using OpenQA.Selenium;
using SeleniumTests.Configuration;
using SeleniumTests.Infrastructure;
using SeleniumTests.Pages;

namespace SeleniumTests.Bdd;

public sealed class ScenarioState
{
    public TestSettings? Settings { get; set; }

    public TestLogger? Logger { get; set; }

    public IWebDriver? Driver { get; set; }

    public ExamplePage? Page { get; set; }

    public LoginPage? LoginPage { get; set; }

    public BookStorePage? BookStorePage { get; set; }

    public BookDetailsPage? BookDetailsPage { get; set; }

    public string? LastAlertMessage { get; set; }
}
