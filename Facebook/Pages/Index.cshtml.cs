using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Facebook.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public OAuthFacebook facebook { get; private set; }

        public IndexModel(IHttpClientFactory clientFactory, ILogger<IndexModel> logger)
        {
            _logger = logger;
            facebook = new OAuthFacebook(clientFactory, 4078143878928916, "380d8eb65cb6af0489e930acfac0094e", "https://localhost:44339/login/authenticate/");
        }

        public void OnGet()
        {

        }
    }
}
