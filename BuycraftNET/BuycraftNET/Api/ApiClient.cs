using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace BuycraftNET.Api
{
    public class ApiClient
    {

        private BuycraftNET _plugin;

        public ApiClient(BuycraftNET Plugin)
        {
            _plugin = Plugin;
        }
        
        private async Task<JObject> Get(string url)
        {
            Console.WriteLine("GETting " + url);
            using (var httpClient = new HttpClient())
            {
                try
                {
                    
                    var request = new HttpRequestMessage() {
                        RequestUri = new Uri(url),
                        Method = HttpMethod.Get,
                    };
                    
                    request.Headers.Add("X-Buycraft-Secret", _plugin.GetConfig("secret"));
                    
                    var response = await httpClient.SendAsync(request).ConfigureAwait(false);
                    var jsonTask = response.Content.ReadAsStringAsync();
                    var jsonString = jsonTask.Result;
                    JObject json = JObject.Parse(jsonString);
                    httpClient.Dispose();
                    
                    if (!response.IsSuccessStatusCode)
                    {
                        JToken errorMessage;
                        
                        if (json.TryGetValue("error_message", out errorMessage))
                        {
                            throw new Exception(errorMessage.ToString());
                        }
                        else
                        {
                            throw new Exception("There was a problem accessing the API. Status code: " + response.StatusCode);    
                        }
                        
                    }
                    
                    return json;
                }
                catch (HttpRequestException e)
                {
                    throw new Exception("There was a problem accessing the API. " + e.Message);
                }
            }
        }

        public async Task<JObject> GetInformation()
        {
            try
            {
                var response = await this.Get("https://plugin.buycraft.net/information").ConfigureAwait(false);
                Console.WriteLine("Getting Response");
                return response;
            }
            catch (Exception e)
            {
                _plugin.LogError(e.Message);
                return null;
            }
            
        }
        
        public async Task<JObject> GetDuePlayers()
        {
            try
            {
                var response = await this.Get(
                    "https://plugin.buycraft.net/queue?"
                    ).ConfigureAwait(false);
                Console.WriteLine("Getting Response");
                return response;
            }
            catch (Exception e)
            {
                _plugin.LogError(e.Message);
                return null;
            }            
        }
        
        public async Task<JObject> GetPlayerQueue(int playerId)
        {
            try
            {
                var response = await this.Get(
                    "https://plugin.buycraft.net/queue/online-commands/" + playerId
                ).ConfigureAwait(false);
                Console.WriteLine("Getting Response");
                return response;
            }
            catch (Exception e)
            {
                _plugin.LogError(e.Message);
                return null;
            }    
        }        
    }   
}