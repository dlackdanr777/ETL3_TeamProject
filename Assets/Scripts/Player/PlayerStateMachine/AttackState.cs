using UnityEngine;
public class AttackState : State
{
    float timePassed;
    float clipLength;
    float clipSpeed;
    bool attack;
    bool roll;

    public AttackState(Character _character, StateMachine _stateMachine,PlayerController _playerController) : base(_character, _stateMachine)
    {
        character = _character;
        stateMachine = _stateMachine;
        playerController = _playerController;
    }

    public override void Enter()
    {
        base.Enter();
        playerController.moveable = false;
        attack = false;
        roll = false;
        character.animator.applyRootMotion = true;
        timePassed = 0f;
        character.animator.SetTrigger("attack");
        character.animator.SetFloat("speed", 0f);

        input = moveAction.ReadValue<Vector2>();
        velocity = new Vector3(input.x, 0, input.y);
    }

    public override void HandleInput()
    {
        base.HandleInput();

        if (attackAction.triggered&&playerController.Sta>playerController.attackStamina)
        {
            attack = true;
            playerController.DepleteSta(playerController.Sta,playerController.attackStamina);
        }

        if (rollAction.triggered)
            roll = true;
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();


        timePassed += Time.deltaTime;
        clipLength = character.animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        clipSpeed = character.animator.GetCurrentAnimatorStateInfo(0).speed;

        if (timePassed >= (clipLength / clipSpeed) * 0.35f && attack)
        {
            stateMachine.ChangeState(character.attacking);
        }
        if (timePassed >= clipLength / clipSpeed)
        {
            stateMachine.ChangeState(character.standing);
            character.animator.SetTrigger("move");
        }

        if (roll)
        {
            stateMachine.ChangeState(character.rolling);
        }

    }


    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (velocity.sqrMagnitude > 0)
        {
            character.transform.rotation = Quaternion.Slerp(character.transform.rotation, Quaternion.LookRotation(velocity), character.rotationDampTime);
        }
    }

    public override void Exit()
    {
        base.Exit();
        character.animator.applyRootMotion = false;
    }
}