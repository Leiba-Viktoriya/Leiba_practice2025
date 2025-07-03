using System;
using System.IO;
using CommandLib;

namespace FileSystemCommands;

public class FindFilesCommand : ICommand
{
    private readonly string _path;
    private readonly string _mask;

    public FindFilesCommand(string path, string mask)
    {
        _path = path;
        _mask = mask;
    }

    public void Execute()
    {
        var files = Directory.EnumerateFiles(_path, _mask, SearchOption.AllDirectories);
        Console.WriteLine($"Файлы, соответствующие маске \"{_mask}\" в каталоге \"{_path}\":");
        foreach (var file in files)
        {
            Console.WriteLine(file);
        }
    }
}
