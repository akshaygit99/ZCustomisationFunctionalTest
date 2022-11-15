using AventStack.ExtentReports;
using System;
using TechTalk.SpecFlow;
using ZCustomization.Config;

namespace ZCustomization
{
    [Binding]

    public sealed class Hooks
    {
        public static ExtentReports extent;
        public static ExtentTest test;
        private readonly ScenarioContext _scenarioContext;
        public Hooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeTestRun()]
        public static void BasicSetUp()
        {
            string currentDate = DateTime.Now.ToString("dddd, dd MMMM yyyy ");
            string currentTime = DateTime.Now.ToShortTimeString().ToString().Replace(":", ".");
            string currentDateTime = currentDate + currentTime;

            string pth = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string actualPath = pth.Substring(0, pth.LastIndexOf("bin"));
            string projectPath = new Uri(actualPath).LocalPath;
            string reportPath = projectPath + "Reports//TestResultReport_" + currentDateTime + ".html";
            extent = new ExtentReports();
            var extentHtml = new AventStack.ExtentReports.Reporter.ExtentHtmlReporter(reportPath);
            extent.AttachReporter(extentHtml);
            extentHtml.LoadConfig(projectPath + "extent-config.xml");
            extent.AddSystemInfo("Environment", EnvironmentConfig.getEnvironment());
        }

        [BeforeScenario()]
        public void BeforeScenarioSetUp()
        {
            string testName = _scenarioContext.ScenarioInfo.Title;
            test = extent.CreateTest(testName);
        }

        [AfterTestRun()]
        public static void EndReport()
        {
            extent.Flush();
        }
    }
}