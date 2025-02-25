namespace DfT.DTRO.IntegrationTests.IntegrationTests.Helpers
{
    public static class FileHelper
    {
        public static void WriteStringToFile(string directory, string fileName, string stringToWrite)
        {
            Directory.CreateDirectory(directory);
            string filePath = $"{directory}/{fileName}";
            File.WriteAllText(filePath, stringToWrite);
        }

        public static void DeleteFilesInDirectory(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                string[] files = Directory.GetFiles(directoryPath);

                foreach (string file in files)
                {
                    File.Delete(file);
                    Console.WriteLine($"Deleted file {file}");
                }
            }
        }
    }
}