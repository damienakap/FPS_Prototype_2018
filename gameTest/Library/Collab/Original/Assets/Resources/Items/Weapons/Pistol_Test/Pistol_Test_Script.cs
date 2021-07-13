using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol_Test_Script : WeaponData
{

    public override void initialize()
    {
        base.initialize();

        this.setWeaponName("Pistol_Test");
        this.setWeaponDirectory("Items/Weapons/Pistol_Test/");
        this.setBulletName("Pistol_Test_Bullet");
        this.setBulletDecalName("Bullet_Decal");
        this.setFireRate(0.01f);
        this.setFireType(0);

        this.setAnimationDirectory("Items/Weapons/Pistol_Test/");
        this.setIdleAnimationName("Pistol_Idle");
        this.setFireAnimationName("Pistol_Fire");
        this.setReloadAnimationName("Pistol_Reload");
        this.setAimDownAnimationName("Pistol_Aim_Down");
        this.setAimUpAnimationName("Pistol_Aim_UP");

        this.setPositionOffset(new Vector3());
        this.setEulerRotationOffset(new Vector3());
        this.setBulletSpawnDistance(1.1f);

        this.setEquippedBone1(HumanBodyBones.RightHand);
        this.setEquippedPositionOffset1(new Vector3(-0.00066f, 0f, 0f));
        this.setEquippedEulerRotationOffset1(new Vector3(0,-90,0));

        this.setEquippedBone2(HumanBodyBones.LeftHand);
        this.setEquippedPositionOffset2(new Vector3());
        this.setEquippedEulerRotationOffset2(new Vector3());

        this.setHolsteredBone1(HumanBodyBones.RightUpperLeg);
        this.setHolsteredPositionOffset1(new Vector3(0f, 0.00058f, 0.0005f));
        this.setHolsteredEulerRotationOffset1(new Vector3(0f,-90f,90f));

        this.setHolsteredBone2(HumanBodyBones.LeftUpperLeg);
        this.setHolsteredPositionOffset2(new Vector3());
        this.setHolsteredEulerRotationOffset2(new Vector3());

        this.setAsInitialized();
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