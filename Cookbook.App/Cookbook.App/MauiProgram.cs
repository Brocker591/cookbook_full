﻿using Cookbook.App.ViewModels;
using Microsoft.Extensions.Logging;

namespace Cookbook.App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            
            
            //ViewModels
            builder.Services.AddSingleton<MainPageViewModel>();

            //Views
            builder.Services.AddSingleton<MainPage>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
