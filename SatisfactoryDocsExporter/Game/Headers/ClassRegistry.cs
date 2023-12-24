using System.Collections;

namespace SatisfactoryDocsExporter.Game.Headers;

public class ClassRegistry : IEnumerable<ClassDefinition>
{
    private readonly Dictionary<string, ClassDefinition> _classes = [];

    public int Count => _classes.Count;

    public ClassDefinition this[string className] => _classes[className];

    public IEnumerator<ClassDefinition> GetEnumerator()
    {
        return _classes.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(ClassDefinition classDefinition)
    {
        _classes.Add(classDefinition.ClassName, classDefinition);
    }

    public bool Contains(string className)
    {
        return _classes.ContainsKey(className);
    }
}