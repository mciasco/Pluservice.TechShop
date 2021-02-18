using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WSF.Authentication;

namespace WSF.Handlers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserAuthenticatorService _userAuthenticatorService;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options, 
            ILoggerFactory logger, 
            UrlEncoder encoder, 
            ISystemClock clock,
            IUserAuthenticatorService userAuthenticatorService) 
            : base(options, logger, encoder, clock)
        {
            _userAuthenticatorService = userAuthenticatorService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if(!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("No authorization header found");

            string username = "";
            string password = "";
            try
            {
                var authenticationHeaderValue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var bytes = Convert.FromBase64String(authenticationHeaderValue.Parameter);
                var stringCredentials = Encoding.UTF8.GetString(bytes).Split(":");
                username = stringCredentials[0];
                password = stringCredentials[1];
            }
            catch (Exception e)
            {
                return AuthenticateResult.Fail("Error in authentication header parameters");
            }
            
            if(!_userAuthenticatorService.TryAuthenticate(username, password))
                return AuthenticateResult.Fail("User is not authenticated");

            var claims = new[] {new Claim(ClaimTypes.Name, username)};
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            
            return AuthenticateResult.Success(ticket);
        }
    }

    
}
