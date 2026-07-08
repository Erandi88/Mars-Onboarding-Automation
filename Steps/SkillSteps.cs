using NUnit.Framework;
using qa_dotnet_cucumber.Pages;
using Reqnroll;

namespace qa_dotnet_cucumber.Steps
{
    [Binding]
    public class SkillSteps
    {
        private readonly SkillPage _skillPage;

        private readonly string _skill = "AutomationSkill";
        private readonly string _level = "Beginner";

        private readonly string _updatedSkill = "AutomationCommunication";
        private readonly string _updatedLevel = "Intermediate";


        public SkillSteps(SkillPage skillPage)
        {
            _skillPage = skillPage;
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

        [When("I add a new skill with valid details")]
        public void WhenIAddANewSkillWithValidDetails()
        {
            DeleteSkillTestDataIfExists();
            _skillPage.AddSkill(_skill, _level);
        }

        [Then("the skill should be displayed in the skill list")]
        public void ThenTheSkillShouldBeDisplayedInTheSkillList()
        {
            Assert.That(_skillPage.IsSkillDisplayed(_skill),Is.True,"The added skill should be displayed in the skill list.");
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