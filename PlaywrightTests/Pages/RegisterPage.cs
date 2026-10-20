using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlaywrightTests.Pages
{
    public class RegisterPage
    {
        private readonly IPage _page;

        private readonly ILocator _emailInput;
        private readonly ILocator _loginInput;
        private readonly ILocator _passwordInput;

        private readonly ILocator _submitButton;
        private readonly ILocator _regRef;

        public readonly ILocator SwalToast;


        public RegisterPage(IPage page)
        {
            _page = page;
            _emailInput = page.GetByTestId("email-input");
            _loginInput = page.GetByTestId("login-input");
            _passwordInput = page.GetByTestId("password-input");
            _submitButton = page.GetByTestId("submit-btn");
            _regRef = page.GetByTestId("reg-ref");
            SwalToast = page.GetByRole(AriaRole.Alert);
        }

        public async Task GotoAsync()
        {
            await _page.GotoAsync("https://localhost:7280/authorization");
            await _regRef.ClickAsync();
        }

        public async Task RegAsync(string email, string login, string password)
        {
            await _emailInput.FillAsync(email);
            await _loginInput.FillAsync(login);
            await _passwordInput.FillAsync(password);
            await _submitButton.ClickAsync();
        }
    }
}
