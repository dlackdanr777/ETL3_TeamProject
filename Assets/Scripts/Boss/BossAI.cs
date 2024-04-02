using System.Collections.Generic;
using UnityEngine;



public class BossAI
{
    private BehaviorTree _bt;

    private BossController _boss;

    private BossAttackData _currentAttackData;


    public BossAI(BossController boss)
    {
        _boss = boss;
        //_boss.OnHpDepleted += ChangeGuardState;
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
        nodes.Add(IdleNode());

        SelectorNode rootNode = new SelectorNode(nodes);

        _bt = new BehaviorTree(rootNode);
    }


    private INode WaitConditionNode()
    {
        List<INode> nodes = new List<INode>();

        // TODO : 아래에 공격, 방어 등등 넣어야함
        nodes.Add(TrackingNode());
        nodes.Add(ReconnaissanceNode());
        nodes.Add(AttackNode());

        SelectorNode actionNode = new SelectorNode(nodes);

        return new ConditionNode(_boss.CheckStateChangeEnabled, actionNode);
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
        _currentAttackData = _boss.GetPossibleAttackData();
        if (_currentAttackData == null)
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
        nodes.Add(new ActionNode(CheckReconnaissance));
        nodes.Add(new ActionNode(Reconnaissance));

        return new SequenceNode(nodes);
    }

    private INode.ENodeState CheckReconnaissance()
    {
        if (!_boss.CheckAttackWaitTime())
            return INode.ENodeState.Success;

        return INode.ENodeState.Failure;
    }


    private INode.ENodeState Reconnaissance()
    {
        _boss.ChangeAiState(BossAIState.Reconnaissance);
            _boss.DepleteAttackTime(_boss.AIUpdateTime);

        return INode.ENodeState.Success;
    }


    private INode AttackNode()
    {
        List<INode> nodes = new List<INode>();
        nodes.Add(new ActionNode(Attack));

        return new SequenceNode(nodes);
    }

    private INode.ENodeState Attack()
    {
        _boss.ChangeAiState(BossAIState.Attack);
        _boss.SetAnimatorAttackValue(_currentAttackData.AttackState);
        _boss.SetAttackTime();
        _boss.FindTarget(3);
        return INode.ENodeState.Success;
    }

    private INode IdleNode()
    {
        List<INode> nodes = new List<INode>();

        nodes.Add(new ActionNode(Idle));

        SelectorNode actionNode = new SelectorNode(nodes);

        return new ConditionNode(() => { return _boss.Target == null ? true : false; }, actionNode);
    }

    private INode.ENodeState Idle()
    {
        _boss.ChangeAiState(BossAIState.Idle);
        return INode.ENodeState.Success;
    }


    private void ChangeGuardState(object subject, float value)
    {
        int randInt = Random.Range(0, 4);
        bool isEnabled = randInt == 0 && _boss.GetAIState() != BossAIState.Attack;
        if(isEnabled)
        {
            float guardTime = Random.Range(0, 3f);
            _boss.ChangeAiState(BossAIState.Guard);
            _boss.SetWaitTime(guardTime);
        }
    }

}
