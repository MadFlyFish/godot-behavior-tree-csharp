using Godot;
using Godot.Collections;
using System.Diagnostics;

namespace Reversion.Module.Btree.Base;

public class Blackboard : Node
{
    [Export] public Dictionary Data{set; get;}

    public override void _EnterTree()
    {
        Data = Data.Duplicate();
    }

    public override void _Ready()
    {
        foreach (var key in Data)
        {
            Debug.Assert(key is string, "Blackboard keys must be stored as strings.");
        }
    }

    public void SetData(string key, object value)
    {
        Data.Add(key, value);
    }
       
    public object GetData(string key)
    {
        if (!Data.Contains(key)) return null;
        var value = Data[key];
        if (value is not NodePath nodePath) return value;
        if (!nodePath.IsEmpty() && GetTree().Root.HasNode(nodePath)) return GetNode(nodePath);
        Data[key] = null;
        return null;
    }

    public bool HasData(string key)
    {
        return Data.Contains(key);
    }
}