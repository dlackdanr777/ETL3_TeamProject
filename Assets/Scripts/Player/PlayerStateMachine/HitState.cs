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
        playerController.moveable = false;

        character.ChangeState(CharacterState.Hit);
        character.ChangeApplyRootMotion(true);

        if (_afterRoutine != null)
            character.StopCoroutine(_afterRoutine);

        _afterRoutine = character.StartCoroutine(AfterAnimation());

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        playerController.moveable = false;
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
        character.ChangeApplyRootMotion(false);

        if (_afterRoutine != null)
            character.StopCoroutine(_afterRoutine);

    }

    IEnumerator AfterAnimation()
    {
        yield return YieldCache.WaitForSeconds(1.1f);
        stateMachine.ChangeState(character.standing);
    }
}
