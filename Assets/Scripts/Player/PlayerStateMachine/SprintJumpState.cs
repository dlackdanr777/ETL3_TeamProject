using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintJumpState : State
{
    float timePassed;
    float jumpTime;

    public SprintJumpState(Character _character, StateMachine _StateMachine) : base(_character, _StateMachine)
    {
        character = _character;
        stateMachine = _StateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        timePassed = 0f;
        character.ChangeState(CharacterState.SprintJump);
        character.ChangeApplyRootMotion(true);
        jumpTime = 1f;
    }

    public override void Exit() 
    {
        base.Exit();
        character.ChangeApplyRootMotion(false);
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(timePassed>jumpTime)
            stateMachine.ChangeState(character.sprinting);

        timePassed += Time.deltaTime;
    }


}
