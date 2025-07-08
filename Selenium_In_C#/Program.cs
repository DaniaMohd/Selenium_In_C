using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

class Program
{
    // MVP Minimum Viable Product
    // at least the selenium driver works
    // it compiles
    // it can login
    static void Main()
    {
        // Start chrome browser
        IWebDriver driver = new ChromeDriver();

        // Now the naive way to wait for the page to load is to sleep for a few seconds.
        //
        //       // Wait for a few seconds for page to load
        //       System.Threading.Thread.Sleep(3000); // This is 3 seconds
        //
        // However this is very rigid, what if the page took 30 seconds to load? 
        // The test would fail when it really just took the page a bit longer to load.
        // Leaving 30 second sleeps everywhere also wastes time.
        // Remember, if you set sleep for 30 seconds, the program is literally doing nothing for 30 seconds. 
        // And a lot of 30 seconds sleeping times adds up to a test that takes too long to run for no reason.
        // We can do better.

        // Set Implicit Timeouts
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);

        // Implicit timeouts are set by the driver itself for any calls to driver.
        // that has a response. 
        // so now `driver.FindElement(By.Name("username"));` will actually wait until 
        // theres a response for up to 15 seconds. 
        // Now responses might take anywhere from 1 second to 30 seconds. 
        // So instead of always sleeping for 30 seconds, once the response was received 
        // we don't have to keep sleeping and are able to continue the code.

        // Open a website
        driver.Navigate().GoToUrl("https://opensource-demo.orangehrmlive.com/");

        /** THIS SECTION IS ATTEMPT AT EXPLICIT WAIT; WE WILL CHECK WHY IT DOESN'T WORK LATER
        // Set up explicit wait
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));

        // Wait for username input to appear
        IWebElement username = wait.Until(d => d.FindElement(By.Name("username")));
        IWebElement password = wait.Until(d => d.FindElement(By.Name("password")));
        IWebElement loginButton = wait.Until(d => d.FindElement(By.CssSelector("button[type='submit']")));
        **/

        IWebElement username = driver.FindElement(By.Name("username")); 
        IWebElement password = driver.FindElement(By.Name("password")); 
        IWebElement loginButton = driver.FindElement(By.CssSelector("button[type='submit']")); 

        // Enter credentials
        username.SendKeys("Admin");
        password.SendKeys("admin123");
        loginButton.Click();


        //// 🏃‍♂️ Click profile dropdown
        //IWebElement profileIcon = wait.Until(d => d.FindElement(By.CssSelector(".oxd-userdropdown-tab")));
        //profileIcon.Click();

        //// 🏃‍♂️ Click Logout
        //var logoutButton = wait.Until(d => d.FindElement(By.XPath("//a[text()='Logout']")));
        //logoutButton.Click();

        ////// Wait for a few seconds for results to load
        ////System.Threading.Thread.Sleep(3000);

        //// Confirm logout by waiting for login page again
        //wait.Until(d =>
        //{
        //    var usernameField = d.FindElement(By.CssSelector("input[name='username']"));
        //    return (usernameField.Displayed && usernameField.Enabled) ? usernameField : null;
        //});
        //Console.WriteLine("✅ Successfully logged out.");



        // Close the browser
        driver.Quit();
    }
}