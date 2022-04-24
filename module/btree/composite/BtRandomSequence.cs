using Godot;
using Reversion.Module.Btree.Base;
using Reversion.Module.Btree.Composite;

namespace Reversion.module.Btree.Composite;

public class BtRandomSelector : BtSequence
{
    protected override  async void _PreTick(Node agent, Blackboard blackboard)
    {
        GD.Randomize();
        Children.Shuffle();
    }
}