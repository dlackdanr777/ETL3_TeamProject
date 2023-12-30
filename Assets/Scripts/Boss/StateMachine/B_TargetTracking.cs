using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_TargetTracking : BossStateMachineBehaviour
{
    private float _speed => _boss.MoveSpeed;
    private Rigidbody _rigidbody => _boss.Rigidbody;


    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Move(animator);
    }


    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat("Vertical", 0);
    }


    private void Move(Animator animator)
    {
        animator.SetFloat("Vertical", 1);
        _rigidbody.MovePosition(_boss.transform.position + _boss.transform.forward * _speed * Time.deltaTime);
    }
}
