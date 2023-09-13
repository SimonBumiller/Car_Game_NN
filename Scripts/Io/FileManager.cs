using System.IO;

public class FileManager
{
    public const string STATS_DIR = "stats";

    public static FileManager Instance = new();

    public void CreateFile(string name, string path)
    {
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        
        File.Create(Path.Combine(path, name)).Close();
    }

    public void WriteToFile(string content, string filePath)
    {
        File.WriteAllText(filePath, content);
    }
}