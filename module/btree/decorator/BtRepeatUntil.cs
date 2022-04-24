using System.Threading.Tasks;
using Godot;
using Reversion.Module.Btree.Base;

namespace Reversion.Module.Btree.Decorator;

public class BtRepeatUntil : BtDecorator
{
    [Export(PropertyHint.Enum, "Fail, Succeed")] private int AlwaysWhat{set; get;}
    [Export] private int Frequency{set; get;}
    private bool ExpectedResult{set; get;}

    public override void _Ready()
    {
        ExpectedResult = AlwaysWhat != 0;
    }

    protected override async Task<bool> _Tick(Node agent, Blackboard blackboard)
    {
        var result = !ExpectedResult;
        while (result != ExpectedResult)
        {
            var tickTask = BtChild.Tick(agent, blackboard);
            if (!tickTask.IsCompleted)
            {
                await tickTask;
            }
            result = tickTask.Result;

            await ToSignal(GetTree().CreateTimer(Frequency), "timeout");
        }
            
        return SetState(BtChild.State);
    }
}