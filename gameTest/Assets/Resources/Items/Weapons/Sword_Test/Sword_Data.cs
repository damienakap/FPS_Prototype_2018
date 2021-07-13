using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Data : WeaponData
{

    public override void initialize()
    {
        base.initialize();

        setWeaponIndex(WeaponIndex.TestSword);

        setBulletName("MeleeHitBoxPref");
        setBulletDecalName("Bullet_Decal");
        setBulletSpawnDistance(1.1f);
        setBulletForce(1000f);
        setFireSpreadAngle(0f);
        setMultishot(1f);
        setMultishotFanRotationAngle(0f);
        setFireRate(2f);
        setBurstFireRate(2f);
        setBurstFireCount(0);
        setChargeRate(1f);
        setDamage(80);
        setFireType(0);

        setAltBulletName("Sword_AltFirePrefab");
        setAltBulletDecalName("Bullet_Decal");
        setAltBulletSpawnDistance(1.1f);
        setAltBulletForce(1000f);
        setAltFireSpreadAngle(1f);
        setAltMultishot(1f);
        setAltMultishotFanRotationAngle(0f);
        setAltFireRate(2f);
        setAltBurstFireRate(2f);
        setAltBurstFireCount(0);
        setAltChargeRate(1f);
        setAltDamage(80);
        setAltFireType(2);

        setReloadSpeed(1.5f);
        setMeleeSpeed(2f);
        setSwapSpeed(2f);

        setAmmo(60);
        setMaxAmmo(60);
        setClipAmmo(1);
        setMaxClipAmmo(1);
        setAmmoCostPerFire(1);
        setAmmoCostPerAltFire(10);
        setAutoReload(true);
        setAutoAltReload(true);

        setAnimationDirectory("Items/Weapons/Sword_Test/");

        setIdleAnimationName("Sword_Idle");
        setFireAnimationName("Sword_Fire");
        setChargeAnimationName("Sword_Melee");
        setAltFireAnimationName("Sword_AltFire");
        setAltChargeAnimationName("Sword_AltCharge");
        setReloadAnimationName("Sword_Swap");
        setMeleeAnimationName("Sword_Melee");
        setSwapAnimationName("Sword_Swap");

        setAimDownAnimationName("Sword_Aim_Down");
        setAimUpAnimationName("Sword_Aim_UP");

        setFpsIdleAnimationName("Fps_Pistol_Idle");
        setFpsFireAnimationName("Fps_Pistol_Fire");
        setFpsChargeAnimationName("Fps_Pistol_Fire");
        setFpsAltFireAnimationName("Fps_Pistol_Fire");
        setFpsAltChargeAnimationName("Fps_Pistol_Fire");
        setFpsReloadAnimationName("Fps_Pistol_Reload");
        setFpsMeleeAnimationName("Fps_Pistol_Melee");
        setFpsSwapAnimationName("Fps_Pistol_Swap");
        

        setEquippedBone1(HumanBodyBones.RightHand);
        setEquippedPositionOffset1(new Vector3(-0.057f, 0.019f, -0.026f));
        setEquippedEulerRotationOffset1(new Vector3(-90f, 0f, 90f));

        setEquippedBone2(HumanBodyBones.LeftHand);
        setEquippedPositionOffset2(new Vector3());
        setEquippedEulerRotationOffset2(new Vector3());

        setHolsteredBone1(HumanBodyBones.LeftUpperLeg);
        setHolsteredPositionOffset1(new Vector3(-0.02f, -0.066f, -0.028f));
        setHolsteredEulerRotationOffset1(new Vector3(-180f, 140f, -90f));

        setHolsteredBone2(HumanBodyBones.RightUpperLeg);
        setHolsteredPositionOffset2(new Vector3());
        setHolsteredEulerRotationOffset2(new Vector3());

        setEquippedFpsBone1(21);

        setAsInitialized();
    }

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