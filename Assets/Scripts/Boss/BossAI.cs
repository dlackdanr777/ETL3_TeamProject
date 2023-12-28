using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>������ ���� ����</summary>
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

        // TODO : �Ʒ��� ����, ��� ��� �־����
        nodes.Add(TrackingNode());

        SelectorNode actionNode = new SelectorNode(nodes);

        return new ConditionNode(_boss.CheckWaitTime, actionNode);
    }

    private INode TrackingNode()
    {
        List<INode> nodes = new List<INode>();
        nodes.Add(new ActionNode(Tracking));

        return new SequenceNode(nodes);
    }

    private INode.ENodeState Tracking()
    {     
        _boss.ChangeAiState(BossAIState.Tracking);
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
