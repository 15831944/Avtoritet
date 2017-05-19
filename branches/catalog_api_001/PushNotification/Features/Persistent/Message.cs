namespace PushNotification.Features.Persistent
{
    public class Message
    {
        public string Type { get; set; }

        public string ConnectionId { get; set; }

        public string Content { get; set; }

        public string GroupName { get; set; }
    }
}