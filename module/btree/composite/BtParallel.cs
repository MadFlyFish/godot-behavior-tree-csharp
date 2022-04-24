using System.Threading.Tasks;
using Godot;
using Reversion.Module.Btree.Base;

namespace Reversion.Module.Btree.Composite;

public class BtParallel : BtComposite
{
    protected override  async Task<bool> _Tick(Node agent, Blackboard blackboard)
    {
        foreach (var child in Children)
        {
            BtChild = child as BtNode;
            BtChild?.Tick(agent, blackboard);
        }
            
        return Succeed();
    }
}