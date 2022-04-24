using System.Threading.Tasks;
using Godot;
using Reversion.Module.Btree.Base;

namespace Reversion.Module.Btree.Leaf;

public class BtWait : BtLeaf
{
    [Export] private float WaitTime{set; get;}

    protected override async Task<bool> _Tick(Node agent, Blackboard blackboard)
    {
        await ToSignal(GetTree().CreateTimer(WaitTime), "timeout");
        return Succeed();
    }
}