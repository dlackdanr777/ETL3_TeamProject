using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollState : State
{
    
    bool grounded;

    float rollDistance = 2.5f;
    float gravityValue;
    Vector3 rollDirection;
    Coroutine _afterRoutine;
    

    public RollState(Character _character, StateMachine _StateMachine, PlayerController _playerController) : base(_character, _StateMachine)
    {
        character = _character;
        stateMachine = _StateMachine;
        playerController = _playerController;
    }

    public override void Enter()
    {
        base.Enter();
        
        gravityValue = character.gravityValue;
        character.animator.SetTrigger("roll");
        gravityVelocity.y = 0f;
        playerController.isHittable = false;
        playerController.moveable = false;

        rollDirection = character.transform.forward.normalized;
        rollDirection.y = 0;
        if (_afterRoutine != null)
        {
            character.StopCoroutine(_afterRoutine);
        }
            
        _afterRoutine = character.StartCoroutine(AfterAnimation());
    }

    public override void HandleInput()
    {
        base.HandleInput();

    }
   
    public override void LogicUpdate()
    {
        base.LogicUpdate();

    }
    

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        playerController.isHittable = false;
        gravityVelocity.y += gravityValue * Time.deltaTime;
        grounded = character.controller.isGrounded;
        
        if (grounded && gravityVelocity.y < 0)
        {
            gravityVelocity.y = 0f;
        }

        if (velocity.sqrMagnitude > 0)
        {
            character.transform.rotation = Quaternion.Slerp(character.transform.rotation, Quaternion.LookRotation(velocity), character.rotationDampTime);
        }


    }
    public override void Exit()
    {
        base.Exit();
    }

    IEnumerator AfterAnimation()
    {
        Vector3 targetRollDirection = rollDirection * rollDistance; // 목표 구르는 방향을 설정합니다.
        float smoothTime = 0.15f; // 구르는 방향을 부드럽게 조정할 시간을 설정합니다.
        Vector3 currentVelocity = Vector3.zero; // 현재 속도를 초기화합니다.

        do
        {
            // 현재 구르는 방향을 목표 구르는 방향으로 부드럽게 이동시킵니다.
            rollDirection = Vector3.SmoothDamp(rollDirection, targetRollDirection, ref currentVelocity, smoothTime);

            character.controller.Move(rollDirection * Time.deltaTime); // 캐릭터를 이동시킵니다.

            yield return YieldCache.WaitForSeconds(0.001f); // 잠시 대기합니다.
        }
        while (character.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);

        stateMachine.ChangeState(character.standing); // 구르는 애니메이션이 끝나면 캐릭터의 상태를 변경합니다.

        if (_afterRoutine != null)
            character.StopCoroutine(_afterRoutine);
    }
}
