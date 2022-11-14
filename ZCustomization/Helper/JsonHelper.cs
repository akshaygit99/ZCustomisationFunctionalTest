using ZCustomization.Config;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TechTalk.SpecFlow;
public class JsonHelper
{
    public string ReadJsonFile(Table table)
        {
            string inputParameters = null;
            foreach (var row in table.Rows)
            {
                inputParameters = ReadJsonFile(row["FileName"]);
            }
            return inputParameters;
        }
    
    public string ReadJsonFile(string fileName)
            {
                string assemblyPath = AppDomain.CurrentDomain.BaseDirectory;
                var fullPathofFile = Path.Combine(assemblyPath, @"" + fileName).Replace("Environment", EnvironmentConfig.getEnvironment());
                string inputData = File.ReadAllText(fullPathofFile);
                return inputData;
            }

     public string BuildRequestURL(string requestUrl, string jsonInputParams)
            {
                if (jsonInputParams == null)
                {
                    return requestUrl;
                }
                else
                {
                    IDictionary<string, string> jsonInputCSharp = JsonConvert.DeserializeObject<IDictionary<string, string>>(jsonInputParams);
                    List<string> stringValues = new List<string>();
                    foreach (var item in jsonInputCSharp)
                    {
                        if (!string.IsNullOrWhiteSpace(item.Value))
                        {
                            stringValues.Add(item.Key + "=" + item.Value);
                        }
                    }
                    return string.Format("{0}?{1}", requestUrl, string.Join("&", stringValues));
                }
            }
        
    public string GetDataByEnvironment(string parameter)
            {
                string environmentData = ReadJsonFile(@"Config/API_Data_Config.json");
                var inputJsonObject = JsonConvert.DeserializeObject<API_Data_Config>(environmentData);

                var envConfig = inputJsonObject.GetDataByEnvironment
                    .Where(x => x.environment == EnvironmentConfig.getEnvironment()).Select(x => x.environmentData)
                    .ToList();
                foreach (var config in envConfig)
                {
                    foreach (var item in config)
                    {
                        if (item.key == parameter)
                            parameter = item.value;
                    }
                }
                return parameter;
            }

    
}