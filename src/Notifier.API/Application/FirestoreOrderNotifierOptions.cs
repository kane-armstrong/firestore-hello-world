namespace Notifier.API.Application
{
    public class FirestoreOrderNotifierOptions
    {
        public string ProjectId { get; set; }
        public bool UseDevelopmentCredentials { get; set; }
        public string JsonCredentials { get; set; }
    }
}