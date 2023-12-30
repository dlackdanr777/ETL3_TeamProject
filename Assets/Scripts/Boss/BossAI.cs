using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>보스의 현재 상태</summary>
public enum BossAIState
{
    Idle,
    Tracking,
    Backstep,
    Reconnaissance,
    Attack,
    Guard,
}


public class BossAI
{
    private BehaviorTree _bt;

    private BossController _boss;

    public BossAI(BossController boss)
    {
        _boss = boss;
        NodeInit();
    }


    public void AIUpdate()
    {
        _bt.Operate();
    }


    private void NodeInit()
    {
        List<INode> nodes = new List<INode>();

        nodes.Add(WaitConditionNode());
        nodes.Add(new ActionNode(Idle));

        SelectorNode rootNode = new SelectorNode(nodes);

        _bt = new BehaviorTree(rootNode);
    }


    private INode WaitConditionNode()
    {
        List<INode> nodes = new List<INode>();

        // TODO : 아래에 공격, 방어 등등 넣어야함
        nodes.Add(TrackingNode());
        nodes.Add(ReconnaissanceNode());

        SelectorNode actionNode = new SelectorNode(nodes);

        return new ConditionNode(_boss.CheckWaitTime, actionNode);
    }

    private INode TrackingNode()
    {
        List<INode> nodes = new List<INode>();
        nodes.Add(new ActionNode(CheckTracking));
        nodes.Add(new ActionNode(Tracking));

        return new SequenceNode(nodes);
    }

    private INode.ENodeState CheckTracking()
    {
        if(5 < Vector3.Distance(_boss.transform.position, _boss.Target.transform.position))
        {
            return INode.ENodeState.Success;
        }

        return INode.ENodeState.Failure;
    }

    private INode.ENodeState Tracking()
    {     
        _boss.ChangeAiState(BossAIState.Tracking);
        return INode.ENodeState.Success;
    }

    private INode ReconnaissanceNode()
    {
        List<INode> nodes = new List<INode>();
        nodes.Add(new ActionNode(Reconnaissance));

        return new SequenceNode(nodes);
    }


    private INode.ENodeState Reconnaissance()
    {
        _boss.ChangeAiState(BossAIState.Reconnaissance);
        return INode.ENodeState.Success;
    }



    private INode.ENodeState Idle()
    {
        _boss.ChangeAiState(BossAIState.Idle);
        return INode.ENodeState.Success;
    }

    private INode.ENodeState SetWaitTime()
    {
        _boss.SetWaitTime();
        return INode.ENodeState.Failure;
    }

}
