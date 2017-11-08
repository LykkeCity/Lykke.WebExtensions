using Lykke.AzureQueueIntegration;

namespace Lykke.WebExtensions
{
    public interface ILogToSlackSettings
    {
        AzureQueueSettings AzureQueue { get; }
    }
}