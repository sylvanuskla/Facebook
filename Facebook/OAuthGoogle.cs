using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Facebook
{
    public class OAuthGoogle : OAuthClientBase
    {
        public OAuthGoogle(IHttpClientFactory clientFactory, string clientId, string clientSecret, string redirectUrl) :
            base(clientFactory, clientId, clientSecret, redirectUrl)
        {
           
        }


        public override string LoginPageUrl
        {
            get
            {
                var queryParams = new Dictionary<string, string>()
                {
                    {"client_id", ClientId.ToString() },
                    {"redirect_uri", RedirectUrl },
                    {"scope", "https://www.googleapis.com/auth/drive.metadata.readonly"},
                    {"response_type", "code" },
                    {"access_type", "offline"},
                    {"state", State}
                };

                var url = QueryHelpers.AddQueryString("https://accounts.google.com/o/oauth2/v2/auth", queryParams);

                return url;
            }
        }


        public override async Task<string> RequestAccessToken(string athorizationCode)
        {
            if (string.IsNullOrWhiteSpace(athorizationCode))
                return null;

            var bodyParams = new Dictionary<string, string>()
            {
                {"client_id", ClientId.ToString() },
                {"client_secret", ClientSecret },
                {"redirect_uri", RedirectUrl },
                {"grant_type", "authorization_code"},
                {"code",athorizationCode }
            };

          
            var request = new HttpRequestMessage(HttpMethod.Post, "https://oauth2.googleapis.com/token") { Content = new FormUrlEncodedContent(bodyParams) };

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

            var request = new HttpRequestMessage(HttpMethod.Get, "https://www.googleapis.com/drive/v2/files");
            request.Headers.Add("Authorization", "Bearer " + accessToken);
            var response = await HttpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
                return DecodeUnicde(await response.Content.ReadAsStringAsync());

            return null;

        }
    }
}
