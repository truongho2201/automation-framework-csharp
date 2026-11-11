# C# Selenium Test Automation

Maintainable .NET 8 UI automation with NUnit, Selenium WebDriver, Page Objects,
cross-browser settings, JSON logging, and screenshots attached to failed tests.

## Prerequisites

- .NET SDK 8.0.427 or a compatible 8.0 patch
- Google Chrome or Microsoft Edge for local execution
- Docker Desktop only when using the remote Chrome container

Selenium Manager resolves the matching local browser driver automatically.

## Run

Restore, build, and run all tests:

```powershell
dotnet restore CSharp.sln
dotnet test CSharp.sln
```

Run only browser-free tests:

```powershell
dotnet test CSharp.sln --filter "TestCategory!=UI"
```

Override settings with environment variables:

```powershell
$env:SELENIUM_TestSettings__Browser = "Edge"
$env:SELENIUM_TestSettings__TargetUrl = "https://example.com/"
dotnet test CSharp.sln
```

Supported browsers are `Chrome` and `Edge`. Copy
`tests/SeleniumTests/appsettings.example.json` to
`tests/SeleniumTests/appsettings.json` for local settings. Keep login
credentials out of source control; configure them with
`SELENIUM_TestSettings__Username` and `SELENIUM_TestSettings__Password`
environment variables. NUnit parameters named `Browser` and `TargetUrl` take
precedence over JSON and environment configuration.

## Remote Chrome

Start Selenium Grid and point the suite at it:

```powershell
docker compose up -d selenium-chrome
$env:SELENIUM_TestSettings__RemoteWebDriverUrl = "http://localhost:4444/wd/hub"
dotnet test CSharp.sln
docker compose down
```

## Failure Artifacts

Failed UI tests capture a timestamped PNG under the configured
`ScreenshotDirectory` and attach it to the NUnit result. Console logs use the
built-in JSON formatter for machine-readable output.

## Allure report

Reqnroll scenarios are integrated with Allure through `Allure.Reqnroll`. Test
results are written to the root `allure-results/` directory, and failed
scenario screenshots are attached to the corresponding Allure result.

Install the Allure command-line tool once if it is not already available:

```powershell
npm install -g allure-commandline
```

Allure CLI also requires Java 8 or newer. The repository script automatically
uses a portable JRE under `.tools/` when one is present; `.tools/` is local-only
and is not committed.

Run the UI scenarios and generate the report:

```powershell
dotnet test CSharp.sln --filter "TestCategory=UI"
allure generate allure-results --clean -o allure-report
allure open allure-report
```

Or use the repository script to run tests, record environment information, and
generate the report in one command:

```powershell
.\scripts\Generate-AllureReport.ps1 -Filter "TestCategory=UI" -Open
```

Generate a report from existing results without rerunning tests:

```powershell
.\scripts\Generate-AllureReport.ps1 -SkipTests -Open
```

For a one-command local preview after tests have produced results:

```powershell
allure serve allure-results
```

Allure tags such as `@allure.epic`, `@allure.suite`, `@allure.story`, and
`@critical` organize the report hierarchy and severity.

## Structure

- `Configuration/`: strongly typed settings and layered configuration loading
- `Drivers/`: local and remote WebDriver creation
- `Infrastructure/`: NUnit lifecycle, logging, and failure screenshots
- `Pages/`: Page Object Model abstractions
- `Bdd/`: Reqnroll feature files, step definitions, scenario state, and hooks
- `Tests/`: browser-free NUnit unit tests and executable UI samples

## BDD scenarios with Reqnroll

The project also supports Cucumber-style Gherkin scenarios through Reqnroll and
NUnit. Feature files live under `tests/SeleniumTests/Bdd/Features/` and reuse
the existing configuration, WebDriver factory, Page Objects, screenshots, and
logging infrastructure.

Run all tests, including BDD UI scenarios:

```powershell
dotnet test CSharp.sln
```

Run the Reqnroll feature only:

```powershell
dotnet test CSharp.sln --filter "FullyQualifiedName~ConfiguredWebsiteFeature"
```
