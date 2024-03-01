using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollState : State
{
    bool rolling;
    bool grounded;

    float rollDistance = 100f;
    float gravityValue;
    float playerSpeed;
    Vector3 currentVelocity;

    Coroutine _afterRoutine;
    Vector3 cVelocity;

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
        velocity = Vector3.zero;
        currentVelocity = Vector3.zero;
        gravityVelocity.y = 0f;
        playerSpeed = character.playerSpeed;
        playerController.isHittable = false;

        
        if(_afterRoutine != null)
        {
            character.StopCoroutine(_afterRoutine);
        }
            
        _afterRoutine = character.StartCoroutine(AfterAnimation());
    }

    public override void HandleInput()
    {
        base.HandleInput();
        input = moveAction.ReadValue<Vector2>();
        velocity = new Vector3(input.x, 0, input.y);

        velocity = velocity.x * character.cameraTransform.right.normalized + velocity.z * character.cameraTransform.forward.normalized;
        velocity.y = 0f;
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        //playerController.isHittable = false;
        //if (grounded)
        //{
        //    stateMachine.ChangeState(character.standing);
        //}
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
        currentVelocity = Vector3.SmoothDamp(currentVelocity, velocity, ref cVelocity, character.velocityDampTime);
       // character.controller.Move(currentVelocity * Time.deltaTime * playerSpeed + gravityVelocity * Time.deltaTime*1000);

        if (velocity.sqrMagnitude > 0)
        {
            character.transform.rotation = Quaternion.Slerp(character.transform.rotation, Quaternion.LookRotation(velocity), character.rotationDampTime);
        }


    }
    public override void Exit()
    {
        base.Exit();
       // playerController.isHittable = true;
    }

    IEnumerator AfterAnimation()
    {
        //character.animator.Play("");
        do
        {
            character.controller.Move(currentVelocity*rollDistance*Time.deltaTime);
            yield return YieldCache.WaitForSeconds(0.1f);
        }
        while (character.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.2f);
       
        
        stateMachine.ChangeState(character.standing);

        if(_afterRoutine != null)
            character.StopCoroutine(_afterRoutine);
        
    }
}
