﻿using Avalonia;
using HeatProductionSystem.Models;
using System;

namespace HeatProductionSystem
{
    sealed class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            // Run the Avalonia app (for GUI-related setup)
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
           
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace();
    }
}
