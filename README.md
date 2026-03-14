# MontjoyPlaces C# SDK

Official C# SDK for the Montjoy Places API.

## Overview

`MontjoyPlaces` is a lightweight .NET client for working with the Montjoy Places API. It wraps the HTTP API in a small, async-first SDK and includes models for common request and response payloads.

The SDK currently supports:

- API key validation with `WhoAmIAsync`
- Group management
- Custom place create, read, update, hide, list, and delete operations
- Place override operations
- US city and ZIP lookup endpoints
- Category search and lookup endpoints
- Search across places

## Requirements

- .NET 10.0 SDK or later
- A Montjoy Places API key

## Installation

```bash
dotnet add package MontjoyPlaces
```

## Authentication

Create a client with your API key:

```csharp
using MontjoyPlacesSdk;

using var client = new MontjoyPlaces("your-api-key");
```

By default, the SDK sends requests to `https://api.montjoyplaces.com`.

You can also supply:

- `baseUrl` to point at a different environment
- `HttpClient` if you want to manage connection lifetime yourself

```csharp
using System.Net.Http;
using MontjoyPlacesSdk;

using var httpClient = new HttpClient();
using var client = new MontjoyPlaces(
    apiKey: "your-api-key",
    baseUrl: "https://api.montjoyplaces.com",
    httpClient: httpClient);
```

## Quick Start

```csharp
using MontjoyPlacesSdk;

var apiKey = Environment.GetEnvironmentVariable("MONTJOY_PLACES_API_KEY")
    ?? throw new InvalidOperationException("Set MONTJOY_PLACES_API_KEY.");

using var client = new MontjoyPlaces(apiKey);

var whoAmI = await client.WhoAmIAsync();
Console.WriteLine($"tenant={whoAmI.TenantId}, key={whoAmI.KeyName}");

var groups = await client.ListGroupsAsync(new ListGroupsRequest(Limit: 5));
Console.WriteLine($"group count returned: {groups.Rows.Count}");

var search = await client.SearchPlacesAsync(new SearchPlacesRequest("coffee near Boston MA")
{
    Limit = 3
});

Console.WriteLine($"search results: {search.Count}");
Console.WriteLine(search.Rows);
```

## Common Workflows

### Groups

```csharp
var created = await client.CreateGroupAsync(new GroupCreateRequest("Favorites"));
var updated = await client.UpdateGroupAsync(created.Row.GroupId, new GroupUpdateRequest("Saved Places"));
var listed = await client.ListGroupsAsync(new ListGroupsRequest(Limit: 10));
await client.DeleteGroupAsync(created.Row.GroupId);
```

### Custom Places

```csharp
var place = await client.CreateCustomPlaceAsync(new CustomPlaceCreateRequest(
    Name: "Coffee Stop",
    Latitude: 42.3601,
    Longitude: -71.0589)
{
    Address = "1 Beacon St",
    Locality = "Boston",
    Region = "MA",
    Postcode = "02108",
    Country = "US",
    Website = "https://example.com",
    Tags = new[] { "coffee", "favorite" },
    Meta = new Dictionary<string, object?> { ["source"] = "README" }
});

var fetched = await client.GetCustomPlaceAsync(place.Row.CustomPlaceId);

var updated = await client.UpdateCustomPlaceAsync(place.Row.CustomPlaceId, new CustomPlaceUpdateRequest
{
    Name = "Coffee Stop Updated",
    Website = "https://example.com/updated"
});

await client.HideCustomPlaceAsync(place.Row.CustomPlaceId, new CustomPlaceHideRequest(true));
await client.HideCustomPlaceAsync(place.Row.CustomPlaceId, new CustomPlaceHideRequest(false));
await client.DeleteCustomPlaceAsync(place.Row.CustomPlaceId);
```

### Place Search

```csharp
var response = await client.SearchPlacesAsync(new SearchPlacesRequest("pizza")
{
    Lat = 42.3601,
    Lon = -71.0589,
    RadiusMeters = 2000,
    Limit = 10
});

Console.WriteLine(response.Rows);
```

### Lookup Endpoints

```csharp
var nearestCities = await client.LookupNearestUsCitiesAsync(new NearestUsCitiesRequest(42.3601, -71.0589)
{
    Limit = 5
});

var citySearch = await client.SearchUsCitiesAsync(new SearchUsCitiesRequest("Boston")
{
    State = "MA",
    Limit = 10
});

var zipLookup = await client.LookupUsZipcodeAsync("02108");

var categories = await client.SearchCategoriesAsync(new SearchCategoriesRequest
{
    Query = "coffee",
    Limit = 10
});
```

## Error Handling

API failures throw `MontjoyPlacesException`.

```csharp
try
{
    var whoAmI = await client.WhoAmIAsync();
}
catch (MontjoyPlacesException ex)
{
    Console.WriteLine($"Status: {(int)ex.StatusCode}");
    Console.WriteLine(ex.Message);
    Console.WriteLine(ex.ResponseBody);
}
```

## Notes

- All SDK methods are asynchronous and support `CancellationToken`.
- Request and response payloads use `System.Text.Json`.
- If you pass your own `HttpClient`, the SDK will not dispose it.
- `SearchResponse.Rows` is returned as `JsonElement` to preserve the raw search payload.

## Samples

Sample programs are included in [`samples/`](./samples):

- `BasicSample.cs` shows authentication, group listing, and search
- `IntegrationSample.cs` exercises create, update, hide, list, and cleanup flows for groups and custom places

Run a sample with an API key set in the environment:

```bash
MONTJOY_PLACES_API_KEY=your-api-key dotnet run --project samples/MontjoyPlaces.Sample.csproj
```

## Package Metadata

- Homepage: https://montjoyplaces.com
- Support: paul@montjoyapp.com
- License: MIT
