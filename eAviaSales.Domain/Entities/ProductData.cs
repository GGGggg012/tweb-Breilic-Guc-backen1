namespace eAviaSales.Domain.Entities
{
    public class ProductData
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }

        public string Airline { get; set; } = string.Empty;
        public string AirlineCode { get; set; } = string.Empty;
        public string Route { get; set; } = string.Empty;
        public string FlightDate { get; set; } = string.Empty;
        public int Stops { get; set; } = 0;
        public int DurationMin { get; set; } = 0;

        /// <summary>РЎРєСЂС‹С‚ СЃ РєР°С‚Р°Р»РѕРіР° (РЅРµ СѓРґР°Р»С‘РЅ).</summary>
        public bool IsActive { get; set; } = true;

        public bool IsAvailable() => IsActive && Stock > 0;

        public void DecreaseStock(int amount)
        {
            if (amount > Stock)
                throw new InvalidOperationException("Not enough stock");
            Stock -= amount;
        }
    }
}
