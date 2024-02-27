
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
        character.animator.applyRootMotion = true;
        timePassed = 0f;
        character.animator.SetTrigger("attack");
        character.animator.SetFloat("speed", 0f);

        if (_afterRoutine != null)
        {
            character.StopCoroutine(_afterRoutine);
        }

        _afterRoutine = character.StartCoroutine(AfterAnimation());
    }

    public override void Exit()
    {
        base.Exit();
        character.animator.applyRootMotion = false;
    }

    IEnumerator AfterAnimation()
    {
        //character.animator.Play("");
        do
        {

            yield return YieldCache.WaitForSeconds(0.2f);
        }
        while (character.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8f);
        

        stateMachine.ChangeState(character.standing);

        if (_afterRoutine != null)
            character.StopCoroutine(_afterRoutine);

    }
}
