using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol_Test_Data : WeaponData
{

    public override void initialize()
    {
        base.initialize();

        setWeaponIndex(WeaponIndex.TestPistol);

        setBulletName("Pistol_Test_Bullet");
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

        setAltBulletName("Pistol_Test_Bullet");
        setAltBulletDecalName("Bullet_Decal");
        setAltBulletSpawnDistance(1.1f);
        setAltBulletForce(1000f);
        setAltFireSpreadAngle(10f);
        setAltMultishot(1f);
        setAltMultishotFanRotationAngle(0f);
        setAltFireRate(2f);
        setAltBurstFireRate(8f);
        setAltBurstFireCount(2);
        setAltChargeRate(1f);
        setAltDamage(80);
        setAltFireType(0);

        setReloadSpeed(1.5f);
        setMeleeSpeed(2f);
        setSwapSpeed(2f);

        setAmmo(60);
        setMaxAmmo(60);
        setClipAmmo(15);
        setMaxClipAmmo(15);
        setAmmoCostPerFire(1);
        setAmmoCostPerAltFire(1);
        setAutoReload(false);
        setAutoAltReload(false);

        setAnimationDirectory("Items/Weapons/Pistol_Test/");

        setIdleAnimationName("Pistol_Idle");
        setFireAnimationName("Pistol_Fire");
        setChargeAnimationName("Pistol_Fire");
        setAltFireAnimationName("Pistol_Fire");
        setAltChargeAnimationName("Pistol_Fire");
        setReloadAnimationName("Pistol_Reload");
        setMeleeAnimationName("Pistol_Melee");
        setSwapAnimationName("Pistol_Swap");

        setAimDownAnimationName("Pistol_Aim_Down");
        setAimUpAnimationName("Pistol_Aim_UP");

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
        setEquippedEulerRotationOffset1(new Vector3(10.836f, -89.98901f, 0));

        setEquippedBone2(HumanBodyBones.LeftHand);
        setEquippedPositionOffset2(new Vector3());
        setEquippedEulerRotationOffset2(new Vector3());

        setHolsteredBone1(HumanBodyBones.RightUpperLeg);
        setHolsteredPositionOffset1(new Vector3(-0.105f, 0.066f, 0.038f));
        setHolsteredEulerRotationOffset1(new Vector3(0f,-90f,90f));

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