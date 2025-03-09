using Avalonia;
using System;
using System.IO;
using Semester2ProjectGroup22;

namespace Semester2ProjectGroup22
{
    class Program

    {
        [STAThread]
        // returned default commits
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        public static void Main(string[] args)
        {

            //ive modified this a bit for me its more readable





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
