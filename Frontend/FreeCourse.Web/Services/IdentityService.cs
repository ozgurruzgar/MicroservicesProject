using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Models;
using FreeCourse.Web.Services.Interfaces;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Globalization;
using System.Security.Claims;
using System.Text.Json;

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

            if (discovery.IsError)
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
                if (token.IsError)
                {
                    var responseContent = await token.HttpResponse.Content.ReadAsStringAsync();

                    var errorDto = JsonSerializer.Deserialize<ErrorDto>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true});

                    return Response<bool>.Fail(errorDto.Errors, 400);
                }

                var userInfoRequestToken = new UserInfoRequest
                {
                    Token = token.AccessToken,
                    Address = discovery.UserInfoEndpoint
                };

                var userInfo = await _httpClient.GetUserInfoAsync(userInfoRequestToken);
                if (userInfo.IsError)
                    throw userInfo.Exception;

                var claimsIdentity = new ClaimsIdentity(userInfo.Claims, CookieAuthenticationDefaults.AuthenticationScheme, "name", "role");

                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                var authenticationProperties = new AuthenticationProperties();
                authenticationProperties.StoreTokens(new List<AuthenticationToken>()
                {
                    new AuthenticationToken { Name = OpenIdConnectParameterNames.AccessToken, Value = token.AccessToken},
                    new AuthenticationToken { Name = OpenIdConnectParameterNames.RefreshToken, Value = token.RefreshToken},
                    new AuthenticationToken { Name = OpenIdConnectParameterNames.ExpiresIn, Value = DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)},
                });

                authenticationProperties.IsPersistent = input.IsRemember;

                await _contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperties);

                return Response<bool>.Success(200);
            }
        }
    }
}
