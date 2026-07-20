using System.Text.Json.Serialization;

namespace ConfigReader.Models;

public record ApplicationConfig(
    string FilePath,
    string? UserName = null,
    string? Domain = null,
    string? Alias = null,
    string? Arguments = null,
    // Показывать ли приложение в контекстном меню папок Explorer. По умолчанию false;
    // при false в конфиг не пишется (WhenWritingDefault), чтобы не засорять файл.
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    bool ShowInFolderMenu = false,
    // Глобальная горячая клавиша запуска, напр. "Ctrl+Alt+K". null → не назначена.
    string? Hotkey = null);
