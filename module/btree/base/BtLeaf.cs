using System.Diagnostics;
using Godot;

namespace Reversion.Module.Btree.Base;

public class BtLeaf : BtNode
{
    public override void _Ready()
    {
        Debug.Assert(GetChildCount() ==0,"A BTLeaf cannot have children.");
    }
}