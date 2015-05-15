using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Web;

using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json;


namespace LmvAuthTokenServer.Models
{
    public class AuthToken
    {
        // Replace "yours" with "your" client_id and client_secret. 
        private static List<KeyValuePair<string, string>> Credentials = new List<KeyValuePair<string, string>> 
        {
            { new KeyValuePair<string, string>( "client_id", "<replace with your_client_id>" ) },
            { new KeyValuePair<string, string>( "client_secret", "<replace with your_client_secret>" ) },
            { new KeyValuePair<string, string>( "grant_type", "client_credentials" ) }
        };

        public static BrokerToken TheBrokerToken { get; set; }
        private HttpResponseMessage AResponseMessage { get; set; }
        private DateTime TokenIssuedTime { get; set; }
        private int AboutExpiredSeconds = 5;

        // Check if token has been retrieved and store response.
        //See Autodesk MyAuthToken.js
        public async Task<BrokerToken> GetApiToken()
        {
            //Ddetermine if token has expired.
            if ((TheBrokerToken == null) || 
                ((DateTime.Now - TokenIssuedTime).TotalSeconds
                    > (TheBrokerToken.ExpiresIn - AboutExpiredSeconds)))
            {
                Debug.WriteLine("AUTH TOKEN: First token or token has expired. Get new one ... ");
                TheBrokerToken = await this.GetAccessToken();             
            }
            else
            {
                Debug.WriteLine("AUTH TOKEN: Has not expired. Use existing token. ");
            }
            return TheBrokerToken;
        }

        // See Autodesk AuthTokenServer.js
        // Request token from Autodesk API using credentials
        private async Task<BrokerToken> GetAccessToken()
        {
            string baseUrl = "https://developer.api.autodesk.com/";
            HttpContent reqData = new FormUrlEncodedContent(Credentials);

            try
            {
                using (var client = new HttpClient())
                {
                    string contentText = null;
                    client.BaseAddress = new Uri(baseUrl);
                    reqData.Headers.ContentType = new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded");

                    HttpResponseMessage response = await client.PostAsync("authentication/v1/authenticate", reqData);
                    if (!response.IsSuccessStatusCode) {
                        Debug.WriteLine("AUTH TOKEN: Call to AD authenticate failed.");
                    }
                    contentText = await response.Content.ReadAsStringAsync();
                    TokenIssuedTime = DateTime.Now;
                        Debug.WriteLine(contentText, "AUTH TOKEN: Content text is ");
                        Debug.WriteLine(TokenIssuedTime.GetDateTimeFormats('T')[0], "AUTH TOKEN: Issued time is ");
                        return JsonConvert.DeserializeObject<BrokerToken>(contentText);
                }
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine("AUTH TOKEN: Caught authenticate call to AD API.");
                System.Diagnostics.Debug.WriteLine(exception);
                throw;
            }
        }
    }

    // Helps to deserialize response 
    public class BrokerToken
    {
        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }
        [JsonProperty(PropertyName = "expires_in")]
        public int ExpiresIn { get; set; }
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }
    }
}