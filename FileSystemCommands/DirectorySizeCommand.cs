using CommandLib;

namespace FileSystemCommands;

public class DirectorySizeCommand : ICommand
{
    private readonly string _path;

    public DirectorySizeCommand(string path)
    {
        _path = path;
    }

    public void Execute()
    {
        long size = Directory.EnumerateFiles(_path, "*", SearchOption.AllDirectories)
                             .Select(file => new FileInfo(file).Length)
                             .Sum();
        Console.WriteLine($"Размер каталога \"{_path}\": {size} байт");
    }
}
