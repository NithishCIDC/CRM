namespace CRM.Application.DTO
{
    public class NotificationRequest
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
    }
}