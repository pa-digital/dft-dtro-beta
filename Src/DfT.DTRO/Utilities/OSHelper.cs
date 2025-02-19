using System.Runtime.InteropServices;

namespace DfT.DTRO.Utilities;

public static class OSHelper
{
    public static string GetOSAppDataPath()
    {
        var dtroDataFolder = "DtroAppData";
        var dataExportFolder = "ExportedData";
        var userProfilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        
        return RuntimeInformation.OSDescription switch
        {
            var os when RuntimeInformation.IsOSPlatform(OSPlatform.Windows) 
                => Path.Combine(userProfilePath, "AppData", "Local", dtroDataFolder, dataExportFolder),

            var os when RuntimeInformation.IsOSPlatform(OSPlatform.OSX) 
                => Path.Combine(userProfilePath, "Library", "Application Support", dtroDataFolder, dataExportFolder),

            var os when RuntimeInformation.IsOSPlatform(OSPlatform.Linux) 
                => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), dtroDataFolder, dataExportFolder),

            _ => Path.Combine(userProfilePath, ".local", "share", dtroDataFolder, dataExportFolder) // assumes generic unix machine
        };
    }
}