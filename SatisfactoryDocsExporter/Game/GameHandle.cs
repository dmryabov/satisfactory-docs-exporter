using PeNet;
using Serilog;

namespace SatisfactoryDocsExporter.Game;

public class GameHandle
{
    public GameHandle(DirectoryInfo rootDirectory)
    {
        if (!rootDirectory.Exists) throw new DirectoryNotFoundException($"Directory '{rootDirectory}' does not exist.");

        RootDirectory = rootDirectory;
        Version = DetectVersion();
    }

    public DirectoryInfo RootDirectory { get; }
    public string Version { get; }

    public string ContentDirectory => Path.Combine(RootDirectory.FullName, "FactoryGame/Content/Paks");
    public string MappingPath => Path.Combine(RootDirectory.FullName, "CommunityResources/FactoryGame.usmap");
    public string HeadersPath => Path.Combine(RootDirectory.FullName, "CommunityResources/Headers.zip");

    private string ExePath => Path.Combine(RootDirectory.FullName, "FactoryGame.exe");

    private string DetectVersion()
    {
        var version = "UNKNOWN";
        var exe = new PeFile(ExePath);

        var tables = exe.Resources?.VsVersionInfo?.StringFileInfo.StringTable ?? [];

        if (tables is [{ ProductVersion: not null }])
            version = tables[0].ProductVersion ?? version;
        else
            Log.Warning("Failed to parse game version");

        Log.Information("Detected game version: {0}", version);
        return version;
    }
}