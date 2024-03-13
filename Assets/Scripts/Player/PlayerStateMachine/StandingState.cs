using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class StandingState : State
{
    
    float gravityValue;
    bool jump;
    Vector3 currentVelocity;
    bool grounded;
    bool sprint;
    bool roll;
    bool skill;
    float playerSpeed;
    bool attack;

    Vector3 cVelocity;

    public StandingState(Character _character, StateMachine _StateMachine,PlayerController _playerController) : base(_character, _StateMachine)
    {
        character = _character;
        stateMachine = _StateMachine;
        playerController = _playerController;
    }

    public override void Enter()
    {
        base.Enter();

        jump = false;
        sprint = false;
        roll = false;
        attack = false;
        skill = false;
        input = Vector2.zero;
        velocity = Vector3.zero;
        currentVelocity = Vector3.zero;
        gravityVelocity.y= 0f;
        playerController.isHittable = true;
        playerController.moveable = true;
        playerSpeed =character.playerSpeed;
        grounded = character.controller.isGrounded;
        gravityValue = character.gravityValue;

        character.ChangeState(CharacterState.IdleAndMove);
    }
    public override void HandleInput()
    {
        base.HandleInput();

        if (jumpAction.triggered)
        {
            jump = true;
        }
        if(sprintAction.triggered)
        {
            sprint = true;
        }
        if (attackAction.triggered&&playerController.Sta>playerController.attackStamina)
        {
            playerController.DepleteSta(playerController.Sta, playerController.attackStamina);
            attack = true;
        }
        if (rollAction.triggered && playerController.Sta > playerController.rollStamina)
        {
            playerController.DepleteSta(playerController.Sta, playerController.rollStamina);
            roll = true;
        }
        if (skillAction.triggered && playerController.Sta > playerController.skillStamina)
        {
            skill = true;
        }
        
        input = moveAction.ReadValue<Vector2>();
        velocity=new Vector3(input.x,0,input.y);

        velocity = velocity.x * character.cameraTransform.right.normalized + velocity.z * character.cameraTransform.forward.normalized;
        velocity.y = 0f;

        
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        character.animator.SetFloat("speed", input.magnitude,character.speedDampTime,Time.deltaTime);

        if (sprint)
        {
            stateMachine.ChangeState(character.sprinting);
        }
        if (jump)
        {
            stateMachine.ChangeState(character.jumping);
        }
        if (attack)
        {
            stateMachine.ChangeState(character.attacking);
        }
        if (roll)
        {
            stateMachine.ChangeState(character.rolling);
        }
        if(skill)
        {
            stateMachine.ChangeState(character.skilling);
        }
        
        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        gravityVelocity.y += gravityValue * Time.deltaTime;
        grounded = character.controller.isGrounded;

        if (grounded && gravityVelocity.y < 0)
        {
            gravityVelocity.y = 0f;
        }
        currentVelocity = Vector3.SmoothDamp(currentVelocity, velocity, ref cVelocity, character.velocityDampTime);
        if (playerController.moveable)
        {
            character.controller.Move(currentVelocity * Time.deltaTime * playerSpeed + gravityVelocity * Time.deltaTime);
        }
            

        if (velocity.sqrMagnitude > 0)
        {
            character.transform.rotation = Quaternion.Slerp(character.transform.rotation, Quaternion.LookRotation(velocity), character.rotationDampTime);
        }

    }

    public override void Exit()
    {
        base.Exit();

        gravityVelocity.y = 0f;
        character.playerVelocity = new Vector3(input.x, 0, input.y);

        if(velocity.sqrMagnitude > 0)
        {
            character.transform.rotation = Quaternion.LookRotation(velocity);
        }
    }
}
