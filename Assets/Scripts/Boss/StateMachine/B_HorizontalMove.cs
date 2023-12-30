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
        ReduceVerticalValue(animator);
    }


    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }


    private void Move(Animator animator)
    {
        animator.SetFloat("Horizontal", _randomMoveDirX);
        _rigidbody.MovePosition(_boss.transform.position + _boss.transform.right * _speed * Time.deltaTime * _randomMoveDirX);
    }


    private void ReduceVerticalValue(Animator animator)
    {
        float verticalValue = animator.GetFloat("Vertical");

        if (0f < verticalValue)
            verticalValue -= Time.deltaTime * 3;

        animator.SetFloat("Vertical", verticalValue);

        Debug.Log(verticalValue);
    }
}
