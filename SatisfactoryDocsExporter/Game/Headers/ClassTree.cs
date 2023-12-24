namespace SatisfactoryDocsExporter.Game.Headers;

public class ClassTree
{
    private readonly Dictionary<string, ClassTreeNode> _nodes = [];

    public ClassTree(ClassRegistry classRegistry)
    {
        foreach (var classDefinition in classRegistry) _nodes[classDefinition.ClassName] = new ClassTreeNode(classDefinition);

        foreach (var child in _nodes.Values)
        foreach (var parent in child.ClassDefinition.ParentClassNames
                     .Where(_nodes.ContainsKey)
                     .Select(pcn => _nodes[pcn]))
        {
            child.AddParent(parent);
            parent.AddChild(child);
        }
    }

    public bool IsSubclass(string instanceClassName, string superClassName)
    {
        return _nodes.TryGetValue(instanceClassName, out var node)
               && node.IsSubclassOf(superClassName);
    }

    public List<string> GetChildren(string className)
    {
        return _nodes.TryGetValue(className, out var node)
            ? node.Children.Select(childNode => childNode.ClassDefinition.ClassName).ToList()
            : [];
    }

    public List<string> GetParents(string className)
    {
        return _nodes.TryGetValue(className, out var node)
            ? node.Parents.Select(parentNode => parentNode.ClassDefinition.ClassName).ToList()
            : [];
    }

    public List<string> GetClassChain(string className, string rootClassName)
    {
        List<string> result = [className];

        if (!_nodes.TryGetValue(className, out var node)) return result;

        foreach (var parent in GetParents(node.ClassDefinition.ClassName).Where(parent => IsSubclass(parent, rootClassName))) result.AddRange(GetClassChain(parent, rootClassName));

        return result;
    }
}