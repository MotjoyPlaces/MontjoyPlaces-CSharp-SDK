using MontjoyPlacesSdk;
using System.Text.Json;

var apiKey = Environment.GetEnvironmentVariable("MONTJOY_PLACES_API_KEY");
if (string.IsNullOrWhiteSpace(apiKey))
{
    throw new InvalidOperationException("Set MONTJOY_PLACES_API_KEY before running the sample.");
}

using var client = new MontjoyPlaces(apiKey);

var plans = await client.ListBillingPlansAsync();
Console.WriteLine("billing plans: " + string.Join(", ", plans.Plans.Select(plan => plan.Code)));

var whoAmI = await client.WhoAmIAsync();
Console.WriteLine($"whoami: tenant={whoAmI.TenantId}, key={whoAmI.KeyName}");

var groups = await client.ListGroupsAsync(new ListGroupsRequest(Limit: 5));
Console.WriteLine("groups: " + string.Join(", ", groups.Rows.Select(group => group.Name)));

var search = await client.SearchPlacesAsync(new SearchPlacesRequest("coffee near Boston MA") { Limit = 3 });
Console.WriteLine($"search results count: {search.Count}");
Console.WriteLine(search.Rows);

var firstPlaceId = search.Rows.ValueKind == JsonValueKind.Array
    ? search.Rows.EnumerateArray()
        .Select(row => row.TryGetProperty("fsq_place_id", out var placeId) ? placeId.GetString() : null)
        .FirstOrDefault(placeId => !string.IsNullOrWhiteSpace(placeId))
    : null;

if (!string.IsNullOrWhiteSpace(firstPlaceId))
{
    var place = await client.GetPlaceAsync(firstPlaceId);
    Console.WriteLine($"direct place lookup: {place.Row?.Name} ({place.Row?.PlaceSource})");
}
