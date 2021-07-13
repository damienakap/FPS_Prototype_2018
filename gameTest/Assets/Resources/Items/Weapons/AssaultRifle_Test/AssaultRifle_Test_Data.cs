using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle_Test_Data : WeaponData
{
    
    public override void initialize()
    {
        base.initialize();

        setWeaponIndex(WeaponIndex.TestAssaultRifle);

        setBulletName("Pistol_Test_Bullet");
        setBulletDecalName("Bullet_Decal");
        setBulletForce(250f);
        setBulletSpawnDistance(1.1f);
        setFireSpreadAngle(2f);
        setMultishot(1f);
        setMultishotFanRotationAngle(0f);
        setFireRate(20f);
        setBurstFireRate(20f);
        setBurstFireCount(0);
        setChargeRate(1f);
        setDamage(20f);
        setFireType(1);

        setAltBulletName("Pistol_Test_Bullet");
        setAltBulletDecalName("Bullet_Decal");
        setAltBulletForce(250f);
        setAltBulletSpawnDistance(1.1f);
        setAltFireSpreadAngle(5f);
        setAltMultishot(1f);
        setAltMultishotFanRotationAngle(0f);
        setAltFireRate(40f);
        setAltBurstFireRate(20f);
        setAltBurstFireCount(0);
        setAltChargeRate(1f);
        setAltDamage(20f);
        setAltFireType(1);

        setReloadSpeed(1f);
        setMeleeSpeed(1.5f);
        setSwapSpeed(1.5f);

        setAmmo(500);
        setMaxAmmo(500);
        setClipAmmo(100);
        setMaxClipAmmo(100);
        setAmmoCostPerFire(1);
        setAmmoCostPerAltFire(1);
        setAutoReload(false);
        setAutoAltReload(false);

        setAnimationDirectory("Items/Weapons/AssaultRifle_Test/");

        setIdleAnimationName("AssaultRifle_Idle");
        setFireAnimationName("AssaultRifle_Fire");
        setChargeAnimationName("AssaultRifle_Fire");
        setAltFireAnimationName("AssaultRifle_Fire");
        setAltChargeAnimationName("AssaultRifle_Fire");
        setReloadAnimationName("Pistol_Reload");
        setMeleeAnimationName("AssaultRifle_Melee");
        setSwapAnimationName("AssaultRifle_Swap");

        setAimDownAnimationName("AssaultRifle_Aim_Down");
        setAimUpAnimationName("AssaultRifle_Aim_UP");

        setFpsIdleAnimationName("Fps_AssaultRifle_Idle");
        setFpsFireAnimationName("Fps_AssaultRifle_Fire");
        setFpsChargeAnimationName("Fps_AssaultRifle_Fire");
        setFpsAltFireAnimationName("Fps_AssaultRifle_Fire");
        setFpsAltChargeAnimationName("Fps_AssaultRifle_Fire");
        setFpsReloadAnimationName("Fps_AssaultRifle_Reload");
        setFpsMeleeAnimationName("Fps_AssaultRifle_Melee");
        setFpsSwapAnimationName("Fps_AssaultRifle_Swap");


        // Equipped
        setEquippedBone1(HumanBodyBones.RightHand);
        setEquippedPositionOffset1(new Vector3(-0.044f, 0.001f, -0.006f));
        setEquippedEulerRotationOffset1(new Vector3(1.731f, -79.677f, 0.058f));

        setEquippedBone2(HumanBodyBones.LeftHand);
        setEquippedPositionOffset2(new Vector3());
        setEquippedEulerRotationOffset2(new Vector3());

        // Holstered
        setHolsteredBone1(HumanBodyBones.Spine);
        setHolsteredPositionOffset1(new Vector3(-0.121f, 0.104f, -0.146f));
        setHolsteredEulerRotationOffset1(new Vector3( 0f,90f,90f));

        setHolsteredBone2(HumanBodyBones.LeftUpperLeg);
        setHolsteredPositionOffset2(new Vector3());
        setHolsteredEulerRotationOffset2(new Vector3());

        setEquippedFpsBone1(21);

        setAsInitialized();
    }
    // Start is called before the first frame update
    private void Start()
    {
        if (!this.isInitialized())
        {
            this.initialize();
        }
    }

    // Start is called before the first frame update
    private void OnEnable()
    {
        if (!this.isInitialized())
        {
            this.initialize();
        }
    }

    private void Awake()
    {
        if (!this.isInitialized())
        {
            this.initialize();
        }
    }
}
