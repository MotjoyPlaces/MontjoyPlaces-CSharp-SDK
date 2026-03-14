using MontjoyPlacesSdk;

var apiKey = Environment.GetEnvironmentVariable("MONTJOY_PLACES_API_KEY");
if (string.IsNullOrWhiteSpace(apiKey))
{
    throw new InvalidOperationException("Set MONTJOY_PLACES_API_KEY before running the sample.");
}

using var client = new MontjoyPlaces(apiKey);
var suffix = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
var groupName = $"sdk-csharp-{suffix}";

string? groupId = null;
string? customPlaceId = null;

try
{
    var createdGroup = await client.CreateGroupAsync(new GroupCreateRequest(groupName));
    groupId = createdGroup.Row.GroupId;
    Console.WriteLine($"created group: {createdGroup.Row}");

    var createdPlace = await client.CreateCustomPlaceAsync(new CustomPlaceCreateRequest(
        Name: $"SDK CSharp Test Place {suffix}",
        Latitude: 42.3601,
        Longitude: -71.0589)
    {
        GroupId = groupId,
        Address = "1 Beacon St",
        Locality = "Boston",
        Region = "MA",
        Postcode = "02108",
        Country = "US",
        Website = "https://example.com/csharp",
        Tags = new[] { "sdk", "csharp" },
        Meta = new Dictionary<string, object?> { ["source"] = "integration-sample" }
    });
    customPlaceId = createdPlace.Row.CustomPlaceId;
    Console.WriteLine($"created custom place: {createdPlace.Row}");

    var fetchedPlace = await client.GetCustomPlaceAsync(customPlaceId);
    Console.WriteLine($"fetched custom place: {fetchedPlace.Row}");

    var updatedPlace = await client.UpdateCustomPlaceAsync(customPlaceId, new CustomPlaceUpdateRequest
    {
        Name = $"SDK CSharp Updated Place {suffix}",
        Website = "https://example.com/csharp-updated",
        Meta = new Dictionary<string, object?> { ["source"] = "integration-sample", ["updated"] = true }
    });
    Console.WriteLine($"updated custom place: {updatedPlace.Row}");

    var hiddenPlace = await client.HideCustomPlaceAsync(customPlaceId, new CustomPlaceHideRequest(true));
    Console.WriteLine($"hidden custom place: {hiddenPlace.Row}");

    var unhiddenPlace = await client.HideCustomPlaceAsync(customPlaceId, new CustomPlaceHideRequest(false));
    Console.WriteLine($"unhidden custom place: {unhiddenPlace.Row}");

    var listedPlaces = await client.ListCustomPlacesAsync(new ListCustomPlacesRequest
    {
        GroupId = groupId,
        Limit = 10,
        IncludeHidden = true
    });
    Console.WriteLine("group custom places: " + string.Join(", ", listedPlaces.Rows.Select(row => row.Name)));
}
finally
{
    if (!string.IsNullOrWhiteSpace(customPlaceId))
    {
        try
        {
            var deletedPlace = await client.DeleteCustomPlaceAsync(customPlaceId);
            Console.WriteLine($"deleted custom place: {deletedPlace}");
        }
        catch (Exception exception)
        {
            Console.WriteLine($"cleanup failed for custom place: {exception.Message}");
        }
    }

    if (!string.IsNullOrWhiteSpace(groupId))
    {
        try
        {
            var deletedGroup = await client.DeleteGroupAsync(groupId);
            Console.WriteLine($"deleted group: {deletedGroup}");
        }
        catch (Exception exception)
        {
            Console.WriteLine($"cleanup failed for group: {exception.Message}");
        }
    }
}
