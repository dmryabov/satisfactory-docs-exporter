using CUE4Parse.UE4.Assets.Objects.Properties;

namespace SatisfactoryDocsExporter.Game.Objects.Wrappers.Properties;

public static class PropertyWrapperFactory
{
    public static IPropertyWrapper Wrap(FPropertyTagType? type)
    {
        return type switch
        {
            ArrayProperty p => new ArrayPropertyWrapper(p),
            MapProperty p => new MapPropertyWrapper(p),

            ObjectProperty p => new ObjectPropertyWrapper(p),
            StructProperty p => new StructPropertyWrapper(p),
            SoftObjectProperty p => new SoftObjectPropertyWrapper(p),

            TextProperty p => new TextPropertyWrapper(p),
            NameProperty p => new NamePropertyWrapper(p),

            BoolProperty p => new DefaultPropertyWrapper(p),
            ByteProperty p => new DefaultPropertyWrapper(p),
            IntProperty p => new DefaultPropertyWrapper(p),
            Int8Property p => new DefaultPropertyWrapper(p),
            Int16Property p => new DefaultPropertyWrapper(p),
            Int64Property p => new DefaultPropertyWrapper(p),
            UInt16Property p => new DefaultPropertyWrapper(p),
            UInt32Property p => new DefaultPropertyWrapper(p),
            UInt64Property p => new DefaultPropertyWrapper(p),
            FloatProperty p => new DefaultPropertyWrapper(p),
            DoubleProperty p => new DefaultPropertyWrapper(p),
            EnumProperty p => new DefaultPropertyWrapper(p),
            StrProperty p => new DefaultPropertyWrapper(p),

            _ => new DebugPropertyWrapper(type)
        };
    }
}