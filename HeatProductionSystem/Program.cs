using Avalonia;
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
           
            try
            {

                var resultDataManager = new ResultDataManager();
                var optimizer = new Optimizer();

                optimizer.ReadInformation();

                resultDataManager.SaveResultsToCsv("GB1");
                resultDataManager.SaveResultsToCsv("GB2");
                resultDataManager.SaveResultsToCsv("OB1");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during optimization: " + ex.Message);
            }
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace();
    }
}
