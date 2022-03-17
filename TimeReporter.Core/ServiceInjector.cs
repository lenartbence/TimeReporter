using Microsoft.Extensions.DependencyInjection;
using System;
using TimeReporter.Core.Exporters.Factory;
using TimeReporter.Core.Storage;
using TimeReporter.Model;

namespace TimeReporter.Core
{
    public static class ServiceInjector
    {
        public static void AddCoreServices(this IServiceCollection services)
        {
            services.AddTransient<IExporterFactory, ExporterFactory>();
            services.AddTransient<IStorageManager<Day, DateTime>, DayStorageManager>();
            services.AddTransient<IStorageManager<ExporterDto>, ExporterStorageManager>();
            services.AddTransient<IStorageReader<Day, DateTime>, NationalSpecialDayStorageManager>();
        }
    }
}
