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
        Vector3 targetRollDirection = rollDirection * rollDistance; // ��ǥ ������ ������ �����մϴ�.
        float smoothTime = 0.15f; // ������ ������ �ε巴�� ������ �ð��� �����մϴ�.
        Vector3 currentVelocity = Vector3.zero; // ���� �ӵ��� �ʱ�ȭ�մϴ�.

        do
        {
            // ���� ������ ������ ��ǥ ������ �������� �ε巴�� �̵���ŵ�ϴ�.
            rollDirection = Vector3.SmoothDamp(rollDirection, targetRollDirection, ref currentVelocity, smoothTime);

            character.controller.Move(rollDirection * Time.deltaTime); // ĳ���͸� �̵���ŵ�ϴ�.

            yield return YieldCache.WaitForSeconds(0.001f); // ��� ����մϴ�.
        }
        while (character.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);

        stateMachine.ChangeState(character.standing); // ������ �ִϸ��̼��� ������ ĳ������ ���¸� �����մϴ�.

        if (_afterRoutine != null)
            character.StopCoroutine(_afterRoutine);
    }
}
