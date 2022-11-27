namespace TFlic.Model.Constants;

public static class FilesystemPaths
{
    // todo чтение из файла
#if !DEBUG
    public const string TokensFilePath = "tokens.json"; // todo
#else
    public const string TokensFilePath = "D:/tokens.json";
#endif
}