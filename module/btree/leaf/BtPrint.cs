using System.Threading.Tasks;
using Godot;
using Reversion.Module.Btree.Base;

namespace Reversion.Module.Btree.Leaf;

public class BtPrint : BtLeaf
{
    protected override  async Task<bool> _Tick(Node agent, Blackboard blackboard)
    {
        await Task.Run(() => GD.Print(agent.Name));
        return Succeed();
    }

    private void Print(Node agent, Blackboard blackboard)
    {
        GD.Print(agent.Name);
    }
}