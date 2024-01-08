using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_LoopAnimation : BossStateMachineBehaviour
{
    [Tooltip("���� ���·� �����ϱ� ���� Ʈ���� �̸�")]
    [SerializeField] private string _triggerName;

    [Tooltip("�ִϸ��̼� �ݺ��� ������ ���� Ÿ�ٰ��� �Ÿ�")]
    [SerializeField] private float _isEndLoopDistance = 1f;


    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_boss.TargetDistance <= _isEndLoopDistance)
        {
            animator.SetTrigger(_triggerName); 
        }
    }
}
