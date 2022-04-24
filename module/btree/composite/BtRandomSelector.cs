using System.Threading.Tasks;
using Godot;
using Reversion.Module.Btree.Base;

namespace Reversion.Module.Btree.Composite;

public class BtRandomSelector : BtSelector
{
    protected override  async void _PreTick(Node agent, Blackboard blackboard)
    {
        GD.Randomize();
        Children.Shuffle();
    }
}