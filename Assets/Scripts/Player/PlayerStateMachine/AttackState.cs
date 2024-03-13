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
        timePassed = 0f;
        character.CurrentAttackCount += 1;
        character.animator.SetFloat("speed", 0f);
        character.ChangeState(CharacterState.Attack);
        character.ChangeApplyRootMotion(true);

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

        if (timePassed >= (clipLength / clipSpeed) * 0.35f && attack && character.CurrentAttackCount <= 4)
        {
            stateMachine.ChangeState(character.attacking);
        }
        if (timePassed >= clipLength / clipSpeed)
        {
            character.CurrentAttackCount = 0;
            stateMachine.ChangeState(character.standing);

        }

        if (roll)
        {
            character.CurrentAttackCount = 0;
            stateMachine.ChangeState(character.rolling);
        }

    }


    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (velocity.sqrMagnitude > 0)
        {
            Vector3 forward = character.Camera.transform.forward;
            forward.y = 0;
            Vector3 dir = Quaternion.LookRotation(velocity) * forward;
            character.transform.rotation = Quaternion.Slerp(character.transform.rotation, Quaternion.LookRotation(dir), character.rotationDampTime);
        }
    }

    public override void Exit()
    {
        base.Exit();
        attack = false;
        roll = false;
        character.ChangeApplyRootMotion(false);
    }
}