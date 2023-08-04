using Microsoft.Extensions.Configuration;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.PeopleService.v1;
using Google.Apis.Auth;
using System.Threading.Tasks;
using DotNetEnv;

namespace API.Services
{
    public class GoogleService
    {
        private readonly IConfiguration _configuration;
        private readonly string _googleClientId;
        private readonly string _googleClientSecret;

        public GoogleService(IConfiguration configuration)
        {
            Env.Load(); // Load .env file
            _configuration = configuration;
            _googleClientId = Env.GetString("Google__ClientId");
            _googleClientSecret = Env.GetString("Google__ClientSecret");
        }

        public async Task<PeopleServiceService> GetPeopleService()
        {
            UserCredential credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = _googleClientId,
                    ClientSecret = _googleClientSecret,
                },
                new[] { PeopleServiceService.Scope.ContactsReadonly },
                "user", // User
                System.Threading.CancellationToken.None
            );

            // Create the PeopleServiceService using the authorized credentials
            var service = new PeopleServiceService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "HomemadeGoodies"
            });

            return service;
        }

        public async Task<GoogleJsonWebSignature.Payload> Verify(string idToken)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { _googleClientId } // Your Google Client ID
                });

                return payload;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return null; // Return null to indicate invalid token
            }
        }
    }
}
