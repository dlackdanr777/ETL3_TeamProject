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
        ReduceHorizontalValue(animator);
    }


    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }


    private void Move(Animator animator)
    {
        animator.SetFloat("Vertical", 1);
        _rigidbody.MovePosition(_boss.transform.position + _boss.transform.forward * _speed * Time.deltaTime);
    }

    private void ReduceHorizontalValue(Animator animator)
    {
        float horizontalValue = animator.GetFloat("Horizontal");

        if (0f < horizontalValue)
            horizontalValue -= Time.deltaTime * 3;

        else if (horizontalValue < 0f)
            horizontalValue += Time.deltaTime * 3;

        animator.SetFloat("Horizontal", horizontalValue);

        Debug.Log(horizontalValue);
    }

}
