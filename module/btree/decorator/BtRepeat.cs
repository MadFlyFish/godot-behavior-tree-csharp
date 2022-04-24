using System.Threading.Tasks;
using Godot;
using Reversion.Module.Btree.Base;

namespace Reversion.Module.Btree.Decorator;

public class BtRepeat : BtDecorator
{
    [Export] private int TimesToRepeat{set; get;} = 1;
    [Export] private int Frequency{set; get;}

    protected override  async Task<bool> _Tick(Node agent, Blackboard blackboard)
    {
        for (var i = 0; i < TimesToRepeat; i++)
        {
            var tickTask = BtChild.Tick(agent, blackboard);
            if (!tickTask.IsCompleted)
            {
                await tickTask;
            }

            await ToSignal(GetTree().CreateTimer(Frequency), "timeout");
        }
            
        return SetState(BtChild.State);
    }
}