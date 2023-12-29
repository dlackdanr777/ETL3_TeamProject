using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_TrackingState : BossStateMachineBehaviour
{
    private float _speed => _boss.MoveSpeed;
    private Rigidbody _rigidbody => _boss.Rigidbody;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {



    }


    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Rotate();
        Move(animator);
    }


    private void Move(Animator animator)
    {
        animator.SetFloat("Vertical", 1);
        _rigidbody.MovePosition(_boss.transform.position + _boss.transform.forward * _speed * Time.deltaTime);
    }

    private void Rotate()
    {
        Vector3 targetDir = (_boss.Target.transform.position - _boss.gameObject.transform.position).normalized;
        targetDir.y = 0;

        Quaternion rot = Quaternion.LookRotation(targetDir);
        _boss.transform.rotation = Quaternion.Lerp(_boss.transform.rotation, rot, _boss.RotateSpeed * Time.deltaTime);
    }
}
