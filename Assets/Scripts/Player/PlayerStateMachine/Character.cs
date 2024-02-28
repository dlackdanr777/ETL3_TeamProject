using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    public PlayerController playerController;

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


    

    private void Start()
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

        movementSM.initialized(standing,playerController);

        normalColliderHeight = controller.height;
        gravityValue *= gravityMultiplier;
    }

    private void Update()
    {
        movementSM.currentState.HandleInput();

        movementSM.currentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        movementSM.currentState.PhysicsUpdate();
    }

    
}
