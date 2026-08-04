using NUnit.Framework;
using qa_dotnet_cucumber.Contexts;
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
        private readonly TestDataContext _testDataContext;

        private string _email = string.Empty;
        private string _password = string.Empty;

        //private readonly string _language = "AutomationEnglish";
        //private readonly string _level = "Fluent";

        //private readonly string _updatedLanguage = "AutomationFrench";
        //private readonly string _updatedLevel = "Native/Bilingual";

        public LanguageSteps(LoginPage loginPage, LanguagePage languagePage, NavigationHelper navigationHelper, TestDataContext testDataContext)
        {
            _loginPage = loginPage;
            _languagePage = languagePage;
            _navigationHelper = navigationHelper;
            _testDataContext = testDataContext;
        }

        [Given("I am logged in to Mars")]
        public void GivenIAmLoggedInToMars()
        {
            _navigationHelper.NavigateTo("/Home");

            LoadCredentialsFromSettings();

            _loginPage.Login(_email, _password);
        }

        [When(@"I add the language ""(.*)"" with level ""(.*)""")]
        public void WhenIAddTheLanguageWithLevel(string language, string level)
        {
            _languagePage.DeleteLanguageIfExists(language);

            Assert.That(
                _languagePage.IsLanguageDisplayed(language),
                Is.False,
                $"The language '{language}' should not exist before the test starts."
            );

            _languagePage.AddLanguage(language, level);

            _testDataContext.CreatedLanguages.Add(language);
        }

        [Then(@"the language ""(.*)"" should be displayed with level ""(.*)""")]
        public void ThenTheLanguageWithLevelShouldBeDisplayed(string language, string level)
        {
            Assert.That(
                _languagePage.IsLanguageAndLevelDisplayed(language, level),
                Is.True,
                $"The language '{language}' should be displayed with level '{level}'."
            );
        }




        [Given(@"the language ""(.*)"" with level ""(.*)"" exists")]
        public void GivenTheLanguageWithLevelExists(string language, string level)
        {
            _languagePage.DeleteLanguageIfExists(language);

            _languagePage.AddLanguage(language, level);

            _testDataContext.CreatedLanguages.Add(language);

            Assert.That(
                _languagePage.IsLanguageAndLevelDisplayed(language, level),
                Is.True,
                $"The language '{language}' with level '{level}' should exist before the scenario action."
            );
        }

        [When(@"I delete the language ""(.*)""")]
        public void WhenIDeleteTheLanguage(string language)
        {
            _languagePage.DeleteLanguage(language);
        }

        [Then(@"the language ""(.*)"" should be removed from the language list")]
        public void ThenTheLanguageShouldBeRemovedFromTheLanguageList(string language)
        {
            Assert.That(
                _languagePage.IsLanguageRemoved(language),
                Is.True,
                $"The language '{language}' should be removed from the language list."
            );

            _testDataContext.CreatedLanguages.Remove(language);
        }

        [When(@"I edit the language ""(.*)"" to ""(.*)"" with level ""(.*)""")]
        public void WhenIEditTheLanguage( string currentLanguage, string newLanguage, string newLevel)
        {
            _languagePage.DeleteLanguageIfExists(newLanguage);

            _languagePage.EditLanguage(
                currentLanguage,
                newLanguage,
                newLevel
            );

            _testDataContext.CreatedLanguages.Remove(currentLanguage);
            _testDataContext.CreatedLanguages.Add(newLanguage);
        }


        [When(@"I try to add the language ""(.*)"" with level ""(.*)"" again")]
        public void WhenITryToAddTheLanguageAgain(string language, string level)
        {
            _languagePage.AddLanguage(language, level);
        }

        [Then(@"only one ""(.*)"" with level ""(.*)"" should exist")]
        public void ThenOnlyOneLanguageWithLevelShouldExist(string language,string level)
        {
            Assert.That(
                _languagePage.GetLanguageRecordCount(language, level),
                Is.EqualTo(1),
                $"Only one '{language}' record with level '{level}' should exist."
            );
        }

        [Then("the duplicate language message should be displayed")]
        public void ThenTheDuplicateLanguageMessageShouldBeDisplayed()
        {
            Assert.That(
                _languagePage.IsDuplicateLanguageMessageDisplayed(),
                Is.True,
                "The duplicate language validation message should be displayed."
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