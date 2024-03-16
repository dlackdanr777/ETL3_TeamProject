using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_LoopAnimation : BossStateMachineBehaviour
{
    [Tooltip("���� ���·� �����ϱ� ���� �Ҹ��� �̸�")]
    [SerializeField] private string _boolName;

    [Tooltip("�ִϸ��̼� �ݺ��� ������ ���� Ÿ�ٰ��� �Ÿ�")]
    [SerializeField] private float _isEndLoopDistance = 1f;


    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_boss.TargetDistance <= _isEndLoopDistance)
        {
            animator.SetBool(_boolName, true);
        }
    }


    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(_boolName, false);
    }
}
