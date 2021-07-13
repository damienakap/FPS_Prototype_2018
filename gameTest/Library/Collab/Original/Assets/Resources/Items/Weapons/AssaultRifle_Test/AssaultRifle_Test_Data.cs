using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle_Test_Data : WeaponData
{
    
    public override void initialize()
    {
        base.initialize();

        this.setWeaponName("AssaultRifle_Test");
        this.setWeaponDirectory("Items/Weapons/AssaultRifle_Test/");
        this.setBulletName("Pistol_Test_Bullet");
        this.setBulletDecalName("Bullet_Decal");
        this.setFireRate(0.01f);
        this.setFireType(1);

        this.setAnimationDirectory("Items/Weapons/AssaultRifle_Test/");
        this.setIdleAnimationName("AssaultRifle_Idle");
        this.setFireAnimationName("AssaultRifle_Fire");
        this.setReloadAnimationName("Pistol_Reload");
        this.setAimDownAnimationName("AssaultRifle_Aim_Down");
        this.setAimUpAnimationName("AssaultRifle_Aim_UP");

        this.setPositionOffset(new Vector3());
        this.setEulerRotationOffset(new Vector3());
        this.setBulletSpawnDistance(1.1f);

        // Equipped
        this.setEquippedBone1(HumanBodyBones.RightHand);
        this.setEquippedPositionOffset1(new Vector3(-0.00066f, 0f, 0f));
        this.setEquippedEulerRotationOffset1(new Vector3(0, -53, 0));

        this.setEquippedBone2(HumanBodyBones.LeftHand);
        this.setEquippedPositionOffset2(new Vector3());
        this.setEquippedEulerRotationOffset2(new Vector3());

        // Holstered
        this.setHolsteredBone1(HumanBodyBones.Spine);
        this.setHolsteredPositionOffset1(new Vector3(-0.00074f, 0.00102f, -0.00138f));
        this.setHolsteredEulerRotationOffset1(new Vector3( 0f,90f,90f));

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
