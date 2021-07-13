using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{

    private bool init = false;

    protected string weaponDirectory;
    protected string weaponName;
    protected Vector3 positionOffset;
    protected Vector3 eulerRotationOffset;

    protected string bulletName;
    protected string bulletDecalName;
    protected float bulletSpawnDistance;   // radially from the character
    protected float fireRate;

    protected string animationDirectory;
    protected string idleAnimationName;
    protected string fireAnimationName;
    protected string reloadAnimationName;

    protected string aimDownAnimationName;
    protected string aimUpAnimationName;

    protected HumanBodyBones equippedBone1;
    protected HumanBodyBones equippedBone2;
    protected HumanBodyBones holsteredBone1;
    protected HumanBodyBones holsteredBone2;

    protected Vector3 equippedPositionOffset1;
    protected Vector3 equippedPositionOffset2;
    protected Vector3 equippedEulerRotationOffset1;
    protected Vector3 equippedEulerRotationOffset2;

    protected Vector3 holsteredPositionOffset1;
    protected Vector3 holsteredPositionOffset2;
    protected Vector3 holsteredEulerRotationOffset1;
    protected Vector3 holsteredEulerRotationOffset2;


    public virtual void initialize() { }
    public void setAsInitialized() { this.init = true; }
    public bool isInitialized() { return this.init; }

    public void setWeaponName(string s) { this.weaponName = s; }
    public void setWeaponDirectory(string s) { this.weaponDirectory = s; }
    public void setBulletName(string s) { this.bulletName = s; }
    public void setBulletDecalName(string s) { this.bulletDecalName = s; }
    public void setFireRate(float f) { this.fireRate = f; }

    public void setAnimationDirectory(string s) { this.animationDirectory = s; }
    public void setIdleAnimationName(string s) { this.idleAnimationName = s; }
    public void setFireAnimationName(string s) { this.fireAnimationName = s; }
    public void setReloadAnimationName(string s) { this.reloadAnimationName = s; }
    public void setAimDownAnimationName(string s) { this.aimDownAnimationName = s; }
    public void setAimUpAnimationName(string s) { this.aimUpAnimationName = s; }

    public void setBulletSpawnDistance(float f) { this.bulletSpawnDistance = f; }
    public void setPositionOffset(Vector3 v) { this.positionOffset = v; }
    public void setEulerRotationOffset(Vector3 v) { this.eulerRotationOffset = v; }
    public void setEquippedBone1(HumanBodyBones bone) { this.equippedBone1 = bone; }
    public void setEquippedBone2(HumanBodyBones bone) { this.equippedBone2 = bone; }
    public void setHolsteredBone1(HumanBodyBones bone) { this.holsteredBone1 = bone; }
    public void setHolsteredBone2(HumanBodyBones bone) { this.holsteredBone2 = bone; }
    public void setEquippedPositionOffset1(Vector3 v) { this.equippedPositionOffset1 = v; }
    public void setEquippedPositionOffset2(Vector3 v) { this.equippedPositionOffset2 = v; }
    public void setEquippedEulerRotationOffset1(Vector3 v) { this.equippedEulerRotationOffset1 = v; }
    public void setEquippedEulerRotationOffset2(Vector3 v) { this.equippedEulerRotationOffset2 = v; }
    public void setHolsteredPositionOffset1(Vector3 v) { this.holsteredPositionOffset1 = v; }
    public void setHolsteredPositionOffset2(Vector3 v) { this.holsteredPositionOffset2 = v; }
    public void setHolsteredEulerRotationOffset1(Vector3 v) { this.holsteredEulerRotationOffset1 = v; }
    public void setHolsteredEulerRotationOffset2(Vector3 v) { this.holsteredEulerRotationOffset2 = v; }

    public string getWeaponName() { return this.weaponName; }
    public string getWeaponDirectory() { return this.weaponDirectory; }
    public string getBulletName() { return this.bulletName; }
    public string getBulletDecalName() { return this.bulletDecalName; }
    public float getFireRate() { return this.fireRate; }

    public string getAnimationDirectory() { return this.animationDirectory; }
    public string getIdleAnimationName() { return this.idleAnimationName; }
    public string getFireAnimationName() { return this.fireAnimationName; }
    public string getReloadAnimationName() { return this.reloadAnimationName; }
    public string getAimDownAnimationName() { return this.aimDownAnimationName; }
    public string getAimUpAnimationName() { return this.aimUpAnimationName; }

    public float getBulletSpawnDistance() { return this.bulletSpawnDistance; }
    public Vector3 getPositionOffset() { return this.positionOffset; }
    public Vector3 getEulerRotationOffset() { return this.eulerRotationOffset; }
    public HumanBodyBones getEquippedBone1() { return this.equippedBone1; }
    public HumanBodyBones getEquippedBone2() { return this.equippedBone2; }
    public HumanBodyBones getHolsteredBone1() { return this.holsteredBone1; }
    public HumanBodyBones getHolsteredBone2() { return this.holsteredBone2; }
    public Vector3 getEquippedPositionOffset1() { return this.equippedPositionOffset1; }
    public Vector3 getEquippedPositionOffset2() { return this.equippedPositionOffset2; }
    public Vector3 getEquippedEulerRotationOffset1() { return this.equippedEulerRotationOffset1; }
    public Vector3 getEquippedEulerRotationOffset2() { return this.equippedEulerRotationOffset2; }
    public Vector3 getHolsteredPositionOffset1() { return this.holsteredPositionOffset1; }
    public Vector3 getHolsteredPositionOffset2() { return this.holsteredPositionOffset2; }
    public Vector3 getHolsteredEulerRotationOffset1() { return this.holsteredEulerRotationOffset1; }
    public Vector3 getHolsteredEulerRotationOffset2() { return this.holsteredEulerRotationOffset2; }


    public AnimationClip getIdleAnimation() { return GameManager.loadAnimationClip(this.idleAnimationName, this.animationDirectory); }
    public AnimationClip getFireAnimation() { return GameManager.loadAnimationClip(this.fireAnimationName, this.animationDirectory); }
    public AnimationClip getReloadAnimation() { return GameManager.loadAnimationClip(this.reloadAnimationName, this.animationDirectory); }
    public AnimationClip getAimDownAnimation() { return GameManager.loadAnimationClip(this.aimDownAnimationName, this.animationDirectory); }
    public AnimationClip getAimUpAnimation() { return GameManager.loadAnimationClip(this.aimUpAnimationName, this.animationDirectory); }

}
