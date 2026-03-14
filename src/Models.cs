using System.Text.Json;
using System.Text.Json.Serialization;

namespace MontjoyPlacesSdk;

public sealed record ErrorResponse([property: JsonPropertyName("error")] string Error);

public sealed record WhoAmIResponse(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("apiKeyId")] string ApiKeyId,
    [property: JsonPropertyName("tenantId")] string TenantId,
    [property: JsonPropertyName("appId")] string AppId,
    [property: JsonPropertyName("keyName")] string KeyName,
    [property: JsonPropertyName("prefix")] string Prefix);

public sealed record Group(
    [property: JsonPropertyName("group_id")] string GroupId,
    [property: JsonPropertyName("tenant_id")] string TenantId,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("created_at")] DateTimeOffset CreatedAt);

public sealed record GroupCreateRequest([property: JsonPropertyName("name")] string Name);

public sealed record GroupUpdateRequest([property: JsonPropertyName("name")] string Name);

public sealed record GroupsListResponse(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("rows")] IReadOnlyList<Group> Rows);

public sealed record GroupSingleResponse(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("row")] Group Row);

public sealed record GroupDeleteResponse(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("deleted")] bool Deleted);

public sealed record DeleteResponse(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("deleted")] bool? Deleted);

public sealed record CustomPlace(
    [property: JsonPropertyName("custom_place_id")] string CustomPlaceId,
    [property: JsonPropertyName("tenant_id")] string TenantId,
    [property: JsonPropertyName("app_id")] string? AppId,
    [property: JsonPropertyName("group_id")] string? GroupId,
    [property: JsonPropertyName("owner_user_id")] string? OwnerUserId,
    [property: JsonPropertyName("source")] string Source,
    [property: JsonPropertyName("fsq_place_id")] string? FsqPlaceId,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("latitude")] double Latitude,
    [property: JsonPropertyName("longitude")] double Longitude,
    [property: JsonPropertyName("address")] string? Address,
    [property: JsonPropertyName("locality")] string? Locality,
    [property: JsonPropertyName("region")] string? Region,
    [property: JsonPropertyName("postcode")] string? Postcode,
    [property: JsonPropertyName("country")] string? Country,
    [property: JsonPropertyName("website")] string? Website,
    [property: JsonPropertyName("tel")] string? Tel,
    [property: JsonPropertyName("email")] string? Email,
    [property: JsonPropertyName("tags")] JsonElement? Tags,
    [property: JsonPropertyName("meta")] JsonElement? Meta,
    [property: JsonPropertyName("created_at")] DateTimeOffset CreatedAt,
    [property: JsonPropertyName("updated_at")] DateTimeOffset UpdatedAt,
    [property: JsonPropertyName("dist_m")] double? DistanceMeters);

public sealed record CustomPlacesListResponse(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("rows")] IReadOnlyList<CustomPlace> Rows,
    [property: JsonPropertyName("nextCursor")] string? NextCursor);

public sealed record CustomPlaceSingleResponse(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("row")] CustomPlace Row);

public sealed record CustomPlaceCreateRequest(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("latitude")] double Latitude,
    [property: JsonPropertyName("longitude")] double Longitude)
{
    [JsonPropertyName("groupId")]
    public string? GroupId { get; init; }

    [JsonPropertyName("source")]
    public string? Source { get; init; }

    [JsonPropertyName("ownerUserId")]
    public string? OwnerUserId { get; init; }

    [JsonPropertyName("fsqPlaceId")]
    public string? FsqPlaceId { get; init; }

    [JsonPropertyName("address")]
    public string? Address { get; init; }

    [JsonPropertyName("locality")]
    public string? Locality { get; init; }

    [JsonPropertyName("region")]
    public string? Region { get; init; }

    [JsonPropertyName("postcode")]
    public string? Postcode { get; init; }

    [JsonPropertyName("country")]
    public string? Country { get; init; }

    [JsonPropertyName("website")]
    public string? Website { get; init; }

    [JsonPropertyName("tel")]
    public string? Tel { get; init; }

    [JsonPropertyName("email")]
    public string? Email { get; init; }

    [JsonPropertyName("tags")]
    public object? Tags { get; init; }

    [JsonPropertyName("meta")]
    public object? Meta { get; init; }
}

public sealed record CustomPlaceUpdateRequest
{
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("latitude")]
    public double? Latitude { get; init; }

    [JsonPropertyName("longitude")]
    public double? Longitude { get; init; }

    [JsonPropertyName("address")]
    public string? Address { get; init; }

    [JsonPropertyName("locality")]
    public string? Locality { get; init; }

    [JsonPropertyName("region")]
    public string? Region { get; init; }

    [JsonPropertyName("postcode")]
    public string? Postcode { get; init; }

    [JsonPropertyName("country")]
    public string? Country { get; init; }

    [JsonPropertyName("website")]
    public string? Website { get; init; }

    [JsonPropertyName("tel")]
    public string? Tel { get; init; }

    [JsonPropertyName("email")]
    public string? Email { get; init; }

    [JsonPropertyName("tags")]
    public object? Tags { get; init; }

    [JsonPropertyName("meta")]
    public object? Meta { get; init; }
}

public sealed record CustomPlaceHideRequest([property: JsonPropertyName("hidden")] bool Hidden = true);

public sealed record SearchResolvedCenter(
    [property: JsonPropertyName("lat")] double Lat,
    [property: JsonPropertyName("lon")] double Lon,
    [property: JsonPropertyName("source")] string Source,
    [property: JsonPropertyName("kind")] string Kind,
    [property: JsonPropertyName("label")] string Label);

public sealed record SearchResolved(
    [property: JsonPropertyName("mode")] string Mode,
    [property: JsonPropertyName("reason")] string? Reason,
    [property: JsonPropertyName("prefix")] string? Prefix,
    [property: JsonPropertyName("categoryName")] string? CategoryName,
    [property: JsonPropertyName("groupId")] string? GroupId,
    [property: JsonPropertyName("customOnly")] bool? CustomOnly,
    [property: JsonPropertyName("localityText")] string? LocalityText,
    [property: JsonPropertyName("center")] SearchResolvedCenter? Center);

public sealed record SearchResponse(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("mode")] string Mode,
    [property: JsonPropertyName("q")] string Query,
    [property: JsonPropertyName("resolved")] SearchResolved Resolved,
    [property: JsonPropertyName("count")] int Count,
    [property: JsonPropertyName("rows")] JsonElement Rows);

public sealed record UsCity(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("city")] string City,
    [property: JsonPropertyName("state_id")] string StateId,
    [property: JsonPropertyName("state_name")] string StateName,
    [property: JsonPropertyName("zipcode")] string Zipcode,
    [property: JsonPropertyName("lat")] double Lat,
    [property: JsonPropertyName("lon")] double Lon,
    [property: JsonPropertyName("dist_m")] double? DistanceMeters);

public sealed record UsCityListResponse(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("count")] int Count,
    [property: JsonPropertyName("rows")] IReadOnlyList<UsCity> Rows);

public sealed record UsCitySearchResponse(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("q")] string Query,
    [property: JsonPropertyName("state")] string? State,
    [property: JsonPropertyName("count")] int Count,
    [property: JsonPropertyName("rows")] IReadOnlyList<UsCity> Rows);

public sealed record UsZipLookupResponse(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("zipcode")] string Zipcode,
    [property: JsonPropertyName("count")] int Count,
    [property: JsonPropertyName("rows")] IReadOnlyList<UsCity> Rows);

public sealed record CategoryHierarchyNode(
    [property: JsonPropertyName("level")] int Level,
    [property: JsonPropertyName("category_id")] string? CategoryId,
    [property: JsonPropertyName("category_name")] string? CategoryName);

public sealed record CategoryLookupRow(
    [property: JsonPropertyName("category_id")] string CategoryId,
    [property: JsonPropertyName("category_name")] string? CategoryName,
    [property: JsonPropertyName("category_label")] string? CategoryLabel,
    [property: JsonPropertyName("category_level")] int? CategoryLevel,
    [property: JsonPropertyName("hierarchy")] IReadOnlyList<CategoryHierarchyNode> Hierarchy);

