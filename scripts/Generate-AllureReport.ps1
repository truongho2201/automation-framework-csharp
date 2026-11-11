[CmdletBinding()]
param(
    [string]$Filter = "TestCategory=UI",
    [switch]$SkipTests,
    [switch]$Open
)

$ErrorActionPreference = "Stop"
$projectRoot = Split-Path -Parent $PSScriptRoot
$solutionPath = Join-Path $projectRoot "CSharp.sln"
$resultsPath = Join-Path $projectRoot "allure-results"
$reportPath = Join-Path $projectRoot "allure-report"
$localJre = Get-ChildItem (Join-Path $projectRoot ".tools") -Directory -ErrorAction SilentlyContinue |
    Where-Object { Test-Path (Join-Path $_.FullName "bin\java.exe") } |
    Select-Object -First 1

if ($localJre) {
    $env:JAVA_HOME = $localJre.FullName
    $env:Path = "$(Join-Path $localJre.FullName 'bin');$env:Path"
}

if (-not (Get-Command allure -ErrorAction SilentlyContinue)) {
    throw "Allure CLI was not found. Install it with: npm install -g allure-commandline"
}

if (-not $env:JAVA_HOME -or -not (Test-Path (Join-Path $env:JAVA_HOME "bin\java.exe"))) {
    throw "Java 8+ was not found. Install a JRE or place a portable JRE under .tools."
}

$testExitCode = 0
if (-not $SkipTests) {
    dotnet test $solutionPath --filter $Filter
    $testExitCode = $LASTEXITCODE
}

if (-not (Test-Path -LiteralPath $resultsPath)) {
    throw "Allure results directory was not found: $resultsPath"
}

$browser = if ([string]::IsNullOrWhiteSpace($env:SELENIUM_TestSettings__Browser)) {
    "Configured in appsettings.json"
} else {
    $env:SELENIUM_TestSettings__Browser
}

$environmentPath = Join-Path $resultsPath "environment.properties"
@(
    "OS=$([System.Environment]::OSVersion.VersionString)"
    ".NET=$((& dotnet --version).Trim())"
    "Browser=$browser"
    "GeneratedAt=$((Get-Date).ToUniversalTime().ToString("o"))"
) | Set-Content -LiteralPath $environmentPath -Encoding utf8

allure generate $resultsPath --clean -o $reportPath
if ($LASTEXITCODE -ne 0) {
    throw "Allure report generation failed."
}

Write-Host "Allure report generated at: $reportPath"

if ($Open) {
    allure open $reportPath
}

exit $testExitCode
