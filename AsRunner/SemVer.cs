namespace AsRunner;

/// <summary>
/// Разбор и сравнение версий по правилам SemVer 2.0 (в объёме, нужном апдейтеру):
/// Major.Minor.Patch + опциональные pre-release-идентификаторы («-beta.1»).
/// System.Version не подходит — он не хранит суффикс пред-релиза.
/// </summary>
internal sealed record SemVer(int Major, int Minor, int Patch, string[] Prerelease) : IComparable<SemVer>
{
    /// <summary>Числовая часть как System.Version (для имён файлов и т.п.).</summary>
    public Version Numeric => new(Major, Minor, Patch);

    /// <summary>Парсит «v1.2.0-beta.1», «1.2.0», «1.2.0-beta.1+build» → SemVer (null при неудаче).</summary>
    public static SemVer? Parse(string? s)
    {
        if (string.IsNullOrWhiteSpace(s))
            return null;

        s = s.Trim().TrimStart('v', 'V');

        // Отбрасываем build-метаданные (SemVer: «+...» не участвуют в сравнении).
        int plus = s.IndexOf('+');
        if (plus >= 0)
            s = s[..plus];

        string core;
        string[] pre = [];

        int dash = s.IndexOf('-');
        if (dash >= 0)
        {
            core = s[..dash];
            var preStr = s[(dash + 1)..];
            pre = preStr.Length == 0 ? [] : preStr.Split('.');
        }
        else
        {
            core = s;
        }

        var parts = core.Split('.');
        if (parts.Length == 0 || !int.TryParse(parts[0], out var maj))
            return null;

        int min = parts.Length > 1 && int.TryParse(parts[1], out var mn) ? mn : 0;
        int pat = parts.Length > 2 && int.TryParse(parts[2], out var pt) ? pt : 0;

        return new SemVer(maj, min, pat, pre);
    }

    public int CompareTo(SemVer? other)
    {
        if (other is null)
            return 1;

        int c = Major.CompareTo(other.Major);
        if (c != 0) return c;
        c = Minor.CompareTo(other.Minor);
        if (c != 0) return c;
        c = Patch.CompareTo(other.Patch);
        if (c != 0) return c;

        // Версия без pre-release старше такой же с pre-release (1.2.0 > 1.2.0-beta).
        bool aPre = Prerelease.Length > 0;
        bool bPre = other.Prerelease.Length > 0;
        if (!aPre && !bPre) return 0;
        if (!aPre) return 1;
        if (!bPre) return -1;

        // Сравнение pre-release-идентификаторов слева направо.
        int n = Math.Min(Prerelease.Length, other.Prerelease.Length);
        for (int i = 0; i < n; i++)
        {
            c = CompareIdentifier(Prerelease[i], other.Prerelease[i]);
            if (c != 0) return c;
        }

        // Все совпавшие равны → у кого идентификаторов больше, тот старше.
        return Prerelease.Length.CompareTo(other.Prerelease.Length);
    }

    private static int CompareIdentifier(string a, string b)
    {
        bool aNum = int.TryParse(a, out var an);
        bool bNum = int.TryParse(b, out var bn);

        if (aNum && bNum) return an.CompareTo(bn);
        if (aNum) return -1;                    // числовой идентификатор младше буквенного
        if (bNum) return 1;
        return string.CompareOrdinal(a, b);
    }
}
