namespace Domain.Configurations;

public class AppUrlConfiguration
{
    public static string SectionName = "AppUrls";

    public string PaymentCallback { get; set; } = default!;
}