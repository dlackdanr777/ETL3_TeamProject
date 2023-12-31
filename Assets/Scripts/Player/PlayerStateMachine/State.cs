using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.InputSystem;

public class State 
{
    public Character character;
    public StateMachine stateMachine;
    public PlayerController playerController;

    protected Vector3 gravityVelocity;
    protected Vector3 velocity;
    protected Vector2 input;

    public InputAction moveAction;
    public InputAction lookAction;
    public InputAction jumpAction;
    public InputAction sprintAction;
    public InputAction drawWeaponAction;
    public InputAction attackAction;
    public InputAction rollAction;

    public State(Character _character,StateMachine _StateMachine)
    {
        character = _character;
        stateMachine = _StateMachine;

        moveAction = character.playerInput.actions["Move"];
        lookAction = character.playerInput.actions["Look"];
        jumpAction = character.playerInput.actions["Jump"];
        sprintAction = character.playerInput.actions["Sprint"];
        //drawWeaponAction = character.playerInput.actions["DrawWeapon"];
        attackAction = character.playerInput.actions["Attack"];
        rollAction = character.playerInput.actions["Roll"];

    }
    public virtual void Enter()
    {
        Debug.Log("enter state: " + this.ToString());
        
    }

    public virtual void HandleInput()
    {

    }
    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {

    }
    public virtual void Exit()
    {

    }

  
}
