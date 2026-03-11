namespace eAviaSales.Domain.Models
{
    public class ActionResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public int StatusCode { get; set; }
    }
}
