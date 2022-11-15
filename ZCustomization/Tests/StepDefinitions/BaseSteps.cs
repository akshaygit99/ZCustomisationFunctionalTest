using System;
using ZCustomization.Config;
using TechTalk.SpecFlow;
using static ZCustomization.HttpClientHelper;
using NUnit.Framework;

namespace ZCustomization.Tests
{
    [Binding]

    class BaseSteps
    {
        FeatureContext _featureContext;
        ScenarioContext _scenarioContext;

        public BaseSteps(FeatureContext featureContext, ScenarioContext scenarioContext)
        {
            _featureContext = featureContext;
            _scenarioContext = scenarioContext;
        }

        public JsonHelper jsonHelper = new JsonHelper();
        public HttpClientHelper clientHelper = new HttpClientHelper();

        [Given(@"I have a '(.*)' API '(\w+)'")]
        public void GivenIHaveAAPI(string httpVerb, string API)
        {
            _featureContext["requestType"] = httpVerb;
            _featureContext["environment"] = EnvironmentConfig.getEnvironment();
            _featureContext["endpoint"] = jsonHelper.GetDataByEnvironment(API);
            _scenarioContext["inputParameters"] = null;
            if (httpVerb != null && jsonHelper.GetDataByEnvironment(API) != null && EnvironmentConfig.getEnvironment()!=null)
            {
                Hooks.test.Pass("I have got API " + httpVerb+" for "+ API + " successfuly.");
            }
            else
            {
                Hooks.test.Fail("I have not got API " + httpVerb + "for " + API + "in environment "+ EnvironmentConfig.getEnvironment() +" successfuly.");
            }
        }

        [Given(@"I have a json input file")]
        public void GivenIHaveAJsonInputFile(Table table)
        {
            _scenarioContext["inputParameters"] = jsonHelper.ReadJsonFile(table);

            if (jsonHelper.ReadJsonFile(table) != null)
            {
                Hooks.test.Pass("I have got table data " + table + " successfuly.");
            }
            else
            {
                Hooks.test.Fail("I have not got table data " + table + " successfuly");
            }
        }

        [Given(@"I have a json input file '(.*)'")]
        public void GivenIHaveAJsonInputFile(string filePath)
        {
            _scenarioContext["inputParameters"] = jsonHelper.ReadJsonFile(filePath);

            if (jsonHelper.ReadJsonFile(filePath) != null)
            {
                Hooks.test.Pass("I have got file " + filePath + " successfuly.");
            }
            else
            {
                Hooks.test.Fail("I have not the file " + filePath + " successfuly");
            }
        }

        [When(@"I send API request for '(\w+)'")]
        public void WhenISendAPIRequest(string sut)
        {
            ApiResponse apiResponse = clientHelper.getApiResponse(sut, (String) _featureContext["endpoint"], (String) _featureContext["requestType"], (String) _scenarioContext["inputParameters"]);
            _scenarioContext["apiResponse"] = apiResponse;
            
            if (apiResponse!= null)
            {
                Hooks.test.Pass("I have received response successfuly.");
            }
            else
            {
                Hooks.test.Fail("I have not received response successfuly.");
            }
            
        }

        [Then(@"I validate status code (.*)")]
        public void ThenIValidateStatusCode(int status)
        {
            ApiResponse response = (ApiResponse)_scenarioContext["apiResponse"];
            
            if (status.Equals(response.statusCode))
            {
                Hooks.test.Pass("I have status "+ status  + " successfuly.");
            }
            else
            {
                Hooks.test.Fail("I have not got status "+ status  + " successfuly. It is " + response.statusCode);
            }

            Assert.AreEqual(status, response.statusCode);
        }

    }
}