using System.Diagnostics;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;

namespace Reversion.Module.Btree.Base;

public class BtComposite : BtNode
{
    protected Array Children{ private set; get;}
    protected BtNode BtChild{set; get;}
        
    public override void _Ready()
    {
        Children = GetChildren();
        Debug.Assert(GetChildCount() >1, "A BTComposite must have more than one child.");
    }

    protected override  async Task<bool> _Tick(Node agent, Blackboard blackboard)
    {
        Task<bool> tickTask = null;

        foreach (var child in Children)
        {
            BtChild = child as BtNode;
            if (BtChild != null) tickTask = BtChild.Tick(agent, blackboard);
            if (tickTask != null && !tickTask.IsCompleted)
            {
                await tickTask;
            }
        }
        return Succeed();
    }
}