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

        public LanguageSteps(LoginPage loginPage, LanguagePage languagePage, NavigationHelper navigationHelper, TestDataContext testDataContext)
        {
            _loginPage = loginPage;
            _languagePage = languagePage;
            _navigationHelper = navigationHelper;
            _testDataContext = testDataContext;
        }

        
        #region 2. Common and Reusable Steps

        [Given("I am logged in to Mars")]
        public void GivenIAmLoggedInToMars()
        {
            _navigationHelper.NavigateTo("/Home");

            LoadCredentialsFromSettings();

            _loginPage.Login(_email, _password);
        }

        /*
         * This precondition can be reused by Edit, Delete,
         * Duplicate and other scenarios.
         */
        [Given(@"the language ""(.*)"" with level ""(.*)"" exists")]
        public void GivenTheLanguageWithLevelExists(string language, string level)
        {
            _languagePage.DeleteLanguageIfExists(language);

            _languagePage.AddLanguage(language, level);

            _testDataContext.CreatedLanguages.Add(language);

            Assert.That(
                _languagePage.IsLanguageAndLevelDisplayed(language, level),
                Is.True,
                $"The language '{language}' with level '{level}' " +
                "should exist before the scenario action."
            );
        }

        /*
         * This assertion can be reused after Add and Edit actions.
         */
        [Then(@"the language ""(.*)"" should be displayed with level ""(.*)""")]
        public void ThenTheLanguageShouldBeDisplayedWithLevel(string language,string level)
        {
            Assert.That(
                _languagePage.IsLanguageAndLevelDisplayed(language, level),
                Is.True,
                $"The language '{language}' should be displayed " +
                $"with level '{level}'."
            );
        }

        #endregion


        #region 3. Add Language - Positive Scenario

        [When(@"I add the language ""(.*)"" with level ""(.*)""")]
        public void WhenIAddTheLanguageWithLevel(
            string language,
            string level)
        {
            // Cleanup before the action.
            _languagePage.DeleteLanguageIfExists(language);

            // Confirm that the test starts with the correct state.
            Assert.That(
                _languagePage.IsLanguageDisplayed(language),
                Is.False,
                $"The language '{language}' should not exist " +
                "before the test starts."
            );

            // Perform the scenario action.
            _languagePage.AddLanguage(language, level);

            // Store the created record for after-scenario cleanup.
            _testDataContext.CreatedLanguages.Add(language);
        }

        #endregion


        #region 4. Edit Language Scenario

        [When(
            @"I edit the language ""(.*)"" to ""(.*)"" with level ""(.*)""")]
        public void WhenIEditTheLanguage(
            string currentLanguage,
            string newLanguage,
            string newLevel)
        {
            // Prevent the updated value from already existing.
            _languagePage.DeleteLanguageIfExists(newLanguage);

            _languagePage.EditLanguage(
                currentLanguage,
                newLanguage,
                newLevel
            );

            // Update the cleanup information.
            _testDataContext.CreatedLanguages.Remove(currentLanguage);
            _testDataContext.CreatedLanguages.Add(newLanguage);
        }

        #endregion


        #region 5. Delete Language Scenario

        [When(@"I delete the language ""(.*)""")]
        public void WhenIDeleteTheLanguage(string language)
        {
            _languagePage.DeleteLanguage(language);
        }

        [Then(
            @"the language ""(.*)"" should be removed from the language list")]
        public void ThenTheLanguageShouldBeRemovedFromTheLanguageList(
            string language)
        {
            Assert.That(
                _languagePage.IsLanguageRemoved(language),
                Is.True,
                $"The language '{language}' should be removed " +
                "from the language list."
            );

            // The record has already been deleted.
            // Therefore, cleanup does not need to delete it again.
            _testDataContext.CreatedLanguages.Remove(language);
        }

        #endregion


        #region 6. Duplicate Language Validation

        /*
         * Used when testing the same language and level again.
         */
        [When(
            @"I try to add the language ""(.*)"" with level ""(.*)"" again")]
        public void WhenITryToAddTheLanguageAgain(
            string language,
            string level)
        {
            _languagePage.AddLanguage(language, level);
        }

        [Then(@"only one ""(.*)"" with level ""(.*)"" should exist")]
        public void ThenOnlyOneLanguageWithLevelShouldExist(
            string language,
            string level)
        {
            int actualRecordCount =
                _languagePage.GetLanguageRecordCount(language, level);

            Assert.That(
                actualRecordCount,
                Is.EqualTo(1),
                $"Only one '{language}' record with level " +
                $"'{level}' should exist."
            );
        }

        /*
         * Used when the application should prevent multiple
         * records with the same language.
         */
        [Then(@"only one record for the language ""(.*)"" should exist")]
        public void ThenOnlyOneRecordForTheLanguageShouldExist(
            string language)
        {
            int actualRecordCount =
                _languagePage.GetLanguageRecordCount(language);

            Assert.That(
                actualRecordCount,
                Is.EqualTo(1),
                $"Only one record for the language " +
                $"'{language}' should exist."
            );
        }

        [Then("the duplicate language message should be displayed")]
        public void ThenTheDuplicateLanguageMessageShouldBeDisplayed()
        {
            Assert.That(
                _languagePage.IsDuplicateLanguageMessageDisplayed(),
                Is.True,
                "The duplicate language validation message " +
                "should be displayed."
            );
        }

        [Then("the duplicated data message should be displayed")]
        public void ThenTheDuplicatedDataMessageShouldBeDisplayed()
        {
            Assert.That(
                _languagePage.IsDuplicatedDataMessageDisplayed(),
                Is.True,
                "The duplicated data validation message " +
                "should be displayed."
            );
        }

        #endregion


        #region 7. Required-Field Validation

        /*
         * Empty Language field test.
         */
        [When(
            @"I try to add a language with an empty language field and level ""(.*)""")]
        public void WhenITryToAddALanguageWithAnEmptyLanguageField(
            string level)
        {
            RememberCurrentLanguageRowCount();

            _languagePage.AddLanguage(string.Empty, level);
        }

        [Then("the language validation message should be displayed")]
        public void ThenTheLanguageValidationMessageShouldBeDisplayed()
        {
            Assert.That(
                _languagePage.IsLanguageValidationMessageDisplayed(),
                Is.True,
                "The validation message should be displayed " +
                "for an empty language field."
            );
        }

        /*
         * Empty Level field test.
         */
        [When(@"I try to add the language ""(.*)"" with an empty level")]
        public void WhenITryToAddALanguageWithAnEmptyLevel(
            string language)
        {
            RememberCurrentLanguageRowCount();

            _languagePage.AddLanguageWithoutLevel(language);
        }

        /*
         * Shared by the empty Language and empty Level tests.
         */
        [Then("no language record should be created")]
        public void ThenNoLanguageRecordShouldBeCreated()
        {
            int currentRowCount =
                _languagePage.GetLanguageRowCount();

            Assert.That(
                currentRowCount,
                Is.EqualTo(
                    _testDataContext.LanguageRowCountBeforeAction
                ),
                "The number of language records should remain " +
                "unchanged after an invalid submission."
            );
        }

        #endregion

        /*
            Update a language to an existing langauage
         */

        [When(@"I update the language ""(.*)"" to ""(.*)"" with level ""(.*)""")]
        public void WhenIUpdateTheLanguageToExistingLanguage(string languageToUpdate,string existingLanguage, string level)
        {
            _languagePage.EditLanguage(
                languageToUpdate,
                existingLanguage,
                level
            );
        }

        [Then("the language already added message should be displayed")]
        public void ThenTheLanguageAlreadyAddedMessageShouldBeDisplayed()
        {
            Assert.That(
                _languagePage.IsLanguageAlreadyAddedMessageDisplayed(),
                Is.True,
                "The language already added validation message should be displayed."
            );
        }

        [When("I cancel the language edit")]
        public void WhenICancelTheLanguageEdit()
        {
            _languagePage.ClickCancelButton();
        }


        #region 8. Destructive Testing - Very Large Input

        [When(
            @"I add a language containing ""(.*)"" characters with level ""(.*)""")]
        public void WhenIAddALanguageContainingCharacters(
            int characterCount,
            string level)
        {
            string longLanguage = new string('A', characterCount);

            _languagePage.DeleteLanguageIfExists(longLanguage);

            _languagePage.AddLanguage(longLanguage, level);

            _testDataContext.CreatedLanguages.Add(longLanguage);
            _testDataContext.CurrentLanguage = longLanguage;
        }

        [Then(
            @"the very large language should be displayed with level ""(.*)""")]
        public void ThenTheVeryLargeLanguageShouldBeDisplayedWithLevel(
            string level)
        {
            string currentLanguage =
                _testDataContext.CurrentLanguage;

            Assert.That(
                _languagePage.IsLanguageAndLevelDisplayed(
                    currentLanguage,
                    level
                ),
                Is.True,
                $"The language containing " +
                $"{currentLanguage.Length} characters should be " +
                $"displayed with level '{level}'."
            );
        }

        [Then("the application should remain responsive")]
        public void ThenTheApplicationShouldRemainResponsive()
        {
            Assert.That(
                _languagePage.IsAddNewButtonAvailable(),
                Is.True,
                "The Languages section should remain responsive " +
                "after adding a very large language."
            );
        }

        #endregion


        #region 9. Private Helper Methods

        private void RememberCurrentLanguageRowCount()
        {
            _testDataContext.LanguageRowCountBeforeAction =
                _languagePage.GetLanguageRowCount();
        }

        private void LoadCredentialsFromSettings()
        {
            string json = File.ReadAllText("settings.json");

            using JsonDocument document =
                JsonDocument.Parse(json);

            JsonElement credentials =
                document.RootElement.GetProperty("Credentials");

            _email =
                credentials.GetProperty("Email").GetString()
                ?? string.Empty;

            _password =
                credentials.GetProperty("Password").GetString()
                ?? string.Empty;
        }

        #endregion
    }
}