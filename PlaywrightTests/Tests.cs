using LR2WebDesign.Controllers.APIRequestBodies;
using LR2WebDesign.Infrastructure.Repositories;
using LR2WebDesign.Services;
using PlaywrightTests.Pages;
using System.Net.Http.Json;

namespace PlaywrightTests
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class Tests : PageTest
    {
        [Test]
        public async Task LoginTestUserDoesNotExists()
        {
            var loginPage = new LoginPage(Page);
            await loginPage.GotoAsync();
            await loginPage.LoginAsync("user12@example.com", "Password123@");

            await Expect(loginPage.SwalToast).ToBeVisibleAsync();
        }

        [Test]
        public async Task LoginTestUserExists()
        {
            var apiContext = await Playwright.APIRequest.NewContextAsync(new()
            {
                IgnoreHTTPSErrors = true
            });
            var formData = new UserCreateRequestDTO
            {
                Email = "user@example.com",
                Login = "user121",
                Name = "",
                Password = "Password123@"
            };

            var response = await apiContext.PostAsync("https://localhost:7280/admin/createUser", new()
            {
                DataObject = formData
            });

            var loginPage = new LoginPage(Page);
            await loginPage.GotoAsync();
            await loginPage.LoginAsync("user@example.com", "Password123@");

            await Expect(Page).ToHaveURLAsync(new Regex(@"^https?://[^/]+/?$"));

            if (response.Status == 200)
            {
                var json = await response.JsonAsync();
                int userId = json.Value.GetProperty("userId").GetInt32();
                var deleteResponse = await apiContext.PostAsync($"https://localhost:7280/admin/deleteUser?userId={userId}");
            }
        }

        [Test]
        public async Task RegTestIncorrectLoginAndPasswordAndEmail()
        {
            var regPage = new RegisterPage(Page);
            await regPage.GotoAsync();
            await regPage.RegAsync("user", "user","password123");

            await Expect(regPage.SwalToast).ToBeVisibleAsync();
        }

        [Test]
        public async Task RegTestIncorrectLoginAndPassword()
        {
            var regPage = new RegisterPage(Page);
            await regPage.GotoAsync();
            await regPage.RegAsync("user@example.com", "user", "password123");

            await Expect(regPage.SwalToast).ToBeVisibleAsync();
        }

        [Test]
        public async Task RegTestIncorrectLogin()
        {
            var regPage = new RegisterPage(Page);
            await regPage.GotoAsync();
            await regPage.RegAsync("user@example.com", "user", "Password123@");

            await Expect(regPage.SwalToast).ToBeVisibleAsync();
        }

        [Test]
        public async Task RegTestIncorrectPassword()
        {
            var regPage = new RegisterPage(Page);
            await regPage.GotoAsync();
            await regPage.RegAsync("user@example.com", "user123", "password");

            await Expect(regPage.SwalToast).ToBeVisibleAsync();
        }

        [Test]
        public async Task RegTestCorrectAll()
        {
            var regPage = new RegisterPage(Page);
            await regPage.GotoAsync();
            await regPage.RegAsync("user@example.com", "user123", "Password123@");

            await Expect(Page).ToHaveURLAsync(new Regex(@"^https?://[^/]+/?$"));
        }

    }
}
