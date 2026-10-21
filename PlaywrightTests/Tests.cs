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

        [SetUp]
        public async Task Setup()
        {
            await Page.AddInitScriptAsync(@"
                Object.defineProperty(navigator, 'webdriver', {
                    get: () => undefined
                });
            ");

            await Page.Context.SetExtraHTTPHeadersAsync(new Dictionary<string, string>
            {
                { "User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36" }
            });
        }

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
            var loginPage = new LoginPage(Page);
            await loginPage.GotoAsync();
            await loginPage.LoginAsync("user@example.com", "Password123@");
            await Expect(Page).ToHaveURLAsync(new Regex(@"^https?://[^/]+/?$"));
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
            await regPage.RegAsync("user1234@example.com", "user1234", "Password1234@");
            await Expect(Page).ToHaveURLAsync(new Regex(@"^https?://[^/]+/?$"));
        }

        [Test]
        public async Task MainPageTestCourseCardClick()
        {
            var mainPage = new MainPage(Page);
            await mainPage.GotoAsync();
            var newPageUrl = await mainPage.ClickCourseCardAsync();
            var expectedUrl = await mainPage.GetCourseCardHrefAsync();
            Assert.That(newPageUrl, Is.EqualTo(expectedUrl));
        }

        [Test]
        public async Task MainPageTestAiConstructorUserExistsNotAuthorized()
        {
            var mainPage = new MainPage(Page);
            await mainPage.GotoAsync();
            var loginPage = await mainPage.ClickAiConstructorAsync();
            await loginPage.LoginAsync("user@example.com", "Password123@");
            await Expect(Page).ToHaveURLAsync(new Regex(@"^https?://[^/]+/ai-constructor/?(\?.*)?$"));
        }

        [Test]
        public async Task MainPageTestAiConstructorUserDoseNotExists()
        {
            var mainPage = new MainPage(Page);
            await mainPage.GotoAsync();
            var loginPage = await mainPage.ClickAiConstructorAsync();
            var regPage = await loginPage.ClickRegisterBtn();
            await regPage.RegAsync("user12345@example.com", "user12345", "Password12345@");
            await Expect(Page).ToHaveURLAsync(new Regex(@"^https?://[^/]+/ai-constructor/?(\?.*)?$"));
        }
    }
}
