using MontjoyPlacesSdk;

var apiKey = Environment.GetEnvironmentVariable("MONTJOY_PLACES_API_KEY");
if (string.IsNullOrWhiteSpace(apiKey))
{
    throw new InvalidOperationException("Set MONTJOY_PLACES_API_KEY before running the sample.");
}

using var client = new MontjoyPlaces(apiKey);

var whoAmI = await client.WhoAmIAsync();
Console.WriteLine($"whoami: tenant={whoAmI.TenantId}, key={whoAmI.KeyName}");

var groups = await client.ListGroupsAsync(new ListGroupsRequest(Limit: 5));
Console.WriteLine("groups: " + string.Join(", ", groups.Rows.Select(group => group.Name)));

var search = await client.SearchPlacesAsync(new SearchPlacesRequest("coffee near Boston MA") { Limit = 3 });
Console.WriteLine($"search results count: {search.Count}");
Console.WriteLine(search.Rows);
