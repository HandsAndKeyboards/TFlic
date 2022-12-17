namespace TokenSecurityKeyGenerator.Constants;

public static class FilesystemPaths
{
#if !DEBUG
    /// <summary> Путь к конфигурационному файлу </summary>
    public static readonly string ConfigDir = Directory.GetCurrentDirectory();
#else
    /// <summary> Путь к конфигурационному файлу </summary>
    public static readonly string ConfigDir = $@"{new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent}\Config\";
#endif
    
    /// <summary> Название конфигурационного файла </summary>
    public const string ConfigName = "config.json";
}