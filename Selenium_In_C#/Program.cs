using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

class Program
{
    static void Main()
    {
        // start chrome browser
        IWebDriver driver = new ChromeDriver();

        //open a website
        driver.Navigate().GoToUrl("https://www.google.com");

        //find the search box
        IWebElement searchbox = driver.FindElement(By.Name("q"));

        //type in the search box
        searchbox.SendKeys("Selenium C# example");

        //submit input
        searchbox.Submit();

        //wait for a few seconds for results to load
        System.Threading.Thread.Sleep(3000);

        //close the browser
        driver.Quit();
    }
}