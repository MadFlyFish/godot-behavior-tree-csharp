using System.Diagnostics;
using Godot;
using Godot.Collections;

namespace Reversion.Module.Btree.Base;

public class Btree : Node
{
    [Export] private bool IsActive { set; get; } = true;
    [Export(PropertyHint.Enum, "Idle, Physics")] private int SyncMode{set; get;}
    [Export] private bool Debug { set; get; }
    [Export] private NodePath AgentPath{set; get;}
    [Export] private NodePath BlackboardPath{set; get;}
    [Export] public Node Agent{set; get;}
    [Export] public Blackboard Blackboard{set; get;}
        
    private BtNode BtRoot{set; get;}
    private int LoopCount{set; get;}
        
    public override void _Ready()
    {
        // Agent = GetNode<Node>(_agentPath);
        // Blackboard = GetNode<Blackboard>(_blackboardPath);
        BtRoot = GetChild<BtNode>(0);
            
        System.Diagnostics.Debug.Assert(GetChildCount() == 1, "A Behavior Tree can only have one entry point.");
        BtRoot.PropagateCall("connect", new Array(nameof(BtNode.TreeAborted), this, nameof(OnTreeAborted)));
        Start();
    }
        
    public override async void _Process(float delta)
    {
        if (! IsActive)
        {
            SetProcess(false);
            return;
        }

        var tickTask = BtRoot.Tick(Agent, Blackboard);
        if (!tickTask.IsCompleted)
        {
            SetProcess(false);
            await tickTask;
                
            SetProcess(true);
                
            LoopCount++;
            if (Debug)
            {
                GD.Print("\n");
                GD.Print("*************************** " + Agent.Name + " " + Name + " 主循环次数：" + LoopCount + " ***************************");
                GD.Print("\n");
            }
        }
    }
        
    public override async void _PhysicsProcess(float delta)
    {
        if (! IsActive)
        {
            SetPhysicsProcess(false);
            return;
        }
        
        if (Debug)
        {
            GD.Print(Name);
        }
        
        var tickTask = BtRoot.Tick(Agent, Blackboard);
        if (!tickTask.IsCompleted)
        {
            SetPhysicsProcess(false);
            await tickTask;
            SetPhysicsProcess(true);
                
            LoopCount++;
            if (Debug)
            {
                GD.Print("\n");
                GD.Print("*************************** " + Agent.Name + " " + Name + " 主循环次数：" + LoopCount + " ***************************");
                GD.Print("\n");
            }
        }
    }
        
    private void Start()
    {
        if (!IsActive)
        {
            return;
        }
        
        switch (SyncMode)
        {
            case 0:
                SetPhysicsProcess(false);
                SetProcess(true);
                break;
            case 1:
                SetPhysicsProcess(true);
                SetProcess(false);
                break;
        }
    }
        
    private void OnTreeAborted()
    {
        IsActive = false;
    }
}