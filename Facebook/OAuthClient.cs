using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Facebook
{
    public abstract class OAuthClientBase
    {
        protected HttpClient HttpClient { get; private set; }
        protected string ClientId { get; private set; }
        protected string ClientSecret { get; private set; }
        protected string RedirectUrl { get; private set; }
        public string State { get; private set; }

        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public OAuthClientBase(IHttpClientFactory clientFactory, string clientId, string clientSecret, string redirectUrl)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            RedirectUrl = redirectUrl;
            HttpClient = clientFactory.CreateClient();

            var random = new Random();
            State = new string(Enumerable.Repeat(chars, 8)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public abstract string LoginPageUrl { get; }

        public abstract Task<string> RequestAccessToken(string athorizationCode); 

        public abstract Task<string> RequestUserData(string accessToken);

        protected string DecodeUnicde(string str)=> Regex.Replace(str, @"\\u([\dA-Fa-f]{4})", v => ((char)Convert.ToInt32(v.Groups[1].Value, 16)).ToString());
    }
}
