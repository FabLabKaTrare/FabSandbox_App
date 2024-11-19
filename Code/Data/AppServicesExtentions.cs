using FabSandbox.Data.Interfaces;
using FabSandbox.Data.Services;
using Microsoft.Extensions.Configuration;
using MudBlazor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabSandbox.Data
{
    public static class AppServicesExtentions
    {
        public static IServiceCollection AddApplicationServicesProviders(this IServiceCollection services)
        {
            services.AddMudServices();

            services.AddSingleton<DeviceDataService>();

            services.AddTransient<IDeviceServices, DeviceServices>();           

            services.AddScoped<IUserInterfaceToolsServices, UserInterfaceToolsServices>();

            services.AddSingleton<IWebSocketService, WebSocketService>();

            return services;
        }

    }
}
