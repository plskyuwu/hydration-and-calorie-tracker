using System;
using System.IO;

namespace hydration_and_calorie_tracker.Models;

/// <summary>
/// Stores important directory paths
/// </summary>
public static class AppPaths
{
    /// <summary>
    /// The os's proper application data directory. <br/>
    /// Linux: ~/.local/share/plskyuwu.hydration-and-calorie-tracker <br/>
    /// macOS: ~/Library/Application Support/plskyuwu.hydration-and-calorie-tracker <br/>
    /// Windows: ?
    /// </summary>
    public static string AppDataPath
    {
        get
        {
            var basePath =
                Environment.GetFolderPath(Environment.SpecialFolder
                    .LocalApplicationData);

            var path = Path.Combine(basePath,
                "plskyuwu.hydration-and-calorie-tracker");

            Directory.CreateDirectory(path);

            return path;
        }
    }

    /// <summary>
    /// The database file path. (AppDataPath + data.db)
    /// </summary>
    public static string DatabasePath => Path.Combine(AppDataPath, "data.db");
}