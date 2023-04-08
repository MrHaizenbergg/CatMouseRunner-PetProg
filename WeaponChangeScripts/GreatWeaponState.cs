using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatWeaponState : WeaponState
{
    public override void Enter()
    {
        base.Enter();
        {
            PlayerController.Instance.PickUpGreatWeapon();
            Debug.Log("GreatWeaponState");
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
