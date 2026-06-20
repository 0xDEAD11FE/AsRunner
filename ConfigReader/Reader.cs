using System.Text.Json;
using System.Text.Json.Serialization;
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
    };

    // Путь строим от папки exe, а не от текущей рабочей директории —
    // иначе запуск из автозагрузки/ярлыка не найдёт файл.
    private static string ConfigPath => Path.Combine(AppContext.BaseDirectory, ConfigFileName);

    public static RootConfig ReadConfig()
    {
        var path = ConfigPath;

        if (!File.Exists(path))
            throw new FileNotFoundException($"Файл конфигурации не найден: {path}", path);

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

    public static void WriteConfig(RootConfig config)
    {
        var json = JsonSerializer.Serialize(config, WriteOptions);
        File.WriteAllText(ConfigPath, json);
    }
}
