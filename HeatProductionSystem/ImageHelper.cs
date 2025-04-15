using System;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace HeatProductionSystem;

public static class ImageHelper
{
    public static Bitmap LoadFromResource(Uri resourceUri)
    {
        return new Bitmap(AssetLoader.Open(resourceUri));
    }
}

public static class AppEnvironment
{
    public static bool IsTestMode { get; set; } = false;
}
