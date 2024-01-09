using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitState : State
{

    float timePassed;
    float clipLength;
    float clipSpeed;
    public HitState(Character _character, StateMachine _StateMachine) : base(_character, _StateMachine)
    {
        character = _character;
        stateMachine = _StateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        timePassed = 0f;
        character.animator.SetTrigger("hit");

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        timePassed += Time.deltaTime;
        clipLength = character.animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        clipSpeed = character.animator.GetCurrentAnimatorStateInfo(0).speed;

       
        if (timePassed >= clipLength / clipSpeed)
        {
            stateMachine.ChangeState(character.standing);
           //character.animator.SetTrigger("move");
        }

    }
}
