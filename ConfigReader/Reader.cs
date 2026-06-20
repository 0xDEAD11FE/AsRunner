using ConfigReader.Models;

namespace ConfigReader;

public class Reader
{
    private const string ConfigFileName = "Config.json";

    public static RootConfig ReadConfig()
    {
        // Путь строим от папки exe, а не от текущей рабочей директории —
        // иначе запуск из автозагрузки/ярлыка не найдёт файл.
        var path = Path.Combine(AppContext.BaseDirectory, ConfigFileName);

        if (!File.Exists(path))
            throw new FileNotFoundException($"Файл конфигурации не найден: {path}", path);

        var json = File.ReadAllText(path);

        RootConfig? result;
        try
        {
            result = System.Text.Json.JsonSerializer.Deserialize<RootConfig>(json);
        }
        catch (System.Text.Json.JsonException ex)
        {
            throw new InvalidDataException($"Не удалось разобрать {ConfigFileName}: {ex.Message}", ex);
        }

        if (result is null)
            throw new InvalidDataException($"{ConfigFileName} пуст или содержит null.");

        return result;
    }
}
