using RunAsApplication.Models;

namespace ConfigReader;

public class Reader
{
    public static RootConfig ReadConfig()
    {
        var json = File.ReadAllText("Config.json");
        var result = System.Text.Json.JsonSerializer.Deserialize<RootConfig>(json);
        return result!;
    }
}
