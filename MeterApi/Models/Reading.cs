using System.Text.Json.Serialization;

namespace MeterApi.Models;

public class Reading
{
    public int Id { get; set; }

    [JsonPropertyName("readingDate")]
    public string ReadingDate { get; set; } = string.Empty;

    [JsonPropertyName("locality")]
    public string Locality { get; set; } = string.Empty;

    [JsonPropertyName("street")]
    public string? Street { get; set; }

    [JsonPropertyName("house")]
    public string House { get; set; } = string.Empty;

    [JsonPropertyName("apartment")]
    public string? Apartment { get; set; }

    [JsonPropertyName("xvsCurrent")]
    public decimal XVS_Current { get; set; }

    [JsonPropertyName("gvsCurrent")]
    public decimal? GVS_Current { get; set; }

    [JsonPropertyName("note")]
    public string? Note { get; set; }
}
