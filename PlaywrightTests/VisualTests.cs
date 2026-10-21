using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Playwright;
using PlaywrightTests.Pages;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using static System.Net.Mime.MediaTypeNames;

namespace PlaywrightTests;

[TestFixture]
public class VisualTests : PageTest
{
    private const string BaselineDir = "VisualBaselines";

    private const string ActualDir = "VisualActual";

    [Test]
    public async Task MainPageVisualTest()
    {
        var mainPage = new MainPage(Page);
        await mainPage.GotoAsync();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await CompareScreenshotAsync("main-page.png");
    }

    private async Task CompareScreenshotAsync(string fileName)
    {
        Directory.CreateDirectory(BaselineDir);
        Directory.CreateDirectory(ActualDir);

        var actualPath = Path.Combine(ActualDir, $"actual-{fileName}");
        var baselinePath = Path.Combine(BaselineDir, fileName);
        var diffPath = Path.Combine(ActualDir, $"diff-{fileName}");

        await Page.ScreenshotAsync(new PageScreenshotOptions
        {
            Path = actualPath,
            FullPage = true,
            Animations = ScreenshotAnimations.Disabled
        });

        if (!File.Exists(baselinePath))
        {
            Assert.Fail($"Эталон не найден: {baselinePath}\n" +
                        $"Скопируйте файл {actualPath} в папку {BaselineDir} и закоммитьте его.");
        }

        using var actualImage = SixLabors.ImageSharp.Image.Load<Rgba32>(actualPath);
        using var baselineImage = SixLabors.ImageSharp.Image.Load<Rgba32>(baselinePath);

        if (actualImage.Width != baselineImage.Width || actualImage.Height != baselineImage.Height)
        {
            Assert.Fail($"Размеры скриншотов не совпадают!\n" +
                        $"Эталон: {baselineImage.Width}x{baselineImage.Height}\n" +
                        $"Текущий: {actualImage.Width}x{actualImage.Height}\n" +
                        $"Сравните {baselinePath} и {actualPath}");
        }

        bool isEqual = true;
        using var diffImage = baselineImage.Clone();

        for (int y = 0; y < baselineImage.Height; y++)
        {
            for (int x = 0; x < baselineImage.Width; x++)
            {
                var expectedPixel = baselineImage[x, y];
                var actualPixel = actualImage[x, y];

                if (expectedPixel != actualPixel)
                {
                    isEqual = false;
                    diffImage[x, y] = new Rgba32(255, 0, 0, 255);
                }
            }
        }

        if (!isEqual)
        {
            diffImage.Save(diffPath);
            Assert.Fail($"Визуальные различия на странице ({fileName})!\n" +
                        $"Эталон: {baselinePath}\n" +
                        $"Текущий: {actualPath}\n" +
                        $"Карта различий (красные пиксели): {diffPath}");
        }

        File.Delete(actualPath);
    }
}
