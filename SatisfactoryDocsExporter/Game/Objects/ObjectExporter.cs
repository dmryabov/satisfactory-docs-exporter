using CUE4Parse.FileProvider;
using CUE4Parse.FileProvider.Objects;
using CUE4Parse.UE4.Assets;
using CUE4Parse.UE4.Objects.UObject;
using SatisfactoryDocsExporter.Game.Objects.Matchers;
using SatisfactoryDocsExporter.Game.Objects.Wrappers;
using Serilog;

namespace SatisfactoryDocsExporter.Game.Objects;

public class ObjectExporter
{
    private const string FilePathPrefix = "FactoryGame/Content/FactoryGame/";
    // private const string FilePathPrefix = "FactoryGame/Content/FactoryGame/Equipment/";
    // private const string FilePathPrefix = "FactoryGame/Content/FactoryGame/Events/Christmas/Parts/";
    // private const string FilePathPrefix = "FactoryGame/Content/FactoryGame/Buildable/Factory/PowerTower";
    // private const string FilePathPrefix = "FactoryGame/Content/FactoryGame/Unlocks/";
    // private const string FilePathPrefix = "FactoryGame/Content/FactoryGame/Recipes/Smelter/";
    // private const string FilePathPrefix = "FactoryGame/Content/FactoryGame/Schematics/";
    // private const string FilePathPrefix = "FactoryGame/Content/FactoryGame/Buildable/";
    // private const string FilePathPrefix = "FactoryGame/Content/FactoryGame/Buildable/Vehicle/Train/Locomotive";

    private const string FileExtension = "uasset";
    private readonly List<IObjectMatcher> _matchers = [];

    private readonly IFileProvider _provider;

    public ObjectExporter(IFileProvider provider)
    {
        _provider = provider;
    }

    public ObjectExporter RegisterMatcher(IObjectMatcher matcher)
    {
        _matchers.Add(matcher);

        return this;
    }

    public IEnumerable<UObjectWrapper> GetExports()
    {
        return GetPackageExports()
            .Select(LogPackageExport)
            .Select(MatchExport)
            .OfType<UObjectWrapper>()
            .DistinctBy(w => w.Id);
    }

    private UObjectWrapper? MatchExport(FObjectExport export)
    {
        return _matchers
            .Select(matcher => matcher.Match(export))
            .OfType<UObjectWrapper>()
            .Select(LogMatchedExport)
            .FirstOrDefault();
    }

    private IEnumerable<FObjectExport> GetPackageExports()
    {
        return _provider.Files.Values
            .Where(f => f.Extension == FileExtension)
            .Where(f => f.Path.StartsWith(FilePathPrefix))
            .Select(LogGameFile)
            .Select(f => _provider.LoadPackage(f))
            .Cast<Package>()
            .SelectMany(p => p.ExportMap);
    }

    private static GameFile LogGameFile(GameFile file)
    {
        Log.Debug("Processing file {0}", file.ToString());

        return file;
    }

    private static FObjectExport LogPackageExport(FObjectExport export)
    {
        Log.Debug("Processing export {0}", export.ToString());

        return export;
    }

    private static UObjectWrapper LogMatchedExport(UObjectWrapper export)
    {
        Log.Debug("Matched export {0}", export.ToString());

        return export;
    }
}