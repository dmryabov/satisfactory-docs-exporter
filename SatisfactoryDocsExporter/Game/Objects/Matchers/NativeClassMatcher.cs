using CUE4Parse.UE4.Assets;
using CUE4Parse.UE4.Objects.UObject;
using SatisfactoryDocsExporter.Game.Headers;
using SatisfactoryDocsExporter.Game.Objects.Wrappers;

namespace SatisfactoryDocsExporter.Game.Objects.Matchers;

public class NativeClassMatcher : IObjectMatcher
{
    private const string BluePrintClassName = "BlueprintGeneratedClass";

    private readonly ClassTree _classTree;
    private readonly string[] _exceptNativeClassNames;
    private readonly string _nativeRootClassName;

    public NativeClassMatcher(ClassTree classTree, string nativeRootClassName)
    {
        _classTree = classTree;
        _nativeRootClassName = nativeRootClassName;
        _exceptNativeClassNames = [];
    }

    public NativeClassMatcher(ClassTree classTree, string nativeRootClassName, string[] exceptNativeClassNames) : this(classTree, nativeRootClassName)
    {
        _classTree = classTree;
        _nativeRootClassName = nativeRootClassName;
        _exceptNativeClassNames = exceptNativeClassNames;
    }

    public UObjectWrapper? Match(FObjectExport export)
    {
        var selfClass = export.ClassIndex.ResolvedObject;
        if (selfClass == null) return null;

        var isBluePrint = selfClass.Name.Text == BluePrintClassName || selfClass.Class?.Name.Text == BluePrintClassName;
        if (!isBluePrint) return null;

        var superClass = export.SuperIndex.ResolvedObject ?? export.TemplateIndex.ResolvedObject?.Class?.Super;
        if (superClass == null) return null;

        var nativeSuperClass = GetNativeClass(superClass);
        if (!IsSubclass(nativeSuperClass, _nativeRootClassName)) return null;
        if (IsExcludedSubclass(nativeSuperClass)) return null;

        var uobject = export.ExportObject.Value;

        return new UObjectWrapper(uobject, nativeSuperClass, GetNativeSuperClasses(nativeSuperClass));
    }

    private static string GetNativeClass(ResolvedObject obj)
    {
        var superObject = obj.Super;

        return superObject == null ? obj.Name.Text : GetNativeClass(superObject);
    }

    private string[] GetNativeSuperClasses(string className)
    {
        return _classTree.GetClassChain(className, _nativeRootClassName).ToArray();
    }

    private bool IsSubclass(string className, string superClassName)
    {
        return _classTree.IsSubclass(className, superClassName);
    }

    private bool IsExcludedSubclass(string className)
    {
        return _exceptNativeClassNames
            .Any(exceptClassName => IsSubclass(className, exceptClassName));
    }
}