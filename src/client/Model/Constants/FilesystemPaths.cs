namespace TFlic.Model.Constants;

public static class FilesystemPaths
{
#if !DEBUG
    /// <summary> Путь к конфигурационному файлу </summary>
    public static readonly string ConfigDir = Directory.GetCurrentDirectory();
#else
    /// <summary> Путь к конфигурационному файлу </summary>
    public const string ConfigDir = @"D:\dev\TFlic client\src\client\Model\Config\";
#endif
    
    /// <summary> Название конфигурационного файла </summary>
    public const string ConfigName = "config.json";
}