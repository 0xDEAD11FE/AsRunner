namespace ConfigReader.Models;

public record ApplicationConfig(string FilePath, string? UserName = null, string? Domain = null);