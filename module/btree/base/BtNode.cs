using Godot;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Reversion.Module.Btree.Base;

/// 全部行为树节点的父类，通常无需直接实例化。比如行为节点，可直接继承BtLeaf类.
public class BtNode : Node
{
    /// （可选）信号：发送Tick结果(True is success, false is failure)
    [Signal] public delegate void Ticked(bool result);
        
    /// 信号：终止整个行为树的运行。
    [Signal] public delegate void TreeAborted();
        
    /// 是否激活（如否，则每次tick都返回false）。
    [Export] private bool IsActive{set; get;} = true;
        
    /// 是否打印debug信息
    [Export] private bool Debug{set; get;}
        
    /// 是否在节点运行完毕后终止行为树的运行。
    [Export] private bool IsAbortTree{set; get;}
        
    /// 节点的三种状态
    public enum BtNodeState { Failure, Success, Running }
        
    /// 节点状态
    public BtNodeState State { private set; get; }
        
    /// 初始化：激活状态判断
    public override void _Ready()
    {
        if (IsActive)
        {
            Succeed();
        }
        else
        {
            Fail();
            GD.Print("禁用节点： '" + Name + "', 路径: '" + GetPath() + "'");
        }
    }
        
    /// 设置State并且返回True或False。
    protected bool SetState(BtNodeState state)
    {
        switch(state)
        {
            case BtNodeState.Success:
                return Succeed();
            case BtNodeState.Failure:
                return Fail();
        };
        System.Diagnostics.Debug.Assert(false, "Invalid BTNodeState assignment. Can only set to success or failure.");
        return false;
    }

    /// 返回State的字符串名称。
    public string GetState()
    {
        if (Succeeded())
        {
            return "success";
        }
        else if (Failed())
        {
            return "failure";
        }
        else
        {
            return "running";
        }
    }

    /// 设置State为成功并返回true。
    protected bool Succeed()
    {
        State = BtNodeState.Success;
        return true;
    }

    /// 设置State为失败并返回false。
    protected bool Fail()
    {
        State = BtNodeState.Failure;
        return false;
    }

    /// 设置State为运行中但不返回任何值。
    private void Run()
    {
        State = BtNodeState.Running;
    }
        
    // 判断是否成功（尽量对外暴露方法，而非直接判断属性或者字段）
    public bool Succeeded()
    {
        return State == BtNodeState.Success;
    }
        
    // 判断是否失败（尽量对外暴露方法，而非直接判断属性或者字段）
    public bool Failed()
    {
        return State == BtNodeState.Failure;
    }
        
    // 判断是否运行中（尽量对外暴露方法，而非直接判断属性或者字段）
    private bool Running()
    {
        return State == BtNodeState.Running;
    }
        
    /// 在_Tick之前执行(在Tick内)。
    protected virtual async void _PreTick(Node agent, Blackboard blackboard)
    {
        await Task.Run(() => { });
    }

    /// _Tick：最主要的行为处理。必须返回succeed() 或 fail()，不能直接设置State；
    protected virtual async Task<bool> _Tick(Node agent, Blackboard blackboard)
    {
        await Task.Run(() => { });
        return Succeeded();
    }

    /// 在_Tick之后执行(在Tick内)。
    protected virtual async void _PostTick(Node agent, Blackboard blackboard, bool result)
    {
        await Task.Run(() => { });
    }
        
    /// Tick：可理解为每个节点完整的执行过程，这里需用异步，因为需要等待完成。(在Tick内)
    public async Task<bool> Tick(Node agent, Blackboard blackboard)
    {
        if (! IsActive) {
            return Fail();
        }

        if (Running())
        {
            return false;
        }
        if (Debug)
        {
            GD.Print("----------------------------------");
            GD.Print(Name + " Tick开始");
        }

        _PreTick(agent, blackboard);
            
        Run();

        var tickTask =  _Tick(agent, blackboard);
        if (!tickTask.IsCompleted)
        {
            System.Diagnostics.Debug.Assert(Running(), "BTNode execution was suspended but it's not running. Did you succeed() or fail() before yield?");
            await tickTask;
        }

        var result = tickTask.Result;

        System.Diagnostics.Debug.Assert(!Running(), "BTNode execution was completed but it's still running. Did you forget to return succeed() or fail()?");
            
        _PostTick(agent, blackboard, result);
            
        EmitSignal(nameof(Ticked), result);

        if (IsAbortTree)
        {
            EmitSignal(nameof(TreeAborted));
        }
            
        if (Debug)
        {
            GD.Print(Name + " Tick结果： " + result);
            GD.Print("----------------------------------");
        }
        return result;
    }
}