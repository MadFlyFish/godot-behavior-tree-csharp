using System.Threading.Tasks;
using Godot;
using Reversion.Module.Btree.Base;

namespace Reversion.Module.Btree.Decorator.Conditional;

public class BtProbability : BtConditional
{
    [Export] private float Probability{set; get;} = 1.0f;

    protected override  async void _PreTick(Node agent, Blackboard blackboard)
    {
        GD.Randomize();
        Verified = GD.Randf() <= Probability;
    }
}