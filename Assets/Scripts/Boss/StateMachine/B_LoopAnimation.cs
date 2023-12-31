using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_LoopAnimation : BossStateMachineBehaviour
{
    [Tooltip("다음 상태로 전이하기 위한 트리거 이름")]
    [SerializeField] private string _triggerName;

    [Tooltip("애니메이션 반복을 끝내기 위한 타겟과의 거리")]
    [SerializeField] private float _isEndLoopDistance = 1f;


    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_boss.TargetDistance <= _isEndLoopDistance)
        {
            animator.SetTrigger(_triggerName); 
        }
    }
}
