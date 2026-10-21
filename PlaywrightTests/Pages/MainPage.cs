using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlaywrightTests.Pages
{
    public class MainPage
    {
        private readonly IPage _page;
        private readonly ILocator _cheapBtn;
        private readonly ILocator _courseCard;
        private readonly ILocator _aiConstructorBtn;

        public MainPage(IPage page)
        {
            _page = page;
            _cheapBtn = page.GetByTestId("cheap_btn_test");
            _courseCard = page.GetByTestId("first_course_card_test");
            _aiConstructorBtn = page.GetByTestId("ai-constructor-btn");
        }

        public async Task GotoAsync()
        {
            await _page.GotoAsync("https://localhost:7280");
        }

        public async Task<string?> GetCourseCardHrefAsync()
        {
            await _courseCard.WaitForAsync(new() { State = WaitForSelectorState.Attached });
            return await _courseCard.GetAttributeAsync("href");
        }

        public async Task<string> ClickCourseCardAsync()
        {
            var tcs = new TaskCompletionSource<string>();

            await _cheapBtn.ClickAsync();

            EventHandler<IPage> handler = null!;
            handler = (sender, popupPage) =>
            {
                tcs.TrySetResult(popupPage.Url);
                _page.Popup -= handler;
            };
            _page.Popup += handler;

            await _courseCard.ClickAsync();
            var completedTask = await Task.WhenAny(tcs.Task, Task.Delay(5000));

            if (completedTask == tcs.Task)
            {
                return await tcs.Task;
            }
            else
            {
                _page.Popup -= handler;
                throw new TimeoutException("Новая вкладка не открылась в течение 5 секунд.");
            }
        }

        public async Task<LoginPage> ClickAiConstructorAsync()
        {
            await _aiConstructorBtn.ClickAsync();
            await _page.WaitForURLAsync(new Regex(".*/authorization.*"));
            return new LoginPage(_page);
        }

    }
}
