namespace Meteorite;

using System.Numerics;
using System.Collections.ObjectModel;

public class Node
{
    public static Node MainRoot = new() {Name = "Root"};

    public Node Root
    {
        get
        {
            var root = this;

            while (root.Parent != null) root = root.Parent;

            return root;
        }
    }

    public string Name;
    public bool Enabled = true;
	public Node? Parent
	{
		get => _parent;
		set
		{
            if (value == _parent) return;
            
            if (Root == MainRoot) Removed();

            _parent?._children.Remove(this);
			value?._children.Add(this);
            
            _parent = value;

            if (Root == MainRoot) Added();
		}
	}
    public int ChildrenCount => _children.Count;
    public Node[] GetChildren() => _children.ToArray();
    public Node[] GetChildrenRecursive()
    {
        var children = new List<Node>();

        // instead of joining list by calling GetChildrenRecursive() recursively (which causes terrible amount of garbage collections)
        // its better to append to the list using recursing function that takes that list as parameter
        RecurseChildren(children, this);

        return children.ToArray();
    }
    static void RecurseChildren(List<Node> list, Node root)
    {
        for (var i = 0; i < root.ChildrenCount; i++)
        {
            var child = root.GetChild(i);

            list.Add(child);
            RecurseChildren(list, child);
        }
    }

    public Node AddChild(Node child)
    {
	    child.Parent = this;
	    return this;
    }
    public Node AddChildren(params Node[] children)
    {
	    foreach (var child in children) child.Parent = this;
	    return this;
    }

    Node? _parent;
	List<Node> _children = new();

	public Node()
	{
		Name = GetType().Name;
	}
    public Node GetChild(int index) => _children[index];
    public Node? GetChildOrNull(int index)
    {
        if (index < 0 && index >= _children.Count) return null;
        return _children[index];
    }
    /// <summary>
    /// Called when the object is added to the <c>MainRoot</c>.
    /// </summary>
    public virtual void Added() { }
	/// <summary>
	/// Called once after <c>Run()</c> is called and windows initialized.
	/// <c>Start()</c> won't be called for objects that are added during runtime of <c>Game</c>.
	/// </summary>
	public virtual void Start() { }
	/// <summary>
	/// Called with fixed rate (by default, the rate delta is 1 / 60 seconds)
	/// </summary>
	public virtual void Update(float delta) { }
	/// <summary>
	/// Called with rendering rate. If vsync is enabled, it uses monitor refresh rate.
	/// Otherwise it uses same rate as <c>Update()</c>.
	/// </summary>
	public virtual void Render(float delta) { }
	/// <summary>
	/// Called when program is about to close.
	/// </summary>
	public virtual void Close() { }
    /// <summary>
    /// Called when object is removed from <c>MainRoot</c>.
    /// <c>Removed()</c> won't call when program is closing, unless the object was ordered to be removed.
    /// </summary>
    public virtual void Removed() { }

    public virtual void InputHandler(InputEvent ievent) { }
}