using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_TargetRotate : BossStateMachineBehaviour
{
    [SerializeField] private float _rotateSpeedMul;

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Rotate();
    }


    private void Rotate()
    {
        Vector3 targetDir = (_boss.Target.transform.position - _boss.gameObject.transform.position).normalized;
        targetDir.y = 0;

        Quaternion rot = Quaternion.LookRotation(targetDir);
        _boss.transform.rotation = Quaternion.Lerp(_boss.transform.rotation, rot, _boss.RotateSpeed * _rotateSpeedMul * Time.deltaTime);
    }
}
