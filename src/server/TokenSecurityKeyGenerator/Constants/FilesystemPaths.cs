using TokenSecurityKeyGenerator.Config;

namespace TokenSecurityKeyGenerator.Constants;

public static class FilesystemPaths
{
    static FilesystemPaths()
    {
        ProgramRoot = Path.Join(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            ConfigurationUtils.FromConfiguration["program_root_relative_path"]
        );
        if (!Directory.Exists(ProgramRoot)) { Directory.CreateDirectory(ProgramRoot!); }
        
        KeysFullPath = Path.Join(ProgramRoot, KeysRelativePath);
    }
    
    /// <summary> Путь к конфигурационному файлу </summary>
    public static readonly string ConfigDir = Directory.GetCurrentDirectory();
    
    /// <summary> Название конфигурационного файла </summary>
    public const string ConfigName = "config.json";

    /// <summary> Путь к директории с файлами программы </summary>
    public static readonly string ProgramRoot;

    /// <summary> Путь к ключам шифрования </summary>
    /// <remarks> Путь относителен и начинается с корневой директории программы </remarks>
    public static readonly string? KeysRelativePath = ConfigurationUtils.FromConfiguration["key_relative_path"];

    /// <summary> Полный путь к файлам с ключами </summary>
    public static readonly string? KeysFullPath;
}