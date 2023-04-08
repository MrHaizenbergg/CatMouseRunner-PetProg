using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamiteState : WeaponState
{
    public override void Enter()
    {
        base.Enter();
        {
            PlayerController.Instance.PickUpDynamit();
            Debug.Log("DynamiteState");
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
