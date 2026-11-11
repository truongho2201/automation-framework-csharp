using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SeleniumTests.Infrastructure;

public sealed class DemoQaApiClient : IDisposable
{
    private readonly HttpClient _httpClient = new();

    public async Task ClearBookCollectionAsync(
        string baseUrl,
        string username,
        string password,
        CancellationToken cancellationToken = default)
    {
        var apiBaseUrl = new Uri(baseUrl);
        var loginUri = new Uri(apiBaseUrl, "Account/v1/Login");
        using var loginResponse = await _httpClient.PostAsJsonAsync(
            loginUri,
            new LoginRequest(username, password),
            cancellationToken);
        await EnsureSuccess(loginResponse, "DemoQA API login", cancellationToken);

        var login = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>(cancellationToken)
            ?? throw new InvalidOperationException("DemoQA login response was empty.");

        using var deleteRequest = new HttpRequestMessage(
            HttpMethod.Delete,
            new Uri(apiBaseUrl, $"BookStore/v1/Books?UserId={Uri.EscapeDataString(login.UserId)}"));
        deleteRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", login.Token);

        using var deleteResponse = await _httpClient.SendAsync(deleteRequest, cancellationToken);
        await EnsureSuccess(deleteResponse, "DemoQA collection cleanup", cancellationToken);
    }

    private static async Task EnsureSuccess(
        HttpResponseMessage response,
        string operation,
        CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        throw new InvalidOperationException($"{operation} failed ({(int)response.StatusCode}): {body}");
    }

    public void Dispose() => _httpClient.Dispose();

    private sealed record LoginRequest(
        [property: JsonPropertyName("userName")] string Username,
        string Password);

    private sealed record LoginResponse(
        [property: JsonPropertyName("userId")] string UserId,
        string Token);
}
