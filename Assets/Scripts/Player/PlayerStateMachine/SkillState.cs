
using System.Collections;
using UnityEditor;
using UnityEngine;

public class SkillState : State
{
    float timePassed;
    Coroutine _afterRoutine;
    public SkillState(Character _character, StateMachine _stateMachine, PlayerController _playerController) : base(_character, _stateMachine)
    {
        character = _character;
        stateMachine = _stateMachine;
        playerController = _playerController;
    }

    public override void Enter()
    {
        base.Enter();
        timePassed = 0f;
        character.animator.SetFloat("speed", 0f);
        character.ChangeState(CharacterState.Skill);
        character.ChangeApplyRootMotion(true);

        playerController.moveable = false;

        if (_afterRoutine != null)
            character.StopCoroutine(_afterRoutine);

        _afterRoutine = character.StartCoroutine(AfterAnimation());
    }


    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }


    public override void Exit()
    {
        base.Exit();
        character.ChangeApplyRootMotion(false);

        if (_afterRoutine != null)
            character.StopCoroutine(_afterRoutine);
    }


    IEnumerator AfterAnimation()
    {
        yield return YieldCache.WaitForSeconds(1.9f);
        stateMachine.ChangeState(character.standing);
    }
}
