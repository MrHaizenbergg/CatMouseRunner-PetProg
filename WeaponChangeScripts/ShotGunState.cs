using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGunState : WeaponState
{

    public override void Enter()
    {
        base.Enter();
        {
            PlayerController.Instance.PickUpShotGun();
            Debug.Log("ShotGunState");
        }
    }

    public override void Exit()
    {
        base.Exit();

    }
}
