using System.Threading.Tasks;
using Godot;
using Reversion.Module.Btree.Base;

namespace Reversion.Module.Btree.Composite;

public class BtSequence : BtComposite
{
    protected override  async Task<bool> _Tick(Node agent, Blackboard blackboard)
    {
        foreach (var child in Children)
        {
            BtChild = child as BtNode;
            if (BtChild == null) continue;
                
            var tickTask = BtChild.Tick(agent, blackboard);
                    
            if (!tickTask.IsCompleted)
            {
                await tickTask;
            }
                    
            if (BtChild.Failed())
            {
                return Fail();
            }
        }
            
        return Succeed();
    }
}