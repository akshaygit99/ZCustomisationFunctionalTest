using System.Collections.Generic;

namespace ZCustomization.Config
{
 public class EnvironmentData
    {
        public string key { get; set; }
        public string value { get; set; }

    }

 public class GetDataByEnvironment
    {
        public string environment { get; set; }
        public IList<EnvironmentData> environmentData { get; set; }

    }

public class API_Data_Config
    {
        public IList<GetDataByEnvironment> GetDataByEnvironment { get; set; }

    }

}