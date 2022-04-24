using System.Threading.Tasks;
using Godot;
using Reversion.Module.Btree.Base;

namespace Reversion.Module.Btree.Decorator;

public class BtConditional : BtDecorator
{
    [Export] private bool Reverse { set; get; }

    protected bool Verified{set; get;}
    private bool IgnoreReverse{set; get;} = false;
        
    protected override async void _PreTick(Node agent, Blackboard blackboard)
    {
        Verified = true;
        await Task.Run(() => { });
    }
        
    protected override async Task<bool> _Tick(Node agent, Blackboard blackboard)
    {
        if (Reverse && !IgnoreReverse)
        {
            Verified = !Verified;
        }

        if (!Verified) return Fail();
            
        var tickTask  = base._Tick(agent, blackboard);
        if (!tickTask.IsCompleted)
        {
            await tickTask;
        }
        return tickTask.Result ? Succeed() : Fail();
    }
        
    protected override async void _PostTick(Node agent, Blackboard blackboard, bool result)
    {
        await Task.Run(() => { });
    }
}