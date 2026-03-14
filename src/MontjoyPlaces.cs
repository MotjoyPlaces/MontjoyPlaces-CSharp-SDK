using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MontjoyPlacesSdk;

public sealed class MontjoyPlaces : IDisposable
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _httpClient;
    private readonly bool _disposeHttpClient;

    public MontjoyPlaces(string apiKey, string? baseUrl = null, HttpClient? httpClient = null)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new ArgumentException("API key is required.", nameof(apiKey));
        }

        _disposeHttpClient = httpClient is null;
        _httpClient = httpClient ?? new HttpClient();
        _httpClient.BaseAddress = new Uri((baseUrl ?? "https://api.montjoyplaces.com").TrimEnd('/') + "/");
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _httpClient.DefaultRequestHeaders.Remove("X-API-Key");
        _httpClient.DefaultRequestHeaders.Add("X-API-Key", apiKey);
    }

    public Task<WhoAmIResponse> WhoAmIAsync(CancellationToken cancellationToken = default) =>
        SendAsync<WhoAmIResponse>(HttpMethod.Get, "v1/whoami", cancellationToken: cancellationToken);

    public Task<GroupsListResponse> ListGroupsAsync(ListGroupsRequest? request = null, CancellationToken cancellationToken = default) =>
        SendAsync<GroupsListResponse>(HttpMethod.Get, "v1/groups", query: ToQuery(request), cancellationToken: cancellationToken);

    public Task<GroupSingleResponse> CreateGroupAsync(GroupCreateRequest request, CancellationToken cancellationToken = default) =>
        SendAsync<GroupSingleResponse>(HttpMethod.Post, "v1/groups", body: request, cancellationToken: cancellationToken);

    public Task<GroupSingleResponse> UpdateGroupAsync(string groupId, GroupUpdateRequest request, CancellationToken cancellationToken = default) =>
        SendAsync<GroupSingleResponse>(HttpMethod.Put, $"v1/groups/{Uri.EscapeDataString(groupId)}", body: request, cancellationToken: cancellationToken);

    public Task<GroupDeleteResponse> DeleteGroupAsync(string groupId, CancellationToken cancellationToken = default) =>
        SendAsync<GroupDeleteResponse>(HttpMethod.Delete, $"v1/groups/{Uri.EscapeDataString(groupId)}", cancellationToken: cancellationToken);

    public Task<CustomPlacesListResponse> ListCustomPlacesAsync(ListCustomPlacesRequest? request = null, CancellationToken cancellationToken = default) =>
        SendAsync<CustomPlacesListResponse>(HttpMethod.Get, "v1/custom-places", query: ToQuery(request), cancellationToken: cancellationToken);

    public Task<CustomPlaceSingleResponse> CreateCustomPlaceAsync(CustomPlaceCreateRequest request, CancellationToken cancellationToken = default) =>
        SendAsync<CustomPlaceSingleResponse>(HttpMethod.Post, "v1/custom-places", body: request, cancellationToken: cancellationToken);

    public Task<CustomPlaceSingleResponse> GetCustomPlaceAsync(string customPlaceId, CancellationToken cancellationToken = default) =>
        SendAsync<CustomPlaceSingleResponse>(HttpMethod.Get, $"v1/custom-places/{Uri.EscapeDataString(customPlaceId)}", cancellationToken: cancellationToken);

    public Task<CustomPlaceSingleResponse> UpdateCustomPlaceAsync(string customPlaceId, CustomPlaceUpdateRequest request, CancellationToken cancellationToken = default) =>
        SendAsync<CustomPlaceSingleResponse>(HttpMethod.Put, $"v1/custom-places/{Uri.EscapeDataString(customPlaceId)}", body: request, cancellationToken: cancellationToken);

    public Task<DeleteResponse> DeleteCustomPlaceAsync(string customPlaceId, CancellationToken cancellationToken = default) =>
        SendAsync<DeleteResponse>(HttpMethod.Delete, $"v1/custom-places/{Uri.EscapeDataString(customPlaceId)}", cancellationToken: cancellationToken);

    public Task<CustomPlaceSingleResponse> HideCustomPlaceAsync(string customPlaceId, CustomPlaceHideRequest request, CancellationToken cancellationToken = default) =>
        SendAsync<CustomPlaceSingleResponse>(HttpMethod.Post, $"v1/custom-places/{Uri.EscapeDataString(customPlaceId)}/hide", body: request, cancellationToken: cancellationToken);

    public Task<OverrideResponse> OverridePlaceAsync(string fsqPlaceId, OverrideRequest request, CancellationToken cancellationToken = default) =>
        SendAsync<OverrideResponse>(HttpMethod.Put, $"v1/places/{Uri.EscapeDataString(fsqPlaceId)}/override", body: request, cancellationToken: cancellationToken);

    public Task<UsCityListResponse> LookupNearestUsCitiesAsync(NearestUsCitiesRequest request, CancellationToken cancellationToken = default) =>
        SendAsync<UsCityListResponse>(HttpMethod.Get, "v1/lookup/us-cities/nearest", query: ToQuery(request), cancellationToken: cancellationToken);

    public Task<UsCitySearchResponse> SearchUsCitiesAsync(SearchUsCitiesRequest request, CancellationToken cancellationToken = default) =>
        SendAsync<UsCitySearchResponse>(HttpMethod.Get, "v1/lookup/us-cities/search", query: ToQuery(request), cancellationToken: cancellationToken);

    public Task<UsZipLookupResponse> LookupUsZipcodeAsync(string zipcode, CancellationToken cancellationToken = default) =>
        SendAsync<UsZipLookupResponse>(HttpMethod.Get, $"v1/lookup/us-cities/zip/{Uri.EscapeDataString(zipcode)}", cancellationToken: cancellationToken);

    public Task<CategorySearchResponse> SearchCategoriesAsync(SearchCategoriesRequest? request = null, CancellationToken cancellationToken = default) =>
        SendAsync<CategorySearchResponse>(HttpMethod.Get, "v1/lookup/categories/search", query: ToQuery(request), cancellationToken: cancellationToken);

    public Task<CategoryResponse> GetCategoryAsync(string categoryId, CancellationToken cancellationToken = default) =>
        SendAsync<CategoryResponse>(HttpMethod.Get, $"v1/lookup/categories/{Uri.EscapeDataString(categoryId)}", cancellationToken: cancellationToken);

    public Task<CategoryChildrenResponse> GetCategoryChildrenAsync(string categoryId, CategoryChildrenRequest? request = null, CancellationToken cancellationToken = default) =>
        SendAsync<CategoryChildrenResponse>(HttpMethod.Get, $"v1/lookup/categories/{Uri.EscapeDataString(categoryId)}/children", query: ToQuery(request), cancellationToken: cancellationToken);

    public Task<SearchResponse> SearchPlacesAsync(SearchPlacesRequest request, CancellationToken cancellationToken = default) =>
        SendAsync<SearchResponse>(HttpMethod.Get, "v1/search", query: ToQuery(request), cancellationToken: cancellationToken);

    private async Task<T> SendAsync<T>(HttpMethod method, string path, object? query = null, object? body = null, CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(method, BuildUri(path, query));
        if (body is not null)
        {
            request.Content = new StringContent(JsonSerializer.Serialize(body, JsonOptions), Encoding.UTF8, "application/json");
        }

        using var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        var content = response.Content is null ? null : await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw CreateException(response.StatusCode, content);
        }

        if (string.IsNullOrWhiteSpace(content))
        {
            return default!;
        }

        var value = JsonSerializer.Deserialize<T>(content, JsonOptions);
        return value ?? throw new MontjoyPlacesException("Response body was empty.", response.StatusCode, content);
    }

    private Uri BuildUri(string path, object? query)
    {
        var builder = new StringBuilder(path);
        var pairs = BuildQueryPairs(query);
        if (pairs.Count > 0)
        {
            builder.Append('?');
            builder.Append(string.Join("&", pairs.Select(pair => $"{Uri.EscapeDataString(pair.Key)}={Uri.EscapeDataString(pair.Value)}")));
        }

        return new Uri(builder.ToString(), UriKind.Relative);
    }

    private static List<KeyValuePair<string, string>> BuildQueryPairs(object? request)
    {
        if (request is null)
        {
            return [];
        }

        var pairs = new List<KeyValuePair<string, string>>();
        foreach (var property in request.GetType().GetProperties())
        {
            var value = property.GetValue(request);
            if (value is null)
            {
                continue;
            }

            var name = property.GetCustomAttributes(typeof(System.Text.Json.Serialization.JsonPropertyNameAttribute), false)
                .Cast<System.Text.Json.Serialization.JsonPropertyNameAttribute>()
                .FirstOrDefault()?.Name ?? property.Name;

            if (name == "includeHidden" && value is bool includeHidden)
            {
                pairs.Add(new KeyValuePair<string, string>(name, includeHidden ? "1" : "0"));
                continue;
            }

            pairs.Add(new KeyValuePair<string, string>(name, ConvertToString(value)));
        }

        return pairs;
    }

    private static string ConvertToString(object value) =>
        value switch
        {
            bool booleanValue => booleanValue ? "true" : "false",
            IFormattable formattable => formattable.ToString(null, CultureInfo.InvariantCulture),
            _ => value.ToString() ?? string.Empty
        };

    private static MontjoyPlacesException CreateException(System.Net.HttpStatusCode statusCode, string? content)
    {
        if (!string.IsNullOrWhiteSpace(content))
        {
            try
            {
                var error = JsonSerializer.Deserialize<ErrorResponse>(content, JsonOptions);
                if (!string.IsNullOrWhiteSpace(error?.Error))
                {
                    return new MontjoyPlacesException(error.Error, statusCode, content);
                }
            }
            catch (JsonException)
            {
            }
        }

        return new MontjoyPlacesException($"Request failed with status {(int)statusCode}.", statusCode, content);
    }

    private static object? ToQuery(object? request) => request;

    public void Dispose()
    {
        if (_disposeHttpClient)
        {
            _httpClient.Dispose();
        }
    }
}
