    namespace Domain.Configurations;

    public class MailConfiguration
    {
        public static string SectionName = "MailConfiguration";
        public string FromName { get; set; }

        public string FromEmail { get; set; }
        public string Url { get; set; }
        public string ApiKey { get; set; }
    }

