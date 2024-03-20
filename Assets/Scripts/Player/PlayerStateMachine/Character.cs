using UnityEngine;
using UnityEngine.InputSystem;

public enum CharacterState
{
    IdleAndMove,
    Jump,
    SprintJump,
    Landing,
    Sprint,
    Roll,
    Attack,
    Skill,
    Hit,
    Die,
}


public class Character : MonoBehaviour
{
    [Header("Controls")]
    public float playerSpeed = 5.0f;
    public float sprintSpeed = 7.0f;
    public float jumpHeight = 0.8f;
    public float gravityMultiplier = 2;
    public float rotationSpeed = 5f;
    public float crouchColliderHeight = 1.35f;

    [Header("Animation Smoothing")]
    [Range(0f, 1f)]
    public float speedDampTime = 0.1f;
    [Range(0f, 1f)]
    public float velocityDampTime = 0.9f;
    [Range(0f, 1f)]
    public float rotationDampTime = 0.2f;
    [Range(0f, 1f)]
    public float airControl = 0.5f;

    [Space]
    [Header("Components")]
    [SerializeField] protected CameraTarget _cameraTarget;
    public CameraTarget CameraTarget => _cameraTarget;


    public StateMachine movementSM;
    public StandingState standing;
    public JumpingState jumping;
    public LandingState landing;
    public SprintState sprinting;
    public SprintJumpState sprintjumping;
    //public CombatState combatting;
    public AttackState attacking;
    public RollState rolling;
    public SkillState skilling;
    public HitState hitting;
    public PlayerController playerController;
    public ParticleSystem _hitParticle;

    [HideInInspector]
    public float gravityValue = -9.81f;
    [HideInInspector]
    public float normalColliderHeight;
    [HideInInspector]
    public CharacterController controller;
    [HideInInspector]
    public PlayerInput playerInput;
    [HideInInspector]
    public Transform cameraTransform;
    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public Vector3 playerVelocity;

    protected CharacterState _state;

    private Vector3 _tmpPos;

    //플레이어의 공격 횟수를 카운트하는 변수
    private int _currentAttackCount;
    public int CurrentAttackCount
    {
        get 
        {
            return _currentAttackCount;
        }

        set
        {
            _currentAttackCount = value;
            animator.SetInteger("AttackCount", _currentAttackCount);
        }
    }

    protected virtual void Start()
    {
        playerController = GetComponent<PlayerController>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;

        movementSM = new StateMachine();
        standing = new StandingState(this, movementSM,playerController);
        jumping = new JumpingState(this, movementSM);
        landing = new LandingState(this, movementSM);
        sprinting = new SprintState(this, movementSM,playerController);
        sprintjumping = new SprintJumpState(this, movementSM);
        //combatting = new CombatState(this, movementSM);
        attacking = new AttackState(this, movementSM,playerController);
        rolling = new RollState(this, movementSM,playerController);
        skilling = new SkillState(this, movementSM,playerController);
        hitting = new HitState(this, movementSM,playerController);
        movementSM.initialized(standing,playerController);

        normalColliderHeight = controller.height;
        gravityValue *= gravityMultiplier;

        playerController.OnHpDepleted += OnHitStateChange;
    }


    protected virtual void Update()
    {
        movementSM.currentState.HandleInput();
        movementSM.currentState.LogicUpdate();
        animator.SetInteger("State", (int)_state);
    }


    protected virtual void FixedUpdate()
    {
        _tmpPos = transform.position;

        movementSM.currentState.PhysicsUpdate();

        RaycastHit[] hit = Physics.RaycastAll(transform.position + transform.up, transform.forward, 0.6f);

        for(int i =0, count = hit.Length; i < count; i++)
        {
            if (hit[i].transform.gameObject == gameObject)
                continue;

            transform.position = _tmpPos;
            return;
        }
    }


    public void OnHitStateChange(object subject, float value)
    {
        if (4 < value)
        {
            movementSM.ChangeState(hitting);
            CurrentAttackCount = 0;
            GameObject target = subject as GameObject;
            gameObject.transform.LookAt(target.transform);

            _hitParticle.Play();
        }
    }


    public virtual void ChangeState(CharacterState state)
    {
        _state = state;
    }


    public virtual void ChangeApplyRootMotion(bool value)
    {
        animator.applyRootMotion = value;
    }


    protected virtual void OnAnimatorMove()
    {
        RaycastHit[] hit = Physics.RaycastAll(transform.position + transform.up, transform.forward, 0.6f);

        for (int i = 0, count = hit.Length; i < count; i++)
        {
            if (hit[i].transform.gameObject == gameObject)
                continue;

            transform.position = _tmpPos;
            return;
        }

        Vector3 movement = animator.deltaPosition;
        controller.Move(movement);
    }
}
