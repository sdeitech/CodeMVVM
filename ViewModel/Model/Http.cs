using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace ViewModel.Model
{
    /// <summary>
    /// Static class providing HTTP request methods.
    /// </summary>
    public static class Http
    {
        // Timeout for HTTP requests in seconds
        private const int REQ_TIMEOUT_SECS = 100;

        // Base API URL
        private static string _APIURL;

        /// <summary>
        /// Executes an HTTP POST request with JSON data and returns a JsonResponse.
        /// </summary>
        /// <typeparam name="T">Type of data to be received in the response.</typeparam>
        /// <param name="methodUrl">Relative URL of the API method.</param>
        /// <param name="JSON">Object representing JSON data to be sent in the request body.</param>
        /// <param name="authToken">Authentication token (optional).</param>
        /// <returns>A JsonResponse containing the response data, status, and message.</returns>
        public static JsonResponse<T> ExecuteJSONPOST<T>(string methodUrl, object JSON, string authToken = null)
        {
            // Set the base API URL
            _APIURL = "https://localhost:7298";

            // Initialize an HttpResponseMessage
            HttpResponseMessage response;

            // Create a new HttpClient
            using (HttpClient client = new HttpClient())
            {
                // Add authorization header if authToken is provided, otherwise, accept JSON
                if (!string.IsNullOrEmpty(authToken))
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + authToken);
                else
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                // Serialize JSON object to a string
                var Json = JsonConvert.SerializeObject(JSON);

                // Initialize HttpContent with JSON string
                HttpContent content = new StringContent(Json, Encoding.UTF8, "application/json");

                // Build the complete API URL
                var completeUrl = $"{_APIURL}/{methodUrl}";
            
                try
                {
                    // Perform the POST request and get the response
                    response = client.PostAsync(String.Concat(_APIURL, methodUrl), content).Result;
                    //or you can you directly API link with method url
                    //response = client.PostAsync(String.Concat("https://localhost:7298/api/Setup/GetCompanyForSetup"), content).Result;

                    // Check if the response is a Bad Request
                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        return new JsonResponse<T>
                        {
                            Status = (int)HttpStatusCode.BadRequest,
                            Message = "Bad Request"
                        };
                    }

                    // Check if the response is successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content as a string
                        var result = response.Content.ReadAsStringAsync().Result;

                        // Deserialize the response content to JsonResponse<T> object
                        var resultObject = JsonConvert.DeserializeObject<JsonResponse<T>>(result);

                        return new JsonResponse<T>
                        {
                            Data = resultObject.Data,
                            Status = (int)HttpStatusCode.OK,
                            Message = "Success"
                        };
                    }
                    else
                    {
                        // Throw an exception for unsuccessful responses
                        throw new Exception(string.Format("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase));
                    }
                }
                catch (ApplicationException ex)
                {
                    // Catch application-specific exceptions related to internet connectivity
                    throw new ApplicationException("There seems to be some internet connectivity issues. Please check and try again later.");
                }
            }
        }

        /// <summary>
        /// Generic class for representing JSON responses.
        /// </summary>
        /// <typeparam name="T">Type of data to be contained in the response.</typeparam>
        public class JsonResponse<T>
        {
            public T? Data { get; set; }
            public Int64 Status { get; set; }
            public string Message { get; set; }
        }
    }
}

