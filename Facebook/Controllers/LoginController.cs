using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Facebook
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        OAuthFacebook facebook;
        public LoginController(IHttpClientFactory clientFactory)
        {
            facebook = new OAuthFacebook(clientFactory, 4078143878928916, "380d8eb65cb6af0489e930acfac0094e", "https://localhost:44339/login/authenticate/");
        }


        [HttpGet("Authenticate")]
        public async Task<IActionResult> Authenticate()
        {
            var response = await facebook.AuthenticateAsync(Request.Query["code"]);
            
            return Ok(response);
        }
    }
}
