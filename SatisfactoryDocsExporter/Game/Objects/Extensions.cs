using CUE4Parse.UE4.Assets;
using CUE4Parse.UE4.Assets.Exports;

namespace SatisfactoryDocsExporter.Game.Objects;

public static class Extensions
{
    public static UObject? GetTopMostOuter(this UObject uobject)
    {
        var outer = uobject;

        do
        {
            outer = outer.Outer;
        } while (outer != null && outer is not IPackage);

        return outer;
    }

    public static string GetShortName(this UObject uobject)
    {
        return uobject.Outer != null && uobject.Outer != uobject.Owner
            ? uobject.GetPathName(uobject.GetTopMostOuter())
            : uobject.Name;
    }
}