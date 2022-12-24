using System;
using System.IO;
using TFlic.Model.Configuration;

namespace TFlic.Model.Constants;

public static class FilesystemPaths
{
    static FilesystemPaths()
    {
        ProgramRoot = Path.Join(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            ConfigurationUtils.FromConfiguration["program_root_relative_path"]
        );
        if (!Directory.Exists(ProgramRoot)) { Directory.CreateDirectory(ProgramRoot!); }
        
        AccountFullClass = Path.Join(ProgramRoot, AccountRelativePath);
    }
    
    /// <summary> Путь к конфигурационному файлу </summary>
    public static readonly string ConfigDir = Directory.GetCurrentDirectory();
    
    /// <summary> Название конфигурационного файла </summary>
    public const string ConfigName = "configuration.json";

    /// <summary> Путь к директории с файлами программы </summary>
    public static readonly string ProgramRoot;

    /// <summary> Путь к ключам шифрования </summary>
    /// <remarks> Путь относителен и начинается с корневой директории программы </remarks>
    public static readonly string? AccountRelativePath = ConfigurationUtils.FromConfiguration["account_relative_path"];

    public static readonly string? AccountFullClass;
}