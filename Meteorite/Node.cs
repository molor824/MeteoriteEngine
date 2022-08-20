namespace Meteorite;

using System.Numerics;
using System.Collections.ObjectModel;

public class Node
{
	public string Name;
	public bool Enabled = true;
	public Game Game = null!;
	public Node? Parent
	{
		get => _parent;
		set
		{
			_parent?._children.Remove(this);
			value?._children.Add(this);

			_parent = value;
		}
	}
	public ReadOnlyCollection<Node> Children => _children.AsReadOnly();

	Node? _parent;
	List<Node> _children = new();

	public Node()
	{
		Name = GetType().Name;
	}

	/// <summary>
	/// Called when the object is added.
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
	/// Called when object is removed.
	/// <c>Removed()</c> won't call when program is closing, unless the object was ordered to be removed.
	/// </summary>
	public virtual void Removed() { }
}