using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Data.Models.DTO;
using DotNetEnv;

namespace API.Services
{
    public class Auth0Service
    {
        private readonly IConfiguration _configuration;
        private readonly AuthenticationApiClient _auth0Client;

        public Auth0Service(IConfiguration configuration)
        {
            _configuration = configuration;
            Env.Load();
            _auth0Client = new AuthenticationApiClient(Env.GetString("Auth0__Domain"));
        }

        public async Task<AccessTokenResponse> GetAccessToken(string code, string role)
        {
            string clientId = Env.GetString("Auth0__ClientId");
            string clientSecret = Env.GetString("Auth0__ClientSecret");

            // Use the 'role' parameter to determine if it's a customer or vendor registration
            // For example, if (role == "customer"), perform customer registration logic.
            // If (role == "vendor"), perform vendor registration logic.

            return await _auth0Client.GetTokenAsync(new AuthorizationCodeTokenRequest
            {
                ClientId = clientId,
                ClientSecret = clientSecret,
                Code = code,
            });
        }

        public async Task<AccessTokenResponse> RegisterCustomerAndIssueToken(CustomerRegisterRequest request)
        {
            // Your registration logic here...

            // After successfully registering the customer, issue an Auth0 token
            var accessTokenResponse = await GetAccessToken("code_from_registration", "customer");

            return accessTokenResponse;
        }

        public async Task<AccessTokenResponse> RegisterVendorAndIssueToken(VendorRegisterRequest request)
        {
            // Your registration logic here...

            // After successfully registering the customer, issue an Auth0 token
            var accessTokenResponse = await GetAccessToken("code_from_registration", "vendor");

            return accessTokenResponse;
        }

        public async Task<AccessTokenResponse> LoginCustomerAndIssueToken(CustomerLoginRequest request)
        {
            // Your login logic here...

            // After successfully logging in the customer, issue an Auth0 token
            var accessTokenResponse = await GetAccessToken("code_from_login", "customer");

            return accessTokenResponse;
        }

        public async Task<AccessTokenResponse> LoginVendorAndIssueToken(VendorLoginRequest request)
        {
            // Your login logic here...

            // After successfully logging in the vendor, issue an Auth0 token
            var accessTokenResponse = await GetAccessToken("code_from_login", "vendor");

            return accessTokenResponse;
        }
    }
}
