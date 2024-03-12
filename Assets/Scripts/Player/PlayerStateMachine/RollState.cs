using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollState : State
{
    
    bool grounded;

    float rollDistance = 10f;
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
        character.animator.applyRootMotion = true;
        gravityValue = character.gravityValue;
        character.animator.SetTrigger("roll");
        gravityVelocity.y = 0f;
        playerController.isHittable = false;
        playerController.moveable = false;


        input = moveAction.ReadValue<Vector2>();
        velocity = new Vector3(input.x, 0, input.y);

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
        character.animator.applyRootMotion = false;

        if (_afterRoutine != null)
            character.StopCoroutine(_afterRoutine);
    }

    IEnumerator AfterAnimation()
    {
        yield return YieldCache.WaitForSeconds(1.3f * 0.8f);
        stateMachine.ChangeState(character.standing); // 구르는 애니메이션이 끝나면 캐릭터의 상태를 변경합니다.   
    }
}
