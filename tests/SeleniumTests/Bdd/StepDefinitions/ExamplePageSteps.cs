using NUnit.Framework;
using Reqnroll;
using SeleniumTests.Bdd;

namespace SeleniumTests.Bdd.StepDefinitions;

[Binding]
public sealed class ExamplePageSteps
{
    private readonly ScenarioState _state;

    public ExamplePageSteps(ScenarioState state)
    {
        _state = state;
    }

    [Given("the browser is started")]
    public void GivenTheBrowserIsStarted()
    {
        Assert.That(_state.Driver, Is.Not.Null);
    }

    [When("I navigate to the configured target URL")]
    public void WhenINavigateToTheConfiguredTargetUrl()
    {
        Assert.That(_state.Settings, Is.Not.Null);
        Assert.That(_state.Page, Is.Not.Null);

        _state.Page!.NavigateTo(_state.Settings!.TargetUrl);
    }

    [Then("the page title should not be empty")]
    public void ThenThePageTitleShouldNotBeEmpty()
    {
        Assert.That(_state.Page, Is.Not.Null);
        Assert.That(_state.Page!.Title, Is.Not.Empty);
    }

    [Then("the page body should not be empty")]
    public void ThenThePageBodyShouldNotBeEmpty()
    {
        Assert.That(_state.Page, Is.Not.Null);
        Assert.That(_state.Page!.BodyText, Is.Not.Empty);
    }
}
