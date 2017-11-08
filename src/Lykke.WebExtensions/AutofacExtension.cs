using System;
using Autofac;
using AzureStorage.Tables;
using Common.Log;
using Lykke.Logs;
using Lykke.SettingsReader;
using Lykke.SlackNotification.AzureQueue;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lykke.WebExtensions
{
    public static class AutofacExtension
    {
        public static void RegisterWebExtensions<TSettings, TISettings>(this ContainerBuilder builder, IServiceCollection services, IConfigurationRoot configuration, string settingsUrlPath, ILog lastResortLog)
            where TSettings : class, ILogSettings, TISettings
        {
            var settings = configuration.LoadSettings<TSettings>(settingsUrlPath);

            builder.RegisterInstance(settings.Nested(x => (ILogSettings)x))
                .As<IReloadingManager<ILogSettings>>()
                .SingleInstance();

            builder.RegisterInstance(settings.Nested(x => (TISettings)x))
                .As<IReloadingManager<TISettings>>()
                .SingleInstance();

            builder.Register(ctx =>
            {
                var logToSlack = new LykkeLogToAzureSlackNotificationsManager(settings.CurrentValue.ServiceName,
                    services.UseSlackNotificationsSenderViaAzureQueue(settings.CurrentValue.LogToSlack.AzureQueue, lastResortLog),
                    lastResortLog);

                var logToAzure = new LykkeLogToAzureStoragePersistenceManager(settings.CurrentValue.ServiceName,
                    AzureTableStorage<LogEntity>.Create(settings.ConnectionString(x => x.LogToAzure.ConnectionString), settings.CurrentValue.LogToAzure.TableName, lastResortLog),
                    lastResortLog);

                var logToTable = new LykkeLogToAzureStorage(settings.CurrentValue.ServiceName, logToAzure, logToSlack, lastResortLog);
                return logToTable;
            }).As<ILog>().SingleInstance();

            builder.Register(ctx => new GlobalErrorHandlerMiddleware(ctx.Resolve<ILog>(), settings.CurrentValue.ServiceName)).AsSelf().SingleInstance();
        }
    }
}
