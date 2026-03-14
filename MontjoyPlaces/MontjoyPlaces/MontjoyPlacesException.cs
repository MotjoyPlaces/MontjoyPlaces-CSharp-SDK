namespace MontjoyPlacesSdk;

public sealed class MontjoyPlacesException : Exception
{
    public MontjoyPlacesException(string message, System.Net.HttpStatusCode statusCode, string? responseBody = null)
        : base(message)
    {
        StatusCode = statusCode;
        ResponseBody = responseBody;
    }

    public System.Net.HttpStatusCode StatusCode { get; }

    public string? ResponseBody { get; }
}
