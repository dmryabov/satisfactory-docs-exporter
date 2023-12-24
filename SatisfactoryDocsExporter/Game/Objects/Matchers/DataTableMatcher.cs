using CUE4Parse.UE4.Assets.Exports.Engine;
using CUE4Parse.UE4.Objects.UObject;
using SatisfactoryDocsExporter.Game.Objects.Wrappers;

namespace SatisfactoryDocsExporter.Game.Objects.Matchers;

public class DataTableMatcher : IObjectMatcher
{
    private const string DataTableClass = "DataTable";

    private readonly string _tableName;

    public DataTableMatcher(string tableName)
    {
        _tableName = tableName;
    }

    public UObjectWrapper? Match(FObjectExport export)
    {
        if (export.ClassIndex.Name != DataTableClass || export.ObjectName.Text != _tableName) return null;

        var table = (UDataTable)export.ExportObject.Value;

        return new UDataTableWrapper(table);
    }
}