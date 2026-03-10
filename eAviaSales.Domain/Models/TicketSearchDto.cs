using System.Collections.Generic;

namespace eAviaSales.Domain.Models
{
    public class TicketSearchDto
    {
        public string? From { get; set; }
        public string? To { get; set; }
        public decimal? MaxPrice { get; set; }
        public List<string>? AirlineCodes { get; set; }
        public List<int>? Stops { get; set; }
    }
}
