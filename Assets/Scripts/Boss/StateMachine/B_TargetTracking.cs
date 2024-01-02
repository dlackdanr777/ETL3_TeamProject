using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_TargetTracking : BossStateMachineBehaviour
{
    [SerializeField] private float _speedMul = 1;
    private float _speed => _boss.MoveSpeed;
    private Rigidbody _rigidbody => _boss.Rigidbody;

    private float _verticalMove;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _verticalMove = 0;
    }

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
        if (_verticalMove < 1)
            _verticalMove += Time.deltaTime * _speed;

        if (1 <= _verticalMove)
            _verticalMove = 1;

        animator.SetFloat("Vertical", _verticalMove);

        _rigidbody.MovePosition(_boss.transform.position + _boss.transform.forward * _speed * _verticalMove * Time.deltaTime * _speedMul);
    }

    private void ReduceHorizontalValue(Animator animator)
    {
        float horizontalValue = animator.GetFloat("Horizontal");

        if (0f < horizontalValue)
            horizontalValue -= Time.deltaTime * 3;

        else if (horizontalValue < 0f)
            horizontalValue += Time.deltaTime * 3;


        animator.SetFloat("Horizontal", horizontalValue);

    }

}
