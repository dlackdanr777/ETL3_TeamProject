using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SprintState : State
{
    float gravityValue;
    Vector3 currentVelocity;

    bool grounded;
    bool sprint;
    float playerSpeed;
    bool sprintJump;
    Vector3 cVelocity;

    public SprintState(Character _character, StateMachine _StateMachine,PlayerController _playerController) : base(_character, _StateMachine)
    {
        character = _character;    
        stateMachine = _StateMachine;
        playerController = _playerController;
    }
    public override void Enter()
    {
        base.Enter();

        sprint = false;
        sprintJump = false;
        input = Vector2.zero;
        velocity = Vector3.zero;
        currentVelocity = Vector3.zero;
        gravityVelocity.y = 0;

        playerSpeed = character.sprintSpeed;
        grounded = character.controller.isGrounded;
        gravityValue = character.gravityValue;
    }

    public override void HandleInput()
    {
        base.Enter();
        input = moveAction.ReadValue<Vector2>();
        velocity = new Vector3(input.x, 0, input.y);

        velocity = velocity.x * character.cameraTransform.right.normalized + velocity.z * character.cameraTransform.forward.normalized;
        velocity.y = 0f;
        if (sprintAction.triggered || input.sqrMagnitude == 0f)
        {
            sprint = false;
        }
        else
        {
            sprint = true;
        }
        if (jumpAction.triggered)
        {
            sprintJump = true;
        }
    }

    public override void LogicUpdate()
    {
        if(sprint&&playerController.Sta>playerController.runStamina)
        {
            character.animator.SetFloat("speed", input.magnitude + 0.5f, character.speedDampTime, Time.deltaTime);
        }
        else
        {
            stateMachine.ChangeState(character.standing);
        }
        if (sprintJump)
        {
            stateMachine.ChangeState(character.sprintjumping);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        playerController.DepleteSta(playerController.Sta, playerController.runStamina);
        gravityVelocity.y += gravityValue * Time.deltaTime;
        grounded = character.controller.isGrounded;
        if (grounded && gravityVelocity.y < 0)
        {
            gravityVelocity.y = 0f;
        }
        currentVelocity = Vector3.SmoothDamp(currentVelocity, velocity, ref cVelocity, character.velocityDampTime);
 
        character.controller.Move(currentVelocity * Time.deltaTime * playerSpeed + gravityVelocity * Time.deltaTime);
 
 
        if (velocity.sqrMagnitude > 0)
        {
            character.transform.rotation = Quaternion.Slerp(character.transform.rotation, Quaternion.LookRotation(velocity), character.rotationDampTime);
        }
    }

}
