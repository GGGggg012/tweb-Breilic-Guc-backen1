namespace eAviaSales.Domain.Models
{
    public class TripDto
    {
        public string Id { get; set; } = string.Empty;
        public string RouteLabel { get; set; } = string.Empty;
        public string Pnr { get; set; } = string.Empty;
        public string Status { get; set; } = "upcoming";
        public string CityHint { get; set; } = string.Empty;
        public string DateRange { get; set; } = string.Empty;
        public string Airline { get; set; } = string.Empty;
        public bool CheckInAvailable { get; set; }
        public string? CheckInUrl { get; set; }
    }
}
