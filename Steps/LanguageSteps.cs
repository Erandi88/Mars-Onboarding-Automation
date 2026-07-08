using NUnit.Framework;
using qa_dotnet_cucumber.Pages;
using Reqnroll;
using System.Text.Json;

namespace qa_dotnet_cucumber.Steps
{
    [Binding]
    public class LanguageSteps
    {
        private readonly LoginPage _loginPage;
        private readonly LanguagePage _languagePage;
        private readonly NavigationHelper _navigationHelper;

        private string _email = string.Empty;
        private string _password = string.Empty;

        private readonly string _language = "AutomationEnglish";
        private readonly string _level = "Fluent";

        private readonly string _updatedLanguage = "AutomationFrench";
        private readonly string _updatedLevel = "Native/Bilingual";

        public LanguageSteps(LoginPage loginPage, LanguagePage languagePage, NavigationHelper navigationHelper)
        {
            _loginPage = loginPage;
            _languagePage = languagePage;
            _navigationHelper = navigationHelper;
        }

        [Given("I am logged in to Mars")]
        public void GivenIAmLoggedInToMars()
        {
            _navigationHelper.NavigateTo("/Home");

            LoadCredentialsFromSettings();

            _loginPage.Login(_email, _password);
        }

        [When("I add a new language with valid details")]
        public void WhenIAddANewLanguageWithValidDetails()
        {
            DeleteLanguageIfExists();

            _languagePage.AddLanguage(_language, _level);
        }

        [Then("the language should be displayed in the language list")]
        public void ThenTheLanguageShouldBeDisplayedInTheLanguageList()
        {
            Assert.That(
                _languagePage.IsLanguageDisplayed(_language),
                Is.True,
                "The added language should be displayed in the language list."
            );
        }


        [Given("a language exists in the language list")]
        public void GivenALanguageExistsInTheLanguageList()
        {
            if (!_languagePage.IsLanguageDisplayed(_language))
            {
                _languagePage.AddLanguage(_language, _level);
            }
        }

        [When("I delete the language")]
        public void WhenIDeleteTheLanguage()
        {
            _languagePage.DeleteLanguage(_language);
        }

        [Then("the language should be removed from the language list")]
        public void ThenTheLanguageShouldBeRemovedFromTheLanguageList()
        {
            Assert.That(
                _languagePage.IsLanguageRemoved(_language),
                Is.True,
                "The language should be removed from the language list."
            );
        }

        [When("I edit the language with new valid details")]
        public void WhenIEditTheLanguageWithNewValidDetails()
        {
            _languagePage.EditLanguage(_language, _updatedLanguage, _updatedLevel);
        }

        [Then("the updated language should be displayed in the language list")]
        public void ThenTheUpdatedLanguageShouldBeDisplayedInTheLanguageList()
        {
            Assert.That(
                _languagePage.IsLanguageDisplayed(_updatedLanguage),
                Is.True,
                "The updated language should be displayed in the language list."
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

        private void DeleteLanguageIfExists()
        {
            _languagePage.DeleteLanguageIfExists(_language);
            _languagePage.DeleteLanguageIfExists(_updatedLanguage);
        }
    }
}