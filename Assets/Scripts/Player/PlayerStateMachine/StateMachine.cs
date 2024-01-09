using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine 
{
    public State currentState;

    public void initialized(State startingState,PlayerController playerController)
    {
        currentState = startingState;
        playerController.OnHpDepleted += OnHitStateChange;
        startingState.Enter();
    }

    public void ChangeState(State newState)
    {
        currentState.Exit();

        currentState = newState;
        newState.Enter();
    }
    public void OnHitStateChange(object subject, float value)
    {
        if (10 < value)
        {
            
        }
    }

}
