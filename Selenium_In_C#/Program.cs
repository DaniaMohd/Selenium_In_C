using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq.Expressions;

// We now extract out the code for Logging into the Site.
class LoginPage
{
    private IWebDriver driver;
    public string loginPageURL = "https://opensource-demo.orangehrmlive.com/";

    // Dependency Injection
    // To declare to anyone that this class relies on or needs a Driver
    public LoginPage(IWebDriver driver)
    {
        this.driver = driver;
    }


    public void Login(string username, string password)
    {
        IWebElement usernameElement = driver.FindElement(By.Name("username"));
        IWebElement passwordElement = driver.FindElement(By.Name("password"));
        IWebElement loginButton = driver.FindElement(By.CssSelector("button[type='submit']"));

        // Enter credentials
        usernameElement.SendKeys(username);
        passwordElement.SendKeys(password);
        loginButton.Click();
    }
}

class Program
{
    // SRP Single Responsibility Principle
    // Every Class should have one responsibility

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

        // Initialise Login Page Testing
        var loginPage = new LoginPage(driver);

        // Open a website
        driver.Navigate().GoToUrl(loginPage.loginPageURL);

        // Log in with correct username and password
        // For future reference, usernames and passwords should be stored in
        // environment variables and not stored in plaintext or in code like this!
        loginPage.Login("Admin", "admin123");

        // By encapsulating the code responsibile for logging into the page into a class, 
        // Its cleaner now, and modular! You could test failed log ins in the future with this.

        // Not sure how to encapsulate this part of the code for SRP purposes
        // For future improvement. 

        // We now try to see if login was successful by finding the header "Dashboard"
        // If we made it to the Dashboard, we were successful!
        try
        {
            // XPath is a query language that can be used to search for and locate elements in XML and HTML documents.
            // XPath is the preferred locator when other CSS locators (ID, Class, etc.)cannot be found.
            driver.FindElement(By.XPath("//h6[text()='Dashboard']"));
            Console.WriteLine("Login successful (dashboard check)!");
        }
        catch
        {
            Console.WriteLine("Failed to Login!");
        }

        // Go To Maintenance Page
        IWebElement maintenanceMenu = driver.FindElement(By.LinkText("Maintenance"));
        Console.WriteLine("Maintenance menu found.");
        maintenanceMenu.Click();

        // Login to Maintenance Page
        IWebElement passwordElement = driver.FindElement(By.Name("password"));
        IWebElement loginButton = driver.FindElement(By.CssSelector("button[type='submit']"));

        // Enter credentials
        passwordElement.SendKeys("admin123");
        loginButton.Click();

        try
        {
            // We now look if we are in Maintenance Page
            driver.FindElement(By.XPath("//h6[text()='Maintenance']"));
            Console.WriteLine("Successful login to Maintenance Page");
        }
        catch (NoSuchElementException)
        {
            Console.WriteLine("Could not login to Maintenance Page");
        }

        // Finally, we want to log out.

        // Click profile dropdown
        IWebElement profileIcon = driver.FindElement(By.CssSelector(".oxd-userdropdown-tab"));
        profileIcon.Click();

        try
        {
            // Click Logout
            var logoutButton = driver.FindElement(By.XPath("//a[text()='Logout']"));
            logoutButton.Click();


            Console.WriteLine("Logout Successful!");
        }
        catch (NoSuchElementException)
        {
            Console.WriteLine("Logout Failed");
        }

        // Close the browser
        driver.Quit();
    }
}