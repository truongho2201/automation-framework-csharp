# Integration Plan

## Worker

- Project: `tests/SeleniumTests/SeleniumTests.csproj`
- Solution: `CSharp.sln`
- Restore: `dotnet restore CSharp.sln`
- Build: `dotnet build CSharp.sln`
- Run all tests: `dotnet test CSharp.sln`
- Run browser-free tests: `dotnet test CSharp.sln --filter "TestCategory!=UI"`
- Health endpoint: Not applicable; this is an NUnit Selenium worker, not a server.
- Required local dependency: Chrome or Edge. Selenium Manager resolves drivers.
- Optional remote dependency: `docker compose up -d selenium-chrome`, then set `SELENIUM_TestSettings__RemoteWebDriverUrl=http://localhost:4444/wd/hub`.

## Frontend

- None. No API seam, mock client, mock datasets, or preview-state files exist.

## Browser Flow Inventory

- UI navigation: `Configured target URL` from `TestSettings:TargetUrl`
- Default target: `https://example.com/`
- Validation: visible HTML body with non-empty content and non-empty page title
- Browser selection: `TestSettings:Browser` (`Chrome` or `Edge`)
- Failure artifact: PNG under `TestSettings:ScreenshotDirectory`, attached to NUnit result

## Database

- None.
- Migration tool/directory/connection variables: Not applicable.
- Create NO migrations and NO seed data.

## Shared Types

- Strongly typed configuration: `tests/SeleniumTests/Configuration/TestSettings.cs`
- No shared package or typed-client import alias is required.

## Services

- Essential: local Selenium WebDriver execution and configured target URL.
- Enhancement: remote Selenium Chrome container from `docker-compose.yml`.

## Integration Checklist

- [x] Restore completed successfully; build completed with 0 warnings and 0 errors.
- [x] Browser-free test run passed 6/6 without launching a browser.
- [x] Complete suite passed 7/7, including the headless Chrome UI flow against `https://example.com/`.
- [x] JSON console logging was emitted during browser creation: `Creating Chrome WebDriver. Headless=True Remote=False`.
- [ ] Deliberately failing UI assertion screenshot attachment was not rerun because the existing scaffolded implementation was left unchanged.
- [x] No frontend, API, database, migrations, or seed data were added; these are not applicable to this Selenium worker.

## Integration Results

- Database migrations: Not applicable; the project has no database or persistence layer.
- Backend smoke test: Not applicable; this is an NUnit Selenium worker with no server or API endpoints.
- Frontend live-data wiring: Not applicable; no frontend, API seam, or mock data layer exists.
- End-to-end verification: Passed through the complete Selenium flow. Chrome navigated to the configured target URL and verified a non-empty title and visible body content.
- Additional checks: `dotnet format CSharp.sln --verify-no-changes` and `docker compose -f docker-compose.yml config --quiet` completed successfully.