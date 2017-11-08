namespace Lykke.WebExtensions
{
    public interface ILogSettings
    {
        ILogToSlackSettings LogToSlack { get; }

        ILogToAzureSettings LogToAzure { get; }

        string ServiceName { get; }
    }
}