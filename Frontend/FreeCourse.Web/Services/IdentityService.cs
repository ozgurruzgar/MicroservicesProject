using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Models;
using FreeCourse.Web.Services.Interfaces;
using IdentityModel.Client;
using Microsoft.Extensions.Options;

namespace FreeCourse.Web.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ClientSettings _clientSettings;
        private readonly ServiceApiSettings _serviceApiSettings;

        public IdentityService(HttpClient httpClient, IHttpContextAccessor contextAccessor, IOptions<ClientSettings> clientSettings, IOptions<ServiceApiSettings> serviceApiSettings)
        {
            _httpClient = httpClient;
            _contextAccessor = contextAccessor;
            _clientSettings = clientSettings.Value;
            _serviceApiSettings = serviceApiSettings.Value;
        }

        public Task<TokenResponse> GetAccessTokenByRefreshToken()
        {
            throw new NotImplementedException();
        }

        public Task RevokeRefreshToken()
        {
            throw new NotImplementedException();
        }

        public async Task<Response<bool>> SignIn(SignInInput input)
        {
            var discovery = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSettings.BaseUrl,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });

            if (!discovery.IsError)
                throw discovery.Exception;
            else
            {
                var passwordTokenRequest = new PasswordTokenRequest
                {
                    ClientId = _clientSettings.WebClientForUser.ClientId,
                    ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
                    UserName = input.Email,
                    Password = input.Password,
                    Address = discovery.TokenEndpoint,
                };

                var token = await _httpClient.RequestPasswordTokenAsync(passwordTokenRequest);
                //if (!token.IsError)

            }
        }
    }
}
