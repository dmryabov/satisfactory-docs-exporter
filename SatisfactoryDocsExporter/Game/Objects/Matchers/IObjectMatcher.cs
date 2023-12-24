using CUE4Parse.UE4.Objects.UObject;
using SatisfactoryDocsExporter.Game.Objects.Wrappers;

namespace SatisfactoryDocsExporter.Game.Objects.Matchers;

public interface IObjectMatcher
{
    public UObjectWrapper? Match(FObjectExport export);
}