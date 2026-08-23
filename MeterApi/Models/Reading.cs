namespace MeterApi.Models;

public class Reading
{
    public int Id { get; set; }
    public DateTime ReadingDate { get; set; }
    public string Locality { get; set; } = string.Empty;
    public string? Street { get; set; }
    public string House { get; set; } = string.Empty;
    public string? Apartment { get; set; }
    public decimal XVS_Current { get; set; }
    public decimal? GVS_Current { get; set; }
    public string? Note { get; set; }
}

