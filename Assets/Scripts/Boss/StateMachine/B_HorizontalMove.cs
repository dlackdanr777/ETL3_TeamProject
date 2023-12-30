using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_HorizontalMove : BossStateMachineBehaviour
{
    private float _speed => _boss.MoveSpeed;
    private Rigidbody _rigidbody => _boss.Rigidbody;

    private int _randomMoveDirX;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _randomMoveDirX = Random.Range(-1, 2);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Move(animator);
    }


    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat("Horizontal", 0);
    }


    private void Move(Animator animator)
    {
        animator.SetFloat("Horizontal", _randomMoveDirX);
        _rigidbody.MovePosition(_boss.transform.position + _boss.transform.right * _speed * Time.deltaTime * _randomMoveDirX);
    }
}
