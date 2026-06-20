using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using ConfigReader.Models;

namespace ConfigReader;

public class Reader
{
    private const string ConfigFileName = "Config.json";

    private static readonly JsonSerializerOptions WriteOptions = new()
    {
        WriteIndented = true,
        // null-поля (UserName/Domain/Alias) не пишем — конфиг остаётся чистым.
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        // Пишем кириллицу как читаемый UTF-8, а не \uXXXX (файл правится руками).
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
    };

    // Активный конфиг: портативный рядом с exe имеет приоритет, иначе —
    // per-user в %APPDATA%\AsRunner (пишется без прав, у каждого пользователя свой).
    private static string ResolveConfigPath()
    {
        var portable = Path.Combine(AppContext.BaseDirectory, ConfigFileName);
        if (File.Exists(portable))
            return portable;

        return Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "AsRunner",
            ConfigFileName);
    }

    public static RootConfig ReadConfig()
    {
        var path = ResolveConfigPath();

        // Первый запуск установленной версии: в %APPDATA% конфига ещё нет —
        // создаём пустой, чтобы приложение стартовало (меню настраивается из UI).
        if (!File.Exists(path))
            WriteConfigTo(path, new RootConfig());

        var json = File.ReadAllText(path);

        RootConfig? result;
        try
        {
            result = JsonSerializer.Deserialize<RootConfig>(json);
        }
        catch (JsonException ex)
        {
            throw new InvalidDataException($"Не удалось разобрать {ConfigFileName}: {ex.Message}", ex);
        }

        if (result is null)
            throw new InvalidDataException($"{ConfigFileName} пуст или содержит null.");

        return result;
    }

    public static void WriteConfig(RootConfig config) => WriteConfigTo(ResolveConfigPath(), config);

    private static void WriteConfigTo(string path, RootConfig config)
    {
        var dir = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(dir))
            Directory.CreateDirectory(dir);

        File.WriteAllText(path, JsonSerializer.Serialize(config, WriteOptions));
    }
}
