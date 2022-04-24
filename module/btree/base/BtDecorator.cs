using System.Diagnostics;
using System.Threading.Tasks;
using Godot;

namespace Reversion.Module.Btree.Base;

public class BtDecorator : BtNode
{
    protected BtNode BtChild{ private set; get;}
        
    public override void _Ready()
    {
        Debug.Assert(GetChildCount() == 1, "A BTDecorator can only have one child.");
        BtChild = GetChild<BtNode>(0);
    }

    protected override  async Task<bool> _Tick(Node agent, Blackboard blackboard)
    {
        var tickTask = BtChild.Tick(agent, blackboard);
        if (!tickTask.IsCompleted)
        {
            await tickTask;
        }
        return SetState(BtChild.State);
    }
}