namespace eAviaSales.Domain.Models
{
    public class TicketDto
    {
        public string Id { get; set; } = string.Empty;
        public string Airline { get; set; } = string.Empty;
        public string Route { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int DurationMin { get; set; }
        public int Stops { get; set; }
        public string AirlineCode { get; set; } = string.Empty;
    }
}
