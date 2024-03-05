using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitState : State
{

    float timePassed;
    float clipLength;
    float clipSpeed;


    Coroutine _afterRoutine;
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
        
        if (_afterRoutine != null)
            character.StopCoroutine(_afterRoutine);

        _afterRoutine = character.StartCoroutine(AfterAnimation());

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();


    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        character.animator.SetFloat("speed", 0f);
        velocity = Vector3.zero;
    }

    IEnumerator AfterAnimation()
    {
        //character.animator.Play("");
        do
        {
           
            yield return YieldCache.WaitForSeconds(0.2f);
        }
        while (character.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.2f);

        stateMachine.ChangeState(character.standing);

        if (_afterRoutine != null)
            character.StopCoroutine(_afterRoutine);

    }
}
