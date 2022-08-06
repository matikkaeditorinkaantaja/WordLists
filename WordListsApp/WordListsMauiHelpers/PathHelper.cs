﻿using Microsoft.Maui.Controls.PlatformConfiguration;
using WordDataAccessLibrary.Helpers;

namespace WordListsMauiHelpers;
public class PathHelper
{

    /// <returns>downloads folder on windows and android, else path to myDocuments</returns>
    public static string GetDefaultBackupFolderPath()
    {
#if WINDOWS
        return Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), "Downloads");

#elif ANDROID
        return "/storage/emulated/0/Download/";
#else
        // hardcoded is kinda bad, but haven't found any not obsolete way (one of bad one under)
        // Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).Path;
        return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
#endif
    }

    /// <summary>
    /// Returns default export path for used platform with filename WordListsExport.json
    /// </summary>
    /// <returns>downloads folder on windows, else path to myDocuments</returns>
    public static string GetDefaultBackupFilePath()
    {
        return Path.Combine(GetDefaultBackupFolderPath(), GetNewBackupFileName());
    }

    public static string GetNewBackupFileName()
    {
        string date = DateTime.Now.ToString("G")
                                  .Replace(" ", string.Empty)
                                  .Replace("/", string.Empty)
                                  .Replace(":", string.Empty)
                                  .Replace("PM", string.Empty)
                                  .Replace("AM", string.Empty)
                                  .Replace(".", string.Empty);
        return $"WordListsExport{date}{AppFileExtension.GetExtension(FileExtension.Wordlist)}";
    }

}
