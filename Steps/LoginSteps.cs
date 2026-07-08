using NUnit.Framework;
using qa_dotnet_cucumber.Pages;
using Reqnroll;
using System.Text.Json;

namespace qa_dotnet_cucumber.Steps
{
    [Binding]
    public class LoginSteps
    {
        private readonly LoginPage _loginPage;
        private readonly NavigationHelper _navigationHelper;

        private string _email = string.Empty;
        private string _password = string.Empty;

        public LoginSteps(LoginPage loginPage, NavigationHelper navigationHelper)
        {
            _loginPage = loginPage;
            _navigationHelper = navigationHelper;
        }

        [Given("I am on the Mars home page")]
        public void GivenIAmOnTheMarsHomePage()
        {
            _navigationHelper.NavigateTo("/Home");
        }

        [When("I open the Sign In form")]
        public void WhenIOpenTheSignInForm()
        {
            _loginPage.OpenSignInForm();
        }

        [When("I enter valid Mars credentials")]
        public void WhenIEnterValidMarsCredentials()
        {
            LoadCredentialsFromSettings();

            _loginPage.EnterEmail(_email);
            _loginPage.EnterPassword(_password);
        }

        [When("I click the Login button")]
        public void WhenIClickTheLoginButton()
        {
            _loginPage.ClickLoginButton();
        }

        [Then("I should be logged in successfully")]
        public void ThenIShouldBeLoggedInSuccessfully()
        {
            Assert.That(
                _loginPage.IsLoggedInSuccessfully(),
                Is.True,
                "User should be redirected to the Profile page and Sign Out button should be visible."
            );
        }

        private void LoadCredentialsFromSettings()
        {
            string json = File.ReadAllText("settings.json");

            using JsonDocument document = JsonDocument.Parse(json);

            JsonElement credentials = document.RootElement.GetProperty("Credentials");

            _email = credentials.GetProperty("Email").GetString() ?? string.Empty;
            _password = credentials.GetProperty("Password").GetString() ?? string.Empty;
        }
    }
}