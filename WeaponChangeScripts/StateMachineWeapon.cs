using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineWeapon
{
    public WeaponState currentState { get; set; }

    public void Initialize(WeaponState startState)
    {
        currentState = startState;
        currentState.Enter();
    }

    public void ChangeState(WeaponState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
