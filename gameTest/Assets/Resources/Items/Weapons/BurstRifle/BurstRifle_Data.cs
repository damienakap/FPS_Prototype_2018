using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstRifle_Data : WeaponData
{
    
    public override void initialize()
    {
        base.initialize();

        setWeaponIndex(WeaponIndex.BurstRifle);

        setBulletName("Pistol_Test_Bullet");
        setBulletDecalName("Bullet_Decal");
        setBulletSpawnDistance(1.1f);
        setBulletForce(500f);
        setFireSpreadAngle(0.5f);
        setMultishot(1f);
        setMultishotFanRotationAngle(0f);
        setFireRate(4f);
        setBurstFireRate(20f);
        setBurstFireCount(3);
        setChargeRate(1f);
        setDamage(50f);
        setFireType(0);

        setAltBulletName("Pistol_Test_Bullet");
        setAltBulletDecalName("Bullet_Decal");
        setAltBulletSpawnDistance(1.1f);
        setAltBulletForce(500f);
        setAltFireSpreadAngle(1f);
        setAltMultishot(1f);
        setAltMultishotFanRotationAngle(0f);
        setAltFireRate(0.5f);
        setAltBurstFireRate(0.5f);
        setAltBurstFireCount(0);
        setAltChargeRate(1f);
        setAltDamage(70f);
        setAltFireType(-1);

        setReloadSpeed(1f);
        setMeleeSpeed(1.5f);
        setSwapSpeed(1.5f);

        setAmmo(100);
        setMaxAmmo(100);
        setClipAmmo(20);
        setMaxClipAmmo(20);
        setAmmoCostPerFire(1);
        setAmmoCostPerAltFire(1);
        setAutoReload(false);
        setAutoAltReload(false);

        setAnimationDirectory("Items/Weapons/BurstRifle/");

        setIdleAnimationName("BurstRifle_Idle");
        setFireAnimationName("BurstRifle_Fire");
        setChargeAnimationName("BurstRifle_Fire");
        setAltFireAnimationName("BurstRifle_Fire");
        setAltChargeAnimationName("BurstRifle_Fire");
        setReloadAnimationName("BurstRifle_Reload");
        setMeleeAnimationName("BurstRifle_Melee");
        setSwapAnimationName("BurstRifle_Swap");

        setAimDownAnimationName("BurstRifle_Aim_Down");
        setAimUpAnimationName("BurstRifle_Aim_UP");

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
        setEquippedEulerRotationOffset1(new Vector3(-75f, -260f, 168f));

        setEquippedBone2(HumanBodyBones.LeftHand);
        setEquippedPositionOffset2(new Vector3());
        setEquippedEulerRotationOffset2(new Vector3());

        // Holstered
        setHolsteredBone1(HumanBodyBones.Spine);
        setHolsteredPositionOffset1(new Vector3(-0.121f, 0.104f, -0.146f));
        setHolsteredEulerRotationOffset1(new Vector3(0f, 0f, 90f));

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
