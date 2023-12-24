namespace SatisfactoryDocsExporter.Game.Headers;

public class ClassTreeNode(ClassDefinition classDefinition)
{
    private readonly List<ClassTreeNode> _children = [];
    private readonly List<ClassTreeNode> _parents = [];

    public ClassDefinition ClassDefinition { get; } = classDefinition;

    public IReadOnlyList<ClassTreeNode> Children => _children;
    public IReadOnlyList<ClassTreeNode> Parents => _parents;

    public void AddChild(ClassTreeNode node)
    {
        _children.Add(node);
    }

    public void AddParent(ClassTreeNode node)
    {
        _parents.Add(node);
    }

    public bool IsSubclassOf(string className)
    {
        return ClassDefinition.ClassName == className
               || _parents.Any(parent => parent.IsSubclassOf(className));
    }
}