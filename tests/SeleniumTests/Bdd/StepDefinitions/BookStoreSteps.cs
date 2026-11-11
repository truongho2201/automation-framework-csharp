using NUnit.Framework;
using Reqnroll;
using SeleniumTests.Bdd;
using SeleniumTests.Infrastructure;
using SeleniumTests.Pages;

namespace SeleniumTests.Bdd.StepDefinitions;

[Binding]
public sealed class BookStoreSteps
{
    private readonly ScenarioState _state;

    public BookStoreSteps(ScenarioState state)
    {
        _state = state;
    }

    [Given("the user's book collection is empty")]
    public async Task GivenTheUsersBookCollectionIsEmpty()
    {
        Assert.That(_state.Settings, Is.Not.Null);

        using var apiClient = new DemoQaApiClient();
        await apiClient.ClearBookCollectionAsync(
            _state.Settings!.TargetUrl,
            _state.Settings.Username,
            _state.Settings.Password);
    }

    [Given("the user is logged in")]
    public void GivenTheUserIsLoggedIn()
    {
        Assert.Multiple((Action)(() =>
        {
            Assert.That(_state.Driver, Is.Not.Null);
            Assert.That(_state.Settings, Is.Not.Null);
        }));

        _state.LoginPage = new LoginPage(
            _state.Driver!,
            TimeSpan.FromSeconds(_state.Settings!.ExplicitWaitSeconds));
        _state.LoginPage.Open(_state.Settings.TargetUrl);
        _state.LoginPage.Login(_state.Settings.Username, _state.Settings.Password);
    }

    [Given("the user opens the Book Store")]
    public void GivenTheUserOpensTheBookStore()
    {
        Assert.That(_state.Settings, Is.Not.Null);
        _state.BookStorePage = new BookStorePage(
            _state.Driver!,
            TimeSpan.FromSeconds(_state.Settings!.ExplicitWaitSeconds));
        _state.BookStorePage.Open(_state.Settings.TargetUrl);
    }

    [When("the user searches for {string}")]
    public void WhenTheUserSearchesFor(string title)
    {
        Assert.That(_state.BookStorePage, Is.Not.Null);
        _state.BookStorePage!.Search(title);
    }

    [When("the user opens the {string} book details")]
    public void WhenTheUserOpensTheBookDetails(string title)
    {
        Assert.That(_state.BookStorePage, Is.Not.Null);
        _state.BookDetailsPage = _state.BookStorePage!.OpenBookDetails(title);
    }

    [Then("the book details title should be {string}")]
    public void ThenTheBookDetailsTitleShouldBe(string expectedTitle)
    {
        Assert.That(_state.BookDetailsPage, Is.Not.Null);
        Assert.That(_state.BookDetailsPage!.BookTitle, Is.EqualTo(expectedTitle));
    }

    [When("the user adds the book to the collection")]
    public void WhenTheUserAddsTheBookToTheCollection()
    {
        Assert.That(_state.BookDetailsPage, Is.Not.Null);
        _state.LastAlertMessage = _state.BookDetailsPage!.AddToCollection();
    }

    [Then("the add-to-collection confirmation should be displayed")]
    public void ThenTheAddToCollectionConfirmationShouldBeDisplayed()
    {
        Assert.That(_state.LastAlertMessage, Does.Contain("Book added to your collection"));
    }
}
