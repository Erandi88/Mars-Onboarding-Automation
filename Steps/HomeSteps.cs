using NUnit.Framework;
using qa_dotnet_cucumber.Pages;
using Reqnroll;

namespace qa_dotnet_cucumber.Steps
{
    [Binding]
    public class HomeSteps
    {
        private readonly HomePage _homePage;
        private readonly NavigationHelper _navigationHelper;

        public HomeSteps(HomePage homePage, NavigationHelper navigationHelper)
        {
            _homePage = homePage;
            _navigationHelper = navigationHelper;
        }

        [Given("I navigate to the Mars home page")]
        public void GivenINavigateToTheMarsHomePage()
        {
            _navigationHelper.NavigateTo("/Home");
        }

        [Then("I should see the Mars home page heading")]
        public void ThenIShouldSeeTheMarsHomePageHeading()
        {
            Assert.That(_homePage.IsHomePageLoaded(), Is.True, "Mars home page should be loaded.");
        }
    }
}