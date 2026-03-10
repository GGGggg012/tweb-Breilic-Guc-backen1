using System.ComponentModel.DataAnnotations;

namespace eAviaSales.Domain.Models
{
    public class ProductDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 999999.99)]
        public decimal Price { get; set; }

        public int Stock { get; set; }
        public bool InStock { get; set; }

        // Авиа-поля (для фронта)
        public string Airline { get; set; } = string.Empty;
        public string AirlineCode { get; set; } = string.Empty;
        public string Route { get; set; } = string.Empty;
        public string FlightDate { get; set; } = string.Empty;
        public int Stops { get; set; } = 0;
        public int DurationMin { get; set; } = 0;

        /// <summary>В каталоге; false — скрыт (админ видит в панели).</summary>
        public bool IsActive { get; set; } = true;
    }
}
