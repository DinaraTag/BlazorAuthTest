using BlazorWATest.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorWATest.Pages.Auth
{
    public partial class Login
    {
        [Inject]
        public IJSRuntime jsRuntime { get; set; }

        [Inject]
        public NavigationManager navigationManager { get; set; }

        [Inject]
        public AuthenticationStateProvider authStateProvider { get; set; }
        
        LoginRequest model = new LoginRequest();
        bool isBusy = false;
        string message = string.Empty;
        AlertMessageType messageType = AlertMessageType.Success;        

        public async Task LoginUser()
        {
            isBusy = true;
            
            try
            {               
                await jsRuntime.InvokeVoidAsync("getUser", model.Email, model.Password);
                await Task.Delay(100);                
                await authStateProvider.GetAuthenticationStateAsync();
                navigationManager.NavigateTo("/");
            }
            catch(Exception ex)
            {
                message = ex.Message;
                messageType = AlertMessageType.Error;
            }
            isBusy = false;
        }

    }
}
