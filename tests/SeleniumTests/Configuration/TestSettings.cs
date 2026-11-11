namespace SeleniumTests.Configuration;

public sealed record TestSettings
{
    public string Browser { get; init; } = "Chrome";

    public bool Headless { get; init; } = true;

    public string TargetUrl { get; init; } = "https://demoqa.com/";

    public string Username { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;

    public int ImplicitWaitSeconds { get; init; }

    public int ExplicitWaitSeconds { get; init; } = 10;

    public string ScreenshotDirectory { get; init; } = "artifacts/screenshots";

    public string? RemoteWebDriverUrl { get; init; }

    public void Validate()
    {
        if (Browser is not ("Chrome" or "Edge"))
        {
            throw new InvalidOperationException("TestSettings:Browser must be Chrome or Edge.");
        }

        if (!Uri.TryCreate(TargetUrl, UriKind.Absolute, out var targetUri)
            || (!string.Equals(targetUri.Scheme, Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase)
                && !string.Equals(targetUri.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException("TestSettings:TargetUrl must be an absolute HTTP or HTTPS URL.");
        }

        if (ImplicitWaitSeconds < 0 || ExplicitWaitSeconds <= 0)
        {
            throw new InvalidOperationException("Wait values must be non-negative and the explicit wait must be greater than zero.");
        }

        if (RemoteWebDriverUrl is not null
            && !Uri.TryCreate(RemoteWebDriverUrl, UriKind.Absolute, out _))
        {
            throw new InvalidOperationException("TestSettings:RemoteWebDriverUrl must be an absolute URL when provided.");
        }
    }
}
