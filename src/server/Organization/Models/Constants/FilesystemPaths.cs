namespace Organization.Models.Constants;

public static class FilesystemPaths
{
#if !DEBUG
    /// <summary> Путь к конфигурационному файлу </summary>
    public static readonly string ConfigDir = Directory.GetCurrentDirectory();
#else
    /// <summary> Путь к конфигурационному файлу </summary>
    public static readonly string ConfigDir = $@"{Directory.GetCurrentDirectory()}\Models\Config\";
#endif
    
    /// <summary> Название конфигурационного файла </summary>
    public const string ConfigName = "config.json";
}