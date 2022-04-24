using System.Threading.Tasks;
using Godot;
using Reversion.Module.Btree.Base;

namespace Reversion.Module.Btree.Leaf;

public class BtSetColor : BtLeaf
{
    [Export] public string NewColor{set; get;} = "White";
        
    protected override  async Task<bool> _Tick(Node agent, Blackboard blackboard)
    {
        await Task.Run(() => SetColor(agent, blackboard));
        return Succeed();
    }

    private  void SetColor(Node agent, Blackboard blackboard)
    {
        var sprite = agent.GetNode<Sprite>("Sprite");
        sprite.Modulate = Color.ColorN(NewColor);
    }
}