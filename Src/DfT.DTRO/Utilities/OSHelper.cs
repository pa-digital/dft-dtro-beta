using System.Runtime.InteropServices;

namespace DfT.DTRO.Utilities;

public static class OSHelper
{
    public static string GetOSAppDataPath()
    {
        string filePath = string.Empty;
        var dtroDataFolder = "DtroAppData";
        var dataExportFolder = "ExportedData";
        
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            filePath = Path.Combine(homeDirectory, "AppData", dtroDataFolder, dataExportFolder);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            var homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            filePath = Path.Combine(homeDirectory, "Library", "Application Support",  dtroDataFolder, dataExportFolder);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), dtroDataFolder, dataExportFolder);
        }
        
        return filePath;
    }
}