using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow_Data : WeaponData
{
    
    public override void initialize()
    {
        base.initialize();

        setWeaponIndex(WeaponIndex.Bow);

        setBulletName("Bow_Arrow");
        setBulletDecalName("Bullet_Decal");
        setBulletSpawnDistance(1.1f);
        setBulletForce(1500f);
        setFireSpreadAngle(0f);
        setMultishot(1f);
        setMultishotFanRotationAngle(0f);
        setFireRate(2f);
        setBurstFireRate(2f);
        setBurstFireCount(0);
        setChargeRate(1f);
        setDamage(300f);
        setFireType(2);

        setAltBulletName("Bow_Arrow");
        setAltBulletDecalName("Bullet_Decal");
        setAltBulletSpawnDistance(1.1f);
        setAltBulletForce(1500f);
        setAltFireSpreadAngle(20f);
        setAltMultishot(5f);
        setAltMultishotFanRotationAngle(90f);
        setAltFireRate(2f);
        setAltBurstFireRate(2f);
        setAltBurstFireCount(0);
        setAltChargeRate(1f);
        setAltDamage(300f);
        setAltFireType(2);

        setReloadSpeed(1f);
        setMeleeSpeed(1.5f);
        setSwapSpeed(1.5f);

        setAmmo(40);
        setMaxAmmo(40);
        setClipAmmo(1);
        setMaxClipAmmo(1);
        setAmmoCostPerFire(1);
        setAmmoCostPerAltFire(5);
        setAutoReload(true);
        setAutoAltReload(true);

        setAnimationDirectory("Items/Weapons/Bow/");

        setIdleAnimationName("Bow_Idle");
        setFireAnimationName("Bow_Fire");
        setChargeAnimationName("Bow_Charge");
        setAltFireAnimationName("Bow_Fire");
        setAltChargeAnimationName("Bow_Charge");
        setReloadAnimationName("Bow_Reload");
        setMeleeAnimationName("Bow_Melee");
        setSwapAnimationName("Bow_Swap");

        setAimDownAnimationName("Bow_Aim_Down");
        setAimUpAnimationName("Bow_Aim_UP");

        setFpsIdleAnimationName("Fps_Bow_Idle");
        setFpsFireAnimationName("Fps_Bow_Fire");
        setFpsChargeAnimationName("Fps_Bow_Charge");
        setFpsAltFireAnimationName("Fps_Bow_Fire");
        setFpsAltChargeAnimationName("Fps_Bow_Charge");
        setFpsReloadAnimationName("FPS_AssaultRifle_Reload");
        setFpsMeleeAnimationName("Fps_Bow_Melee");
        setFpsSwapAnimationName("Fps_Bow_Swap");

        // Equipped
        setEquippedBone1(HumanBodyBones.LeftHand);
        setEquippedPositionOffset1(new Vector3(-0.049f, 0f, -0.019f));
        setEquippedEulerRotationOffset1(new Vector3(-0f, -90f, 90));

        // Holstered
        setHolsteredBone1(HumanBodyBones.Spine);
        setHolsteredPositionOffset1(new Vector3(-0.074f, 0f, -0.138f));
        setHolsteredEulerRotationOffset1(new Vector3(45f, 90f, 90f));

        /*
         * Left Hand: 3
         * Right Hand: 21
         */
        setEquippedFpsBone1(3);

        setAsInitialized();
    }

    // Start is called before the first frame update
    private void OnEnable()
    {
        if (!isInitialized())
        {
            initialize();
        }
    }

    private void Awake()
    {
        if (!isInitialized())
        {
            initialize();
        }
    }
}
