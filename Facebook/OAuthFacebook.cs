using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Facebook
{
    public class OAuthFacebook : OAuthClientBase
    {
        public OAuthFacebook(IHttpClientFactory clientFactory, string clientId, string clientSecret, string redirectUrl) :
            base(clientFactory, clientId, clientSecret, redirectUrl)
        {
           
        }


        public override string LoginPageUrl => $"https://www.facebook.com/v11.0/dialog/oauth?client_id={ClientId}&redirect_uri={RedirectUrl}&state={State}&scope=email";

        public override async Task<string> RequestAccessToken(string athorizationCode)
        {
            if (string.IsNullOrWhiteSpace(athorizationCode))
                return null;

            var queryParams = new Dictionary<string, string>()
                {
                    {"client_id", ClientId.ToString() },
                    {"client_secret", ClientSecret },
                    {"redirect_uri", RedirectUrl },
                    {"code",athorizationCode }
                };

            var url = QueryHelpers.AddQueryString("https://graph.facebook.com/v11.0/oauth/access_token", queryParams);

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await HttpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseText = await response.Content.ReadAsStringAsync();
                var json = Utility.DeserializeJson(responseText, new { access_token = "", token_type = "", expires_in = 0 });

                return json.access_token;
            }

            return null;

        }

        public override async Task<string> RequestUserData(string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
                return null;

            var queryParams = new Dictionary<string, string>()
                {
                    {"fields", "id,email,first_name,last_name" },
                    {"access_token", accessToken }
                };

            var url = QueryHelpers.AddQueryString("https://graph.facebook.com/me", queryParams);

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var  response = await HttpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
                return DecodeUnicde(await response.Content.ReadAsStringAsync());

            return null;

        }
        
    }
}
