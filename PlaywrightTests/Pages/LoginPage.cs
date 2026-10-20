using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlaywrightTests.Pages
{
    public class LoginPage
    {
        private readonly IPage _page;

        private readonly ILocator _emailOrLoginInput;
        private readonly ILocator _passwordInput;
        private readonly ILocator _submitButton;

        public readonly ILocator SwalToast;


        public LoginPage(IPage page)
        {
            _page = page;
            _emailOrLoginInput = page.GetByTestId("login-or-email-input");
            _passwordInput = page.GetByTestId("password-input");
            _submitButton = page.GetByTestId("submit-btn");
            SwalToast = page.GetByRole(AriaRole.Alert);
        }

        public async Task GotoAsync()
        {
            await _page.GotoAsync("https://localhost:7280/authorization");
        }

        public async Task LoginAsync(string emailOrLogin, string password)
        {
            await _emailOrLoginInput.FillAsync(emailOrLogin);
            await _passwordInput.FillAsync(password);
            await _submitButton.ClickAsync();
        }
    }
}
