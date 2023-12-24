using System.IO.Compression;
using CUE4Parse.FileProvider;
using CUE4Parse.MappingsProvider;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;
using SatisfactoryDocsExporter.Game.Headers;
using SatisfactoryDocsExporter.Game.Objects;
using SatisfactoryDocsExporter.Game.Objects.Matchers;
using Serilog;
using ClassParser = SatisfactoryDocsExporter.Game.Headers.ClassParser;

namespace SatisfactoryDocsExporter.Game;

public class DocsExporter(GameHandle game, FileInfo output)
{
    public void Run()
    {
        CheckOutputPath();

        Log.Information("Start parsing class tree");
        var classTree = ParseClassTree();
        Log.Information("Done parsing class tree");

        var provider = GetFileProvider();

        var exporter = new ObjectExporter(provider)
                .RegisterMatcher(new NativeClassMatcher(classTree, "FGItemDescriptor"))
                .RegisterMatcher(new NativeClassMatcher(classTree, "FGBuildable"))
                .RegisterMatcher(new NativeClassMatcher(classTree, "FGEquipment"))
                .RegisterMatcher(new NativeClassMatcher(classTree, "FGRecipe"))
                .RegisterMatcher(new NativeClassMatcher(classTree, "FGSchematic"))
                .RegisterMatcher(new NativeClassMatcher(classTree, "FGVehicle"))
                .RegisterMatcher(new NativeClassMatcher(classTree, "FGCategory", ["FGUserSettingCategory"]))
                .RegisterMatcher(new NativeClassMatcher(classTree, "FGUnlock"))
                .RegisterMatcher(new DataTableMatcher("DT_ResourceSinkPoints"))
                .RegisterMatcher(new DataTableMatcher("DT_ExplorationSinkPoints"))
            ;

        Log.Information("Start exporting objects");
        var objects = exporter.GetExports().ToList();
        Log.Information("Done exporting objects");

        var docs = new Dictionary<string, object>
        {
            { "GameVersion", game.Version },
            { "Classes", objects }
        };

        Log.Information("Start serializing objects");
        var json = JsonConvert.SerializeObject(docs, Formatting.Indented);
        Log.Information("Done serializing objects");

        Log.Information("Start writing JSON");
        using var writer = new StreamWriter(output.OpenWrite());
        writer.Write(json);
        Log.Information("Done writing JSON");
    }

    private void CheckOutputPath()
    {
        using var writer = new StreamWriter(output.Open(FileMode.Append));

        writer.Write(string.Empty);
    }

    private DefaultFileProvider GetFileProvider()
    {
        var provider = new DefaultFileProvider(game.ContentDirectory, SearchOption.TopDirectoryOnly, true, new VersionContainer(EGame.GAME_UE5_2));
        provider.MappingsContainer = new FileUsmapTypeMappingsProvider(game.MappingPath);

        provider.Initialize();
        provider.Mount();
        provider.LoadLocalization();

        return provider;
    }

    private ClassTree ParseClassTree()
    {
        var classRegistry = new ClassRegistry();
        var classParser = new ClassParser();

        using var archive = ZipFile.OpenRead(game.HeadersPath);

        foreach (var entry in archive.Entries)
        {
            var content = new StreamReader(entry.Open()).ReadToEnd();

            classParser
                .ParseFile(content)
                .ForEach(definition => classRegistry.Add(definition));
        }

        return new ClassTree(classRegistry);
    }
}