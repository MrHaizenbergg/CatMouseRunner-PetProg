using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStats : MonoBehaviour
{
    public float StrengthKick { get; private set; }

    public float SpeedKick { get; private set; }

    public float DamageKick { get;private set; }

    public ItemStats(float strengthKick, float speedKick, float damageKick)
    {
        StrengthKick = strengthKick;
        SpeedKick = speedKick;
        DamageKick = damageKick;
    }
}

public enum ItemType
{
    Toaster,
    Cubok1,
    Cubok2,
    Telek,
    Plant
}
