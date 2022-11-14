using System;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ZCustomization
{
    public class HttpClientHelper
    {
        public string response = null;
        public string requestUrl = null;
        readonly Stopwatch timer = new Stopwatch();

        HttpClient client = new HttpClient();
        JsonHelper jsonHelper = new JsonHelper();


    public class ApiResponse
        {
            public int statusCode;
            public string statusMessage;
            public string jsonResponse;
            public int responseTimeInMilliseconds;
        }

    public ApiResponse getApiResponse(string endpoint, string methodType, string jsonInput)
    {

        ApiResponse response = new ApiResponse();


        try
        {
            switch(methodType.ToLower())
            {
                case "get":
                    timer.Start();
                    var responseTask = client.GetAsync(client.BaseAddress);
                    responseTask.Wait();
                    timer.Stop();
                    response = getAllData(responseTask,timer);
                    return response;

                case "post":
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress);
                    httpRequestMessage.Content = new StringContent(jsonInput, Encoding.UTF8, "application/json");
                    timer.Start();
                    responseTask = client.SendAsync(httpRequestMessage);
                    responseTask.Wait();
                    timer.Stop();
                    response = getAllData(responseTask,timer);
                    return response;
                
                case "put":
                    timer.Start();
                    responseTask = client.PutAsync(client.BaseAddress, new StringContent(jsonInput));
                    responseTask.Wait();
                    timer.Start();
                    response = getAllData(responseTask, timer);
                    return response;

                case "delete":
                    timer.Start();
                    responseTask = client.DeleteAsync(client.BaseAddress);
                    responseTask.Wait();
                    timer.Stop();
                    response = getAllData(responseTask, timer);
                    return response;

                    default:
                    return null;
            }

        }

        catch(WebException ex)
        {
            timer.Stop();
                HttpWebResponse responseEx = (HttpWebResponse)ex.Response;
                response.statusCode = (int)responseEx.StatusCode;
                response.statusMessage = Convert.ToString(responseEx.StatusCode);
                response.jsonResponse = ex.Message;
                response.responseTimeInMilliseconds = Convert.ToInt32(timer.ElapsedMilliseconds);
            return response;
        }


    }

    public ApiResponse getAllData(Task<HttpResponseMessage> responseTask, Stopwatch timer)
    {
        ApiResponse apiResponse = new ApiResponse
        {
            jsonResponse = responseTask.Result.Content.ReadAsStringAsync().Result,
            statusCode = (int)responseTask.Result.StatusCode,
            statusMessage = Convert.ToString(responseTask.Result.StatusCode),
            responseTimeInMilliseconds = Convert.ToInt32(timer.ElapsedMilliseconds)

        };

        return apiResponse;

    }

    }



}