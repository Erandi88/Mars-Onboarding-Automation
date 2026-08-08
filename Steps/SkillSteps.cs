using NUnit.Framework;
using qa_dotnet_cucumber.Contexts;
using qa_dotnet_cucumber.Pages;
using Reqnroll;

namespace qa_dotnet_cucumber.Steps
{
    [Binding]
    public class SkillSteps
    {
        private readonly SkillPage _skillPage;
        private readonly TestDataContext _testDataContext;

        private readonly string _skill = "AutomationSkill";
        private readonly string _level = "Beginner";

        private readonly string _updatedSkill = "AutomationCommunication";
        private readonly string _updatedLevel = "Intermediate";


        public SkillSteps(SkillPage skillPage, TestDataContext testDataContext)
        {
            _skillPage = skillPage;
            _testDataContext = testDataContext;
        }

        [Given("I am on the Profile page")]
        public void GivenIAmOnTheProfilePage()
        {
            Assert.That(
                _skillPage.IsProfilePageDisplayed(),
                Is.True,
                "User should be on the Profile page."
            );
        }

        [Given("I am on the Skills tab")]
        public void GivenIAmOnTheSkillsTab()
        {
            _skillPage.ClickSkillsTab();
        }

        /*add a new skill*/

        [When(@"I add the skill ""(.*)"" with level ""(.*)""")]
        public void WhenIAddTheSkillWithLevel(string skill,string level)
        {
            // Cleanup before the action.
            _skillPage.DeleteSkillIfExists(skill);

            // Confirm that the test starts with the correct state.
            Assert.That(
                _skillPage.IsSkillDisplayed(skill),Is.False,
                $"The skill '{skill}' should not exist " +
                "before the test starts."
            );

            // Perform the scenario action.
            _skillPage.AddSkill(skill, level);

            // Store the created record for after-scenario cleanup.
            _testDataContext.CreatedSkills.Add(skill);
        }

        //reuse for add, edit
        [Then(@"the skill ""(.*)"" should be displayed with level ""(.*)""")]
        public void ThenTheSkillShouldBeDisplayedWithLevel(string skill,string level)
        {
            Assert.That(
                _skillPage.IsSkillAndLevelDisplayed(skill, level),
                Is.True,
                $"The skill '{skill}' should be displayed " +
                $"with level '{level}'."
            );
        }


        /* Edit an existing skill with valid details, edid, delete , duplicate*/

        [Given(@"the skill ""(.*)"" with level ""(.*)"" exists")]
        public void GivenTheSkillWithLevelExists(string skill, string level)
        {
            _skillPage.DeleteSkillIfExists(skill);

            _skillPage.AddSkill(skill, level);

            _testDataContext.CreatedSkills.Add(skill);

            Assert.That(
                _skillPage.IsSkillAndLevelDisplayed(skill, level),
                Is.True,
                $"The skill '{skill}' with level '{level}' should exist before the scenario action."
            );
        }

        [When(@"I update the skill ""(.*)"" to ""(.*)"" with level ""(.*)""")]
        public void WhenIUpdateTheSkill(string currentSkill,string updatedSkill, string updatedLevel)
        {
            _skillPage.DeleteSkillIfExists(updatedSkill);

            _skillPage.EditSkill(currentSkill, updatedSkill,updatedLevel);

            _testDataContext.CreatedSkills.Add(updatedSkill);
        }

        /* delete*/

        [When(@"I delete the skill ""(.*)""")]
        public void WhenIDeleteTheSkill(string skill)
        {
            _skillPage.DeleteSkill(skill);
        }

        [Then(@"the skill ""(.*)"" should be removed from the skill list")]
        public void ThenTheSkillShouldBeRemovedFromTheSkillList(string skill)
        {
            Assert.That(
                _skillPage.IsSkillRemoved(skill),
                Is.True,
                $"The skill '{skill}' should be removed from the skill list."
            );

            _testDataContext.CreatedSkills.Remove(skill);
        }

        /* duplicate */
        [When(@"I try to add the skill ""(.*)"" with level ""(.*)"" again")]
        public void WhenITryToAddTheSkillAgain(string skill, string level)
        {
            _skillPage.AddSkill(skill, level);
        }

        [Then("the duplicate skill message should be displayed")]
        public void ThenTheDuplicateSkillMessageShouldBeDisplayed()
        {
            Assert.That(
                _skillPage.IsDuplicateSkillMessageDisplayed(),
                Is.True,
                "The duplicate skill validation message should be displayed."
            );
        }

        /*empty skill*/

        [When(@"I try to add a skill with an empty skill field and level ""(.*)""")]
        public void WhenITryToAddASkillWithAnEmptySkillFieldAndLevel(string level)
        {
            _testDataContext.SkillRowCountBeforeAction =
                _skillPage.GetSkillRowCount();

            _skillPage.AddSkill(string.Empty, level);
        }

        //reuse for empty skill, empty level
        [Then("the skill validation message should be displayed")]
        public void ThenTheSkillValidationMessageShouldBeDisplayed()
        {
            Assert.That(
                _skillPage.IsSkillValidationMessageDisplayed(),
                Is.True,
                "The skill validation message should be displayed."
            );
        }

        //reuse for empty skill, empty level
        [Then("no new skill record should be created")]
        public void ThenNoNewSkillRecordShouldBeCreated()
        {
            int skillRowCountAfterAction = _skillPage.GetSkillRowCount();

            Assert.That(
                skillRowCountAfterAction,
                Is.EqualTo(_testDataContext.SkillRowCountBeforeAction),
                "A new skill record should not be created when the skill field is empty."
            );
        }

        /*empty level*/
        [When(@"I try to add the skill ""(.*)"" with an empty level")]
        public void WhenITryToAddTheSkillWithAnEmptyLevel(string skill)
        {
            _testDataContext.SkillRowCountBeforeAction =
                _skillPage.GetSkillRowCount();

            _skillPage.AddSkillWithoutLevel(skill);
        }



        [Given("a skill exists in the skill list")]
        public void GivenASkillExistsInTheSkillList()
        {
            if (!_skillPage.IsSkillDisplayed(_skill))
            {
                _skillPage.AddSkill(_skill, _level);
            }
        }

        [When("I delete the skill")]
        public void WhenIDeleteTheSkill()
        {
            _skillPage.DeleteSkill(_skill);
        }

        [Then("the skill should be removed from the skill list")]
        public void ThenTheSkillShouldBeRemovedFromTheSkillList()
        {
            Assert.That(
                _skillPage.IsSkillRemoved(_skill),
                Is.True,
                "The skill should be removed from the skill list."
            );
        }

        [When("I edit the skill with new valid details")]
        public void WhenIEditTheSkillWithNewValidDetails()
        {
            _skillPage.EditSkill(_skill, _updatedSkill, _updatedLevel);
        }

        [Then("the updated skill should be displayed in the skill list")]
        public void ThenTheUpdatedSkillShouldBeDisplayedInTheSkillList()
        {
            Assert.That(
                _skillPage.IsSkillDisplayed(_updatedSkill),
                Is.True,
                "The updated skill should be displayed in the skill list."
            );
        }

        private void DeleteSkillTestDataIfExists()
        {
            _skillPage.DeleteSkillIfExists(_updatedSkill);
            _skillPage.DeleteSkillIfExists(_skill);
        }
    }
}