using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : State
{
    bool grounded;

    float gravityValue;
    float jumpHeight;
    float playerSpeed;

    Vector3 airVelocity;
    public JumpingState(Character _character, StateMachine _StateMachine) : base(_character, _StateMachine)
    {
        character = _character;
        stateMachine = _StateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        grounded = false;
        gravityValue = character.gravityValue;
        jumpHeight = character.jumpHeight;
        playerSpeed = character.playerSpeed;
        gravityVelocity.y = 0;

        character.animator.SetFloat("speed", 0);
        character.animator.SetTrigger("jump");
        Jump();
    }
    public override void HandleInput()
    {
        base.HandleInput();
        input = moveAction.ReadValue<Vector2>();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (grounded)
        {
            stateMachine.ChangeState(character.landing);
        }
    }


    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if(!grounded)
        {
            velocity = character.playerVelocity;
            airVelocity = new Vector3(input.x, 0, input.y);

            velocity = velocity.x * character.cameraTransform.right.normalized + velocity.z * character.cameraTransform.forward.normalized;
            velocity.y = 0f;
            airVelocity = airVelocity.x * character.cameraTransform.right.normalized + airVelocity.z * character.cameraTransform.forward.normalized;
            airVelocity.y = 0f;
            character.controller.Move(gravityVelocity*Time.deltaTime+(airVelocity*character.airControl+velocity*(1-character.airControl)*playerSpeed*Time.deltaTime));
        }
        gravityVelocity.y += gravityValue * Time.deltaTime;
        grounded = character.controller.isGrounded;

    }

    public override void Exit()
    {
        base.Exit();
    }

    void Jump()
    {
        gravityVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
    }
}
