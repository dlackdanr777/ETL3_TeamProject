using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitState : State
{

    float timePassed;
    float clipLength;
    float clipSpeed;


    Coroutine _afterRoutine;
    public HitState(Character _character, StateMachine _StateMachine, PlayerController _playerController) : base(_character, _StateMachine)
    {
        character = _character;
        stateMachine = _StateMachine;
        playerController = _playerController;
    }

    public override void Enter()
    {
        base.Enter();
        timePassed = 0f;
        character.animator.SetTrigger("hit");
        playerController.moveable = false;
        
        if (_afterRoutine != null)
            character.StopCoroutine(_afterRoutine);

        _afterRoutine = character.StartCoroutine(AfterAnimation());

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        playerController.moveable = false;
        character.animator.applyRootMotion = true;
    }



    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        character.animator.SetFloat("speed", 0f);
    }

    public override void Exit()
    {
        base.Exit();
        playerController.moveable = true;
        character.animator.applyRootMotion = false;

        if (_afterRoutine != null)
            character.StopCoroutine(_afterRoutine);

    }

    IEnumerator AfterAnimation()
    {
        
        do
        {
            character.animator.SetFloat("speed", 0f);
            yield return YieldCache.WaitForSeconds(0.1f);
        }
        while (character.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.9f);

        stateMachine.ChangeState(character.standing);

    }
}
