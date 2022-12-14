using Blazored.LocalStorage;
using BlazorWATest.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorWATest
{
    public class LocalAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _storageService;

        public LocalAuthenticationStateProvider(ILocalStorageService storageService)
        {
            _storageService = storageService;
        }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {           

            if (await _storageService.ContainKeyAsync("User"))
            {
                var userInfo = await _storageService.GetItemAsync<LocalUserInfo>("User");
                var accessToken = Guid.NewGuid().ToString();
                var claims = new[]
                {
                    new Claim("Email", userInfo.email),
                    new Claim("FirstName", userInfo.firstName),
                    new Claim("LastName", userInfo.lastName),                    
                    new Claim(ClaimTypes.Role, userInfo.position),
                    new Claim(ClaimTypes.NameIdentifier, accessToken)
                };

                var identity = new ClaimsIdentity(claims, "BearerToken");
                var user = new ClaimsPrincipal(identity);
                var state = new AuthenticationState(user);

                NotifyAuthenticationStateChanged(Task.FromResult(state));
                return state;
            }
            return new AuthenticationState(new ClaimsPrincipal());
        }

        public async Task LogOutAsync()
        {
            await _storageService.RemoveItemAsync("User");
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal())));
        }
    }
}
