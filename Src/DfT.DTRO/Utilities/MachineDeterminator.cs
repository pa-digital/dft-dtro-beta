namespace DfT.DTRO.Utilities;

public static class MachineDeterminator
{
    public static string GetOSPath()
    {
        
        // Get the OS version
        OperatingSystem os = Environment.OSVersion;
        // Get the platform identifier
        PlatformID pid = os.Platform;

        if (pid == PlatformID.Win32NT || pid == PlatformID.Win32Windows)
        {
            return Path.Combine("C:\\Users\\cameron.auld\\", "AppData", "Exported_Files");
        }
        if (pid == PlatformID.Unix || pid == PlatformID.MacOSX)
        {
            return Path.Combine("/Users/cameron.auld/Library", "AppData", "Exported_Files");
        }
        
        return string.Empty;
    }
   
}