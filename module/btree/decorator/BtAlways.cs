using System.Threading.Tasks;
using Godot;
using Reversion.Module.Btree.Base;

namespace Reversion.Module.Btree.Decorator;

public class BtAlways : BtDecorator
{
    [Export(PropertyHint.Enum, "Fail, Succeed")] private int AlwaysWhat{set; get;}
        
    protected override async Task<bool> _Tick(Node agent, Blackboard blackboard)
    {
        var tickResult = BtChild.Tick(agent, blackboard);
        if (!tickResult.IsCompleted)
        {
            await tickResult;
        }
            
        return AlwaysWhat ==  0 ? Fail() : Succeed();
    }
}