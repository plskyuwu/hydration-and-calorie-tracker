using System;
using System.IO;

namespace hydration_and_calorie_tracker.Models;

/// <summary>
/// Stores important directory paths
/// </summary>
public static class AppPaths
{
    public static string AppDataPath
    {
        get
        {
            var basePath =
                Environment.GetFolderPath(Environment.SpecialFolder
                    .LocalApplicationData);

            var path = Path.Combine(basePath, "plskyuwu.hydration-and-calorie-tracker");

            Directory.CreateDirectory(path);

            return path;
        }
    }

    public static string DatabasePath => Path.Combine(AppDataPath, "data.db");
}