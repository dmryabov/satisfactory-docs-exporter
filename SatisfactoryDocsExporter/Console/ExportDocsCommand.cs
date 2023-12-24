using System.CommandLine;
using SatisfactoryDocsExporter.Game;
using Serilog;

namespace SatisfactoryDocsExporter.Console;

public static class ExportDocsCommand
{
    public static Command Create()
    {
        var gameDirOption = new Option<DirectoryInfo>(
            "--game-dir",
            description: "Path to Satisfactory game directory where FactoryGame.exe is located",
            getDefaultValue: () => new DirectoryInfo("..")
        );
        gameDirOption.AddAlias("-d");

        var outputOption = new Option<FileInfo>(
            "--output",
            description: "Path to output JSON file.",
            getDefaultValue: () => new FileInfo("Docs.json")
        );
        outputOption.AddAlias("-o");

        var command = new RootCommand("Parser of Satisfactory game's data files to JSON database.");
        command.AddOption(gameDirOption);
        command.AddOption(outputOption);
        command.SetHandler(Run, gameDirOption, outputOption);

        return command;
    }

    private static void Run(DirectoryInfo gameDir, FileInfo output)
    {
        Log.Information("Using game directory {0}", gameDir.FullName);
        Log.Information("Using output file {0}", output.FullName);

        GameHandle game;

        try
        {
            game = new GameHandle(gameDir);
        }
        catch (Exception e)
        {
            throw new ApplicationException("Cannot load game info. Check game directory is valid.", e);
        }

        try
        {
            new DocsExporter(game, output).Run();
        }
        catch (SystemException e)
        {
            throw new ApplicationException("Cannot write/read some files. Check game is not missing some files and output path is valid.", e);
        }
    }
}