using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingState : State
{
    float timePassed;
    float landingTime;
    public LandingState(Character _character, StateMachine _StateMachine) : base(_character, _StateMachine)
    {
        character = _character;
        stateMachine = _StateMachine;
    }
    public override void Enter()
    {
        base.Enter();
        timePassed = 0f;
        landingTime = 0.5f;
        character.ChangeState(CharacterState.Landing);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (timePassed > landingTime)
            stateMachine.ChangeState(character.standing);

        timePassed += Time.deltaTime;
    }
}
