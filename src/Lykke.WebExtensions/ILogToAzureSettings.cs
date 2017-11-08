namespace Lykke.WebExtensions
{
    public interface ILogToAzureSettings
    {
        string ConnectionString { get; }
        string TableName { get; }
    }
}