using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;


/// <summary> 조건문을 확인 후 true일 경우 하위 노드 실행, false일 경우 Failure반환</summary>
public class ConditionNode : INode
{
    private INode _node;

    private Func<bool> _condition;

    public ConditionNode(Func<bool> condition, INode node)
    {
        _condition = condition;
        _node = node;
    }


    public INode.ENodeState Evaluate()
    {         
        bool conditionResult = _condition.Invoke();
        return conditionResult ? _node.Evaluate() : INode.ENodeState.Failure;
    }
}
