using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobData : MonoBehaviour
{
    public int mobType = 0;

    public float baseHealth = 100f;
    public float baseArmor = 100f;
    public float baseShields = 100f;

    public float health = 100f;
    public float armor = 100f;
    public float shields = 100f;

    public float regenCooldown = 5f;
    public float currentRegenCooldown = 0f;

    public float healthRegenRate = 10f;
    public float shieldRegenRate = 10f;

}
