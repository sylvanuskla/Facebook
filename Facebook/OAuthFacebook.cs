using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Facebook
{
    public class OAuthFacebook
    {
        IHttpClientFactory clientFactory;
        public OAuthFacebook(IHttpClientFactory clientFactory, long clientId, string clientSecret, string redirectUrl)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            RedirectUrl = redirectUrl;
            this.clientFactory = clientFactory;
        }

        public long ClientId { get; private set; }
        public string ClientSecret { get; private set; }
        public string RedirectUrl { get; private set; }
        public string GetLoginUrl(string state) =>
            $"https://www.facebook.com/v11.0/dialog/oauth?client_id={ClientId}&redirect_uri={RedirectUrl}&state={state}&scope=email";


        public async Task<string> AuthenticateAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return null;

            var queryParams = new Dictionary<string, string>()
                {
                    {"client_id", ClientId.ToString() },
                    {"client_secret", ClientSecret },
                    {"redirect_uri", RedirectUrl },
                    {"code",code }
                };

            var url = QueryHelpers.AddQueryString("https://graph.facebook.com/v11.0/oauth/access_token", queryParams);

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var client = clientFactory.CreateClient("kla");

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseText = await response.Content.ReadAsStringAsync();
                var obj = Utility.DeserializeJson(responseText, new { access_token = "", token_type = "", expires_in = 0 });

                queryParams = new Dictionary<string, string>()
                {
                    {"fields", "id,email,first_name,last_name" },
                    {"access_token", obj.access_token }
                };

                url = QueryHelpers.AddQueryString("https://graph.facebook.com/me", queryParams);

                request = new HttpRequestMessage(HttpMethod.Get, url);
                response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    responseText = await response.Content.ReadAsStringAsync();
                    responseText = Regex.Replace(responseText, @"\\u([\dA-Fa-f]{4})", v => ((char)Convert.ToInt32(v.Groups[1].Value, 16)).ToString());

                    return responseText;
                }

            }

            return null;
            

        }
    }
}
