namespace eAviaSales.Domain.Models
{
    public class AdminBookingsQueryDto
    {
        public string? Query { get; set; }
        public string? DateFrom { get; set; }
        public string? DateTo { get; set; }
    }

    public class AdminBookingDto
    {
        public int OrderId { get; set; }
        public string Ref { get; set; } = string.Empty;
        public string Route { get; set; } = string.Empty;
        public string Created { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        /// <summary>Имя пользователя, оформившего заказ.</summary>
        public string UserFirstName { get; set; } = string.Empty;

        /// <summary>Email пользователя.</summary>
        public string UserEmail { get; set; } = string.Empty;
    }
}
