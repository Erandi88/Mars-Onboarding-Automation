using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace qa_dotnet_cucumber.Pages
{
    public class SkillPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        private readonly By SkillsTab = By.XPath("//a[normalize-space()='Skills']");
        private readonly By AddNewButton = By.XPath("//div[contains(@class,'active')]//div[contains(@class,'ui teal button') and normalize-space()='Add New']");
        private readonly By SkillField = By.XPath("//div[contains(@class,'active')]//input[@placeholder='Add Skill']");
        private readonly By SkillLevelDropdown = By.XPath("//div[contains(@class,'active')]//select[@name='level']");
        private readonly By AddButton = By.XPath("//div[contains(@class,'active')]//input[@value='Add']");
        private readonly By UpdateButton = By.XPath("//div[contains(@class,'active')]//input[@value='Update']");

        public SkillPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));
        }

        public bool IsProfilePageDisplayed()
        {
            _wait.Until(driver => driver.Url.Contains("/Account/Profile"));
            return _driver.Url.Contains("/Account/Profile");
        }

        public void ClickSkillsTab()
        {
            var skillsTab = _wait.Until(ExpectedConditions.ElementToBeClickable(SkillsTab));
            skillsTab.Click();
        }

        public void ClickAddNewButton()
        {
            var addNewButton = _wait.Until(ExpectedConditions.ElementToBeClickable(AddNewButton));
            addNewButton.Click();
        }

        public void EnterSkill(string skill)
        {
            var skillInput = _wait.Until(ExpectedConditions.ElementIsVisible(SkillField));
            skillInput.Clear();
            skillInput.SendKeys(skill);
        }

        public void SelectSkillLevel(string level)
        {
            var dropdownElement = _wait.Until(ExpectedConditions.ElementIsVisible(SkillLevelDropdown));
            var selectElement = new SelectElement(dropdownElement);
            selectElement.SelectByText(level);
        }

        public void ClickAddButton()
        {
            var addButton = _wait.Until(ExpectedConditions.ElementToBeClickable(AddButton));
            addButton.Click();
        }

        public void AddSkill(string skill, string level)
        {
            ClickAddNewButton();
            EnterSkill(skill);
            SelectSkillLevel(level);
            ClickAddButton();
        }

        public bool IsSkillDisplayed(string skill)
        {
            try
            {
                var skillRow = By.XPath($"//div[contains(@class,'active')]//td[normalize-space()='{skill}']");
                return _wait.Until(ExpectedConditions.ElementIsVisible(skillRow)).Displayed;
            }
            catch
            {
                return false;
            }
        }

        private By DeleteButtonForSkill(string skill)
        {
            return By.XPath($"//div[contains(@class,'active')]//td[normalize-space()='{skill}']/following-sibling::td//i[contains(@class,'remove')]");
        }

        public void DeleteSkill(string skill)
        {
            var deleteButton = _wait.Until(ExpectedConditions.ElementToBeClickable( DeleteButtonForSkill(skill)));

            deleteButton.Click();
        }

        
        private By EditButtonForSkill(string skill)
        {
            return By.XPath($"//div[contains(@class,'active')]//td[normalize-space()='{skill}']/following-sibling::td//i[contains(@class,'write')]");
            
        }

        public void ClickEditSkill(string skill)
        {
            var editButton = _wait.Until(ExpectedConditions.ElementToBeClickable(EditButtonForSkill(skill)));
            editButton.Click();
        }

        public void ClickUpdateButton()
        {
            var updateButton = _wait.Until(ExpectedConditions.ElementToBeClickable(UpdateButton));
            updateButton.Click();
        }

        public void EditSkill(string currentSkill, string newSkill, string newLevel)
        {
            ClickEditSkill(currentSkill);
            EnterSkill(newSkill);
            SelectSkillLevel(newLevel);
            ClickUpdateButton();
        }


        public void DeleteSkillIfExists(string skill)
        {
            if (IsSkillDisplayed(skill))
            {
                DeleteSkill(skill);

                if (!IsSkillRemoved(skill))
                {
                    throw new WebDriverTimeoutException(
                        $"The language '{skill}' was not removed during cleanup."
                    );
                }
            }
        }

        // Add a new langauage

        public bool IsSkillAndLevelDisplayed(string skill, string level)
        {
            try
            {
                var skillRow = By.XPath(
                    $"//div[contains(@class,'active')]" +
                    $"//td[normalize-space()='{skill}']" +
                    $"/following-sibling::td[normalize-space()='{level}']"
                );

                return _wait
                    .Until(ExpectedConditions.ElementIsVisible(skillRow))
                    .Displayed;
            }
            catch
            {
                return false;
            }
        }

        public bool IsSkillRemoved(string skill)
        {
            try
            {
                var skillRow = By.XPath(
                    $"//div[contains(@class,'active')]" +
                    $"//td[normalize-space()='{skill}']"
                );

                _wait.Until(
                    ExpectedConditions.InvisibilityOfElementLocated(skillRow)
                );

                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }


    }

}