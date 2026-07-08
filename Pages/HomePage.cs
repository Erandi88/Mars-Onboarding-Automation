using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace qa_dotnet_cucumber.Pages
{
    public class HomePage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public HomePage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));
        }

        public bool IsHomePageLoaded()
        {
            string baseUrl = Hooks.Hooks.Settings.Environment.BaseUrl;

            _wait.Until(driver => driver.Url.Contains(baseUrl));

            return _driver.Url.Contains(baseUrl);
        }
    }
}