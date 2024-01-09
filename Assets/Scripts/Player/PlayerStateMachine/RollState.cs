using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollState : State
{
    bool rolling;
    bool grounded;

    float gravityValue;
    float playerSpeed;
    Vector3 currentVelocity;
    public RollState(Character _character, StateMachine _StateMachine) : base(_character, _StateMachine)
    {
        character = _character;
        stateMachine = _StateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        gravityValue = character.gravityValue;
        character.animator.SetTrigger("roll");
        velocity = Vector3.zero;
        currentVelocity = Vector3.zero;
        gravityVelocity.y = 0f;

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
            stateMachine.ChangeState(character.standing);
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

        character.controller.Move(currentVelocity * Time.deltaTime * playerSpeed + gravityVelocity * Time.deltaTime);

        if (velocity.sqrMagnitude > 0)
        {
            character.transform.rotation = Quaternion.Slerp(character.transform.rotation, Quaternion.LookRotation(velocity), character.rotationDampTime);
        }


    }



}