public sealed record CategorySearchResponse(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("q")] string? Query,
    [property: JsonPropertyName("level")] int? Level,
    [property: JsonPropertyName("parentId")] string? ParentId,
    [property: JsonPropertyName("count")] int Count,
    [property: JsonPropertyName("rows")] IReadOnlyList<CategoryLookupRow> Rows);

public sealed record CategoryResponse(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("row")] CategoryLookupRow Row);

public sealed record CategoryChildrenResponse(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("parent")] CategoryLookupRow Parent,
    [property: JsonPropertyName("count")] int Count,
    [property: JsonPropertyName("rows")] IReadOnlyList<CategoryLookupRow> Rows);

public sealed record OverrideRequest
{
    [JsonPropertyName("groupId")]
    public string? GroupId { get; init; }

    [JsonPropertyName("hide")]
    public bool? Hide { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("latitude")]
    public double? Latitude { get; init; }

    [JsonPropertyName("longitude")]
    public double? Longitude { get; init; }

    [JsonPropertyName("address")]
    public string? Address { get; init; }

    [JsonPropertyName("locality")]
    public string? Locality { get; init; }

    [JsonPropertyName("region")]
    public string? Region { get; init; }

    [JsonPropertyName("postcode")]
    public string? Postcode { get; init; }

    [JsonPropertyName("country")]
    public string? Country { get; init; }

    [JsonPropertyName("website")]
    public string? Website { get; init; }

    [JsonPropertyName("tel")]
    public string? Tel { get; init; }

    [JsonPropertyName("email")]
    public string? Email { get; init; }

    [JsonPropertyName("tags")]
    public object? Tags { get; init; }

    [JsonPropertyName("meta")]
    public object? Meta { get; init; }
}

public sealed record OverrideResponse(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("action")] string Action,
    [property: JsonPropertyName("row")] CustomPlace Row);

public sealed record ListGroupsRequest([property: JsonPropertyName("limit")] int? Limit = null);

public sealed record ListCustomPlacesRequest
{
    [JsonPropertyName("groupId")]
    public string? GroupId { get; init; }

    [JsonPropertyName("limit")]
    public int? Limit { get; init; }

    [JsonPropertyName("cursor")]
    public string? Cursor { get; init; }

    [JsonPropertyName("includeHidden")]
    public bool? IncludeHidden { get; init; }
}

public sealed record NearestUsCitiesRequest(
    [property: JsonPropertyName("lat")] double Lat,
    [property: JsonPropertyName("lon")] double Lon)
{
    [JsonPropertyName("limit")]
    public int? Limit { get; init; }
}

public sealed record SearchUsCitiesRequest([property: JsonPropertyName("q")] string Query)
{
    [JsonPropertyName("state")]
    public string? State { get; init; }

    [JsonPropertyName("limit")]
    public int? Limit { get; init; }
}

public sealed record SearchCategoriesRequest
{
    [JsonPropertyName("q")]
    public string? Query { get; init; }

    [JsonPropertyName("level")]
    public int? Level { get; init; }

    [JsonPropertyName("parentId")]
    public string? ParentId { get; init; }

    [JsonPropertyName("limit")]
    public int? Limit { get; init; }
}

public sealed record CategoryChildrenRequest([property: JsonPropertyName("limit")] int? Limit = null);

public sealed record SearchPlacesRequest([property: JsonPropertyName("q")] string Query)
{
    [JsonPropertyName("lat")]
    public double? Lat { get; init; }

    [JsonPropertyName("lon")]
    public double? Lon { get; init; }

    [JsonPropertyName("radiusMeters")]
    public double? RadiusMeters { get; init; }

    [JsonPropertyName("limit")]
    public int? Limit { get; init; }

    [JsonPropertyName("excludeCategoryMatch")]
    public bool? ExcludeCategoryMatch { get; init; }

    [JsonPropertyName("forceTypeahead")]
    public bool? ForceTypeahead { get; init; }

    [JsonPropertyName("customOnly")]
    public bool? CustomOnly { get; init; }

    [JsonPropertyName("onlyCustom")]
    public bool? OnlyCustom { get; init; }

    [JsonPropertyName("groupId")]
    public string? GroupId { get; init; }
}
