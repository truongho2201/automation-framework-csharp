using OpenQA.Selenium;

namespace SeleniumTests.Infrastructure;

public static class ScreenshotCapture
{
    public static string Capture(IWebDriver driver, string directory, string testName)
    {
        if (driver is not ITakesScreenshot screenshotDriver)
        {
            throw new NotSupportedException("The active WebDriver does not support screenshots.");
        }

        Directory.CreateDirectory(directory);
        var safeTestName = string.Concat(testName.Select(character =>
            Path.GetInvalidFileNameChars().Contains(character) ? '_' : character));
        var fileName = $"{safeTestName}-{DateTime.UtcNow:yyyyMMdd-HHmmssfff}.png";
        var path = Path.GetFullPath(Path.Combine(directory, fileName));

        screenshotDriver.GetScreenshot().SaveAsFile(path);
        return path;
    }
}
