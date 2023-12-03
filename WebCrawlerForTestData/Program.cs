using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Newtonsoft.Json;
using WebCrawlerForTestData;
using System.Xml.Linq;


List<TestData> ldata = new();

try
{
    bool ageOver23 = true;
    int children = 0;
    int taxClass = 1;

    for (int i = 0; i < 20000; i = i + 10)
    {
        TestData testData = new TestData();
        testData.AgeOver23 = ageOver23;
        testData.Children = children;
        testData.TaxClass = taxClass;
        testData.Income = i;

        string pDriver = @"..\..\..\BrowserDriver";

        IWebDriver driver = new ChromeDriver(pDriver);

        driver.Navigate().GoToUrl("https://brutto-netto-rechner.info");

        driver.Manage().Window.Maximize();

        // Use Selenium in C# to check if an HTML element exists using XPath. 
        // The XPath to check is: //*[@id="consentDialog"]/div[2]/div[2]/div/div[2]/div/div[1]/div/div
        // Initialize the WebDriver, navigate to the URL, and check for the element's existence.

        IWebElement cookies = driver.FindElement(By.XPath("/html/body/div[10]//div/div/div[2]/div[2]/div/div[2]/div/div[1]/div/div"));

        if (cookies != null)
        {
            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
            jse.ExecuteScript("arguments[0].click();", cookies);
        }

        IWebElement btn = driver.FindElement(By.XPath("/html/body/div[2]/div[4]/div[1]/div[3]/form/table/tbody/tr[23]/td/input[1]"));

        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
        js.ExecuteScript("arguments[0].click();", btn);



        //btn.Click();

        string a = driver.FindElement(By.XPath("//*[@id=\"wide_content_small\"]/div[3]/table/tbody/tr[22]/td[3]/b")).Text.Replace("€", "").Replace(",", ".");

        testData.TransferSum = Convert.ToDecimal(a);

        ldata.Add(testData);

        driver.Quit();

        Task.WaitAll(Task.Delay(1000));

    }

    if (!Directory.Exists("../../../Data"))
    {
        Directory.CreateDirectory("../../../Data");
    }

    using (StreamWriter file = File.CreateText("../../../Data/TestData.json"))
    {
        JsonSerializer serializer = new JsonSerializer();
        serializer.Serialize(file, ldata);
    }

}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}



