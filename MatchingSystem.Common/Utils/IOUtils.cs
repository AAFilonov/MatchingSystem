using System.IO;

namespace MatchingSystem.Constants.Utils;

public class IOUtils
{
    private const string TestBinDirectoryPath = "./../../../";

    public static void WriteStringToFile(string json, string path, string pathToRoot = TestBinDirectoryPath)
    {
        using (StreamWriter outputFile = new StreamWriter(TestBinDirectoryPath + path))
        {
            outputFile.Write(json);
        }
    }

    public static string ReadFileAsString(string path, string pathToRoot = TestBinDirectoryPath)
    {
        var json = File.ReadAllText(pathToRoot + path);
        return json;
    }
}