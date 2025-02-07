using System.Runtime.InteropServices;

namespace DfT.DTRO.Utilities;

public static class OSHelper
{
    public static string GetOSAppDataPath()
    {
        var filePath = string.Empty;
        var dtroDataFolder = "DtroAppData";
        var dataExportFolder = "ExportedData";
        
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), 
                "AppData", dtroDataFolder, dataExportFolder);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), 
                "Library", "Application Support",  dtroDataFolder, dataExportFolder);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), 
                dtroDataFolder, dataExportFolder);
        }
        
        return filePath;
    }
}