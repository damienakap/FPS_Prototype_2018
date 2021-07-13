using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData : MonoBehaviour
{

    private bool init = false;

    protected WeaponIndex weaponIndex;

    protected int maxAmmo;
    protected int ammo;
    protected int maxClipAmmo;
    protected int clipAmmo;

    protected int ammoCostPerFire = 1;
    protected int ammoCostPerAltFire = 1;
    protected bool autoReload = false;
    protected bool autoAltReload = false;

    //protected string weaponDirectory;
    //protected string weaponName;

    protected string bulletName;
    protected string bulletDecalName;

    protected float bulletSpawnDistance;   // radially from the character

    protected string altBulletName;
    protected string altBulletDecalName;
    protected float altBulletSpawnDistance;


    protected float bulletForce = 1;
    protected float fireSpreadAngle;
    protected float multishot;
    protected float currentMultishotCount = 0f;
    protected float multishotFanRotationAngle;      // 0-180deg ( set = 0 for no fan angle)
    protected float fireRate;
    protected float burstFireRate;
    protected float chargeRate;
    protected float chargeAmount = 0f;
    protected float damage;
    protected int burstFireCount = 0;
    protected int currentBurstFireCount = 0;
    protected float burstFireDelay = 0f;
    protected float burstFireTimer = 0f;
    protected bool fired = false;

    protected float altBulletForce = 1;
    protected float altFireSpreadAngle;
    protected float altMultishot;
    protected float currentAltMultishotCount = 0f;
    protected float altMultishotFanRotationAngle;   // 0-180deg ( set = 0 for no fan angle)
    protected float altFireRate;
    protected float altBurstFireRate;
    protected float altChargeRate;
    protected float altChargeAmount = 0f;
    protected float altDamage;
    protected int altBurstFireCount = 0;
    protected int currentAltBurstFireCount = 0;
    protected float altBurstFireDelay = 0f;
    protected float altBurstFireTimer = 0f;
    protected bool altFired = false;

    protected float reloadSpeed;
    protected float meleeSpeed;
    protected float swapSpeed;

    protected bool triggerPulled = false;
    protected bool triggerPulledLast = false;
    protected bool justTriggerPulled = false;
    protected bool justTriggerReleased = false;

    protected bool altTriggerPulled = false;
    protected bool altTriggerPulledLast = false;
    protected bool justAltTriggerPulled = false;
    protected bool justAltTriggerReleased = false;

    /* <-1: none (only alt fire)
     * -1: zoom (only alt fire)
     * 0: semi-auto
     * 1: automatic
     * 2: charge
     */
    protected int fireType;
    protected int altFireType;

    //##### Animation Variables #####

    protected string animationDirectory;

    // 3rd person animations
    protected string idleAnimationName;

    protected string fireAnimationName;
    protected string chargeAnimationName;
    protected string altFireAnimationName;
    protected string altChargeAnimationName;
    protected string reloadAnimationName;
    protected string meleeAnimationName;
    protected string swapAnimationName;

    // 3rd person animation lengths
    protected float idleAnimationLength;

    protected float fireAnimationLength;
    protected float chargeAnimationLength;
    protected float altFireAnimationLength;
    protected float altChargeAnimationLength;
    protected float reloadAnimationLength;
    protected float meleeAnimationLength;
    protected float swapAnimationLength;

    // first person animations
    protected string fpsIdleAnimationName;

    protected string fpsFireAnimationName;
    protected string fpsChargeAnimationName;
    protected string fpsAltFireAnimationName;
    protected string fpsAltChargeAnimationName;
    protected string fpsReloadAnimationName;
    protected string fpsMeleeAnimationName;
    protected string fpsSwapAnimationName;

    // first person animation lengths
    protected float fpsIdleAnimatioLength;

    protected float fpsFireAnimationLength;
    protected float fpsChargeAnimationLength;
    protected float fpsAltFireAnimationLength;
    protected float fpsAltChargeAnimationLength;
    protected float fpsReloadAnimationLength;
    protected float fpsMeleeAnimationLength;
    protected float fpsSwapAnimationLength;

    // 3rd person aim animations
    protected string aimDownAnimationName;
    protected string aimUpAnimationName;

    // equip and holster variables
    protected HumanBodyBones equippedBone1;
    protected HumanBodyBones equippedBone2;
    protected HumanBodyBones holsteredBone1;
    protected HumanBodyBones holsteredBone2;

    protected int equippedFpsBone1;

    protected Vector3 equippedPositionOffset1;
    protected Vector3 equippedPositionOffset2;
    protected Vector3 equippedEulerRotationOffset1;
    protected Vector3 equippedEulerRotationOffset2;

    protected Vector3 holsteredPositionOffset1;
    protected Vector3 holsteredPositionOffset2;
    protected Vector3 holsteredEulerRotationOffset1;
    protected Vector3 holsteredEulerRotationOffset2;

    // models
    [SerializeField] protected GameObject[] models3P;
    [SerializeField] protected GameObject[] models1P;
    [SerializeField] protected GameObject[] model3PShadows;
    [SerializeField] protected GameObject gameObject1P;

    //Temp variables
    private AnimationClip tempClip;





    public virtual void initialize() { }
    public void setAsInitialized() { this.init = true; }
    public bool isInitialized() { return this.init; }






    // weapon directory setters
    public void setWeaponIndex( WeaponIndex wi ) { weaponIndex = wi; }






    // Primary fire data Setters
    public void setBulletName(string s) { bulletName = s; }
    public void setBulletDecalName(string s) { bulletDecalName = s; }
    public void setBulletSpawnDistance(float f) { bulletSpawnDistance = f; }
    public void setBulletForce(float f) { bulletForce = f; }
    public void setFireSpreadAngle(float f) { fireSpreadAngle = f; }
    public void setMultishot(float f) {
        multishot = f;
        currentMultishotCount = 0f;
    }
    public void setMultishotFanRotationAngle(float f) { multishotFanRotationAngle = f; }
    public void setFireRate(float f) { fireRate = f; }
    public void setBurstFireRate(float f)
    {
        burstFireRate = f;
        burstFireDelay = 1f / f;
    }
    public void setBurstFireCount(int i)
    {
        burstFireCount = i;
        currentBurstFireCount = 0;
    }
    public void setChargeRate(float f) { chargeRate = f; }
    public void setDamage(float f) { damage = f; }
    public void setFireType(int i) { fireType = i; }






    // Weapon action speed setters
    public void setReloadSpeed(float f) { reloadSpeed = f; }
    public void setMeleeSpeed(float f) { meleeSpeed = f; }
    public void setSwapSpeed(float f) { swapSpeed = f; }





    // Alternate fire setters
    public void setAltBulletName(string s) { altBulletName = s; }
    public void setAltBulletDecalName(string s) { altBulletDecalName = s; }
    public void setAltBulletSpawnDistance(float f) { altBulletSpawnDistance = f; }
    public void setAltBulletForce(float f) { altBulletForce = f; }
    public void setAltFireSpreadAngle(float f) { altFireSpreadAngle = f; }
    public void setAltMultishot(float f) {
        altMultishot = f;
        currentAltMultishotCount = 0f;
    }
    public void setAltMultishotFanRotationAngle(float f) { altMultishotFanRotationAngle = f; }
    public void setAltFireRate(float f) { altFireRate = f; }
    public void setAltBurstFireRate(float f)
    {
        altBurstFireRate = f;
        altBurstFireDelay = 1f / f;
    }
    public void setAltBurstFireCount(int i)
    {
        altBurstFireCount = i;
        currentAltBurstFireCount = 0;
    }
    public void setAltChargeRate(float f) { altChargeRate = f; }
    public void setAltDamage(float f) { altDamage = f; }
    public void setAltFireType(int i) { altFireType = i; }





    // Weapon ammo variable setters
    public void setAmmo(int i) { ammo = i; }
    public void setMaxAmmo(int i) { maxAmmo = i; }
    public void setClipAmmo(int i) { clipAmmo = i; }
    public void setMaxClipAmmo(int i) { maxClipAmmo = i; }
    public void setAmmoCostPerFire(int i) { ammoCostPerFire = i; }
    public void setAmmoCostPerAltFire(int i) { ammoCostPerAltFire = i; }
    public void setAutoReload(bool b) { autoReload = b; }
    public void setAutoAltReload(bool b) { autoAltReload = b; }





    // Weapon animation directory
    public void setAnimationDirectory(string s) { animationDirectory = s; }
    public void setIdleAnimationName(string s) { idleAnimationName = s; }





    // Humanoid 3rd person action animation name setters
    public void setFireAnimationName(string s) { fireAnimationName = s; }
    public void setChargeAnimationName(string s) { chargeAnimationName = s; }
    public void setAltFireAnimationName(string s) { altFireAnimationName = s; }
    public void setAltChargeAnimationName(string s) { altChargeAnimationName = s; }
    public void setReloadAnimationName(string s) { reloadAnimationName = s; }
    public void setMeleeAnimationName(string s) { meleeAnimationName = s; }
    public void setSwapAnimationName(string s) { swapAnimationName = s; }

    public void setAimDownAnimationName(string s) { aimDownAnimationName = s; }
    public void setAimUpAnimationName(string s) { aimUpAnimationName = s; }


    // Humanoid 1rst person action animation name setters
    public void setFpsIdleAnimationName(string s) { fpsIdleAnimationName = s; }

    public void setFpsFireAnimationName(string s) { fpsFireAnimationName = s; }
    public void setFpsChargeAnimationName(string s) { fpsChargeAnimationName = s; }
    public void setFpsAltFireAnimationName(string s) { fpsAltFireAnimationName = s; }
    public void setFpsAltChargeAnimationName(string s) { fpsAltChargeAnimationName = s; }
    public void setFpsReloadAnimationName(string s) { fpsReloadAnimationName = s; }
    public void setFpsMeleeAnimationName(string s) { fpsMeleeAnimationName = s; }
    public void setFpsSwapAnimationName(string s) { fpsSwapAnimationName = s; }
    



    // Equip/Holster 3rd person bone parent setters
    public void setEquippedBone1(HumanBodyBones bone) { equippedBone1 = bone; }
    public void setEquippedBone2(HumanBodyBones bone) { equippedBone2 = bone; }
    public void setHolsteredBone1(HumanBodyBones bone) { holsteredBone1 = bone; }
    public void setHolsteredBone2(HumanBodyBones bone) { holsteredBone2 = bone; }

    
    
    
    // Equip/Holster 1st person bone parent setter
    public void setEquippedFpsBone1(int i) { equippedFpsBone1 =i; }

    
    
    
    // Equip/Holster weapon rotation and position offset relative to parent setters
    public void setEquippedPositionOffset1(Vector3 v) { equippedPositionOffset1 = v; }
    public void setEquippedPositionOffset2(Vector3 v) { equippedPositionOffset2 = v; }
    public void setEquippedEulerRotationOffset1(Vector3 v) { equippedEulerRotationOffset1 = v; }
    public void setEquippedEulerRotationOffset2(Vector3 v) { equippedEulerRotationOffset2 = v; }
    public void setHolsteredPositionOffset1(Vector3 v) { holsteredPositionOffset1 = v; }
    public void setHolsteredPositionOffset2(Vector3 v) { holsteredPositionOffset2 = v; }
    public void setHolsteredEulerRotationOffset1(Vector3 v) { holsteredEulerRotationOffset1 = v; }
    public void setHolsteredEulerRotationOffset2(Vector3 v) { holsteredEulerRotationOffset2 = v; }





    // Weapon diretory & prefab setters
    public WeaponIndex GetWeaponIndex() { return weaponIndex; }





    // Primary fire property getters
    public string getBulletName() { return bulletName; }
    public string getBulletDecalName() { return bulletDecalName; }
    public float getBulletSpawnDistance() { return bulletSpawnDistance; }
    public float getBulletForce() { return bulletForce; }
    public float getFireSpreadAngle() { return fireSpreadAngle; }
    public float getMultishot() { return multishot; }
    public float getMultishotFanRotationAngle() { return multishotFanRotationAngle; }
    public float getFireRate() { return fireRate; }
    public float getBurstFireRate() { return burstFireRate; }
    public float getChargeRate() { return chargeRate; }
    public float getDamage() { return damage; }
    public int getFireType() { return fireType; }





    // weapon action speed getters
    public float getReloadSpeed() { return reloadSpeed; }
    public float getMeleeSpeed() { return meleeSpeed; }
    public float getSwapSpeed() { return swapSpeed; }





    // Alternate fire property getters
    public string getAltBulletName() { return altBulletName; }
    public string getAltBulletDecalName() { return altBulletDecalName; }
    public float getAltBulletSpawnDistance() { return altBulletSpawnDistance; }
    public float getAltFireSpreadAngle() { return altFireSpreadAngle; }
    public float getAltBulletForce() { return altBulletForce; }
    public float getAltMultishot() { return altMultishot; }
    public float getAltMultishotFanRotationAngle() { return altMultishotFanRotationAngle; }
    public float getAltFireRate() { return altFireRate; }
    public float getAltBurstFireRate() { return altBurstFireRate; }
    public float getAltChargeRate() { return chargeRate; }
    public float getAltDamage() { return altDamage; }
    public int getAltFireType() { return altFireType; }





    // ammo variable getters
    public int getAmmo() { return ammo; }
    public int getMaxAmmo() { return maxAmmo; }
    public int getClipAmmo() { return clipAmmo; }
    public int getMaxClipAmmo() { return maxClipAmmo; }
    public int getAmmoCostPerFire() { return ammoCostPerFire; }
    public int getAmmoCostPerAltFire() { return ammoCostPerAltFire; }
    public bool getAutoReload() { return autoReload; }
    public bool getAutoAltReload() { return autoAltReload; }





    // Animation directory getters
    public string getAnimationDirectory() { return animationDirectory; }
    public string getIdleAnimationName() { return idleAnimationName; }





    // 3rd person action animation name getters
    public string getFireAnimationName() { return fireAnimationName; }
    public string getChargeAnimationName() { return chargeAnimationName; }
    public string getAltFireAnimationName() { return altFireAnimationName; }
    public string getAltChargeAnimationName() { return altChargeAnimationName; }
    public string getReloadAnimationName() { return reloadAnimationName; }
    public string getMeleeAnimationName() { return meleeAnimationName; }
    public string getSwapAnimationName() { return swapAnimationName; }

    public string getAimDownAnimationName() { return aimDownAnimationName; }
    public string getAimUpAnimationName() { return aimUpAnimationName; }





    // 1st person action animation name getters
    public string getFpsIdleAnimationName() { return fpsIdleAnimationName; }

    public string getFpsFireAnimationName() { return fpsFireAnimationName; }
    public string getFpsChargeAnimationName() { return fpsChargeAnimationName; }
    public string getFpsAltFireAnimationName() { return fpsAltFireAnimationName; }
    public string getFpsAltChargeAnimationName() { return fpsAltChargeAnimationName; }
    public string getFpsReloadAnimationName() { return fpsReloadAnimationName; }
    public string getFpsMeleeAnimationName() { return fpsMeleeAnimationName; }
    public string getFpsSwapAnimationName() { return fpsSwapAnimationName; }






    // 3rd person Equip/Holster parent bone getters
    public HumanBodyBones getEquippedBone1() { return equippedBone1; }
    public HumanBodyBones getEquippedBone2() { return equippedBone2; }
    public HumanBodyBones getHolsteredBone1() { return holsteredBone1; }
    public HumanBodyBones getHolsteredBone2() { return holsteredBone2; }





    // 1st person Equip/Holster parent bone getters
    public int getEquippedFpsBone1() { return equippedFpsBone1; }





    // Equip/Holster weapon rotation and position offset relative to parent setters
    public Vector3 getEquippedPositionOffset1() { return equippedPositionOffset1; }
    public Vector3 getEquippedPositionOffset2() { return equippedPositionOffset2; }
    public Vector3 getEquippedEulerRotationOffset1() { return equippedEulerRotationOffset1; }
    public Vector3 getEquippedEulerRotationOffset2() { return equippedEulerRotationOffset2; }
    public Vector3 getHolsteredPositionOffset1() { return holsteredPositionOffset1; }
    public Vector3 getHolsteredPositionOffset2() { return holsteredPositionOffset2; }
    public Vector3 getHolsteredEulerRotationOffset1() { return holsteredEulerRotationOffset1; }
    public Vector3 getHolsteredEulerRotationOffset2() { return holsteredEulerRotationOffset2; }






    // 3rd person animation clip getters
    public AnimationClip getIdleAnimation() { return GameManager.loadAnimationClip(idleAnimationName, animationDirectory) ; }

    public AnimationClip getFireAnimation()
    {
        tempClip = GameManager.loadAnimationClip(fireAnimationName, animationDirectory);
        if (tempClip) fireAnimationLength = tempClip.length;
        return tempClip;
    }
    public AnimationClip getChargeAnimation()
    {
        tempClip = GameManager.loadAnimationClip(chargeAnimationName, animationDirectory);
        if (tempClip) chargeAnimationLength = tempClip.length;
        return tempClip;
    }
    public AnimationClip getAltFireAnimation()
    {
        tempClip = GameManager.loadAnimationClip(altFireAnimationName, animationDirectory);
        if (tempClip) altFireAnimationLength = tempClip.length;
        return tempClip;
    }
    public AnimationClip getAltChargeAnimation()
    {
        tempClip = GameManager.loadAnimationClip(altChargeAnimationName, animationDirectory);
        if (tempClip) altChargeAnimationLength = tempClip.length;
        return tempClip;
    }
    public AnimationClip getReloadAnimation()
    {
        tempClip = GameManager.loadAnimationClip(reloadAnimationName, animationDirectory);
        if (tempClip) reloadAnimationLength = tempClip.length;
        return tempClip;
    }
    public AnimationClip getMeleeAnimation()
    {
        tempClip = GameManager.loadAnimationClip(meleeAnimationName, animationDirectory);
        if (tempClip) meleeAnimationLength = tempClip.length;
        return tempClip;
    }
    public AnimationClip getSwapAnimation()
    {
        tempClip = GameManager.loadAnimationClip(swapAnimationName, animationDirectory);
        if (tempClip) swapAnimationLength = tempClip.length;
        return tempClip;
    }






    public AnimationClip getAimDownAnimation() { return GameManager.loadAnimationClip(aimDownAnimationName, animationDirectory); }
    public AnimationClip getAimUpAnimation() { return GameManager.loadAnimationClip(aimUpAnimationName, animationDirectory); }






    // 1st person animation clip getters
    public AnimationClip getFpsIdleAnimation() { return GameManager.loadAnimationClip(fpsIdleAnimationName, animationDirectory); }

    public AnimationClip getFpsFireAnimation()
    {
        tempClip = GameManager.loadAnimationClip(fpsFireAnimationName, animationDirectory);
        if (tempClip) fpsFireAnimationLength = tempClip.length;
        return tempClip;
    }
    public AnimationClip getFpsChargeAnimation()
    {
        tempClip = GameManager.loadAnimationClip(fpsChargeAnimationName, animationDirectory);
        if (tempClip) fpsChargeAnimationLength = tempClip.length;
        return tempClip;
    }
    public AnimationClip getFpsAltFireAnimation()
    {
        tempClip = GameManager.loadAnimationClip(fpsAltFireAnimationName, animationDirectory);
        if (tempClip) fpsAltFireAnimationLength = tempClip.length;
        return tempClip;
    }
    public AnimationClip getFpsAltChargeAnimation()
    {
        tempClip = GameManager.loadAnimationClip(fpsAltChargeAnimationName, animationDirectory);
        if (tempClip) fpsAltChargeAnimationLength = tempClip.length;
        return tempClip;
    }
    public AnimationClip getFpsReloadAnimation()
    {
        tempClip = GameManager.loadAnimationClip(fpsReloadAnimationName, animationDirectory);
        if (tempClip) fpsReloadAnimationLength = tempClip.length;
        return tempClip;
    }
    public AnimationClip getFpsMeleeAnimation()
    {
        tempClip = GameManager.loadAnimationClip(fpsMeleeAnimationName, animationDirectory);
        if (tempClip) fpsMeleeAnimationLength = tempClip.length;
        return tempClip;
    }
    public AnimationClip getFpsSwapAnimation()
    {
        tempClip = GameManager.loadAnimationClip(fpsSwapAnimationName, animationDirectory);
        if(tempClip) fpsSwapAnimationLength = tempClip.length;
        return tempClip;
    }







    // get 3rd person animation clip lengths
    public float getFireAnimationLength() { return fireAnimationLength; }
    public float getChargeAnimationLength() { return chargeAnimationLength; }
    public float getAltFireAnimationLength() { return altFireAnimationLength; }
    public float getAltChargeAnimationLength() { return altChargeAnimationLength; }
    public float getReloadAnimationLength() { return reloadAnimationLength; }
    public float getMeleeAnimationLength() { return meleeAnimationLength; }
    public float getSwapAnimationLength() { return swapAnimationLength; }





    // get 1st person animation clip lengths
    public float getFpsFireAnimationLength() { return fpsFireAnimationLength; }
    public float getFpsChargeAnimationLength() { return fpsChargeAnimationLength; }
    public float getFpsAltFireAnimationLength() { return fpsAltFireAnimationLength; }
    public float getFpsAltChargeAnimationLength() { return fpsAltChargeAnimationLength; }
    public float getFpsReloadAnimationLength() { return fpsReloadAnimationLength; }
    public float getFpsMeleeAnimationLength() { return fpsMeleeAnimationLength; }
    public float getFpsSwapAnimationLength() { return fpsSwapAnimationLength; }



    public WeaponTransferData getTranferData()
    {
        return new WeaponTransferData( weaponIndex, ammo, clipAmmo );
    }


    void setAll1PModelLayers(int layer)
    {
        foreach (GameObject go in models1P)
            go.layer = layer;
    }
    void setAll3PModelLayers(int layer)
    {
        foreach (GameObject go in models3P)
            go.layer = layer;
    }
    void setAll3PModelShadowLayers(int layer)
    {
        foreach (GameObject go in model3PShadows)
            go.layer = layer;
    }

    void setActiveAll1PModels(bool b)
    {
        foreach (GameObject go in models1P)
            go.SetActive(b);
    }

    public void setModelLayers(int player)
    {

        setAll1PModelLayers(GameManager.get1PLayer(player));
        setAll3PModelLayers(GameManager.get3PLayer(player));
        setAll3PModelShadowLayers(GameManager.get1PLayer(player));

    }

    public void resetModelLayers()
    {
        setAll1PModelLayers(0);
        setAll3PModelLayers(0);
        setAll3PModelShadowLayers(0);
    }

    public void equip1P(Transform bone)
    {
        setActiveAll1PModels(true);
        gameObject1P.transform.SetParent(bone);
        gameObject1P.transform.localPosition = equippedPositionOffset1;
        gameObject1P.transform.localRotation = Quaternion.Euler(equippedEulerRotationOffset1);
    }

    public void equip3P(Transform bone)
    {
        transform.SetParent(bone);
        transform.localPosition = equippedPositionOffset1;
        transform.localRotation = Quaternion.Euler(equippedEulerRotationOffset1);
    }

    public void holster()
    {
        gameObject1P.transform.SetParent(transform);
        gameObject1P.transform.localPosition.Set(0f, 0f, 0f);
        gameObject1P.transform.localRotation.Set(0f, 0f, 0f, 1f);
        setActiveAll1PModels(false);
        //model3PShadow.enabled = false;
    }





    public bool canReload()
    {
        return !(ammo <= 0 | fullClip());
    }

    public void reload()
    {
        currentAltBurstFireCount = 0;
        currentBurstFireCount = 0;
        int clipAmmoDelta = maxClipAmmo - clipAmmo;
        if (ammo - clipAmmoDelta <= 0)
        {
            clipAmmo = ammo + clipAmmo;
            ammo = 0;
            return;
        }

        clipAmmo += clipAmmoDelta;
        ammo -= clipAmmoDelta;
        
    }

    public bool fullClip()
    {
        return (clipAmmo >= maxClipAmmo);
    }

    public bool canFire()
    {
        if (autoReload)
            return ( ammo + clipAmmo - ammoCostPerFire >= 0 );
        else
            return ( clipAmmo - ammoCostPerFire >= 0);
    }

    public bool canAltFire()
    {
        if (autoAltReload)
            return (ammo + clipAmmo - ammoCostPerAltFire >= 0 );
        else
            return (clipAmmo - ammoCostPerAltFire >= 0);
    }






    public int altFire(
        bool fire,
        bool fireEnabled, 
        Vector3 relSpawnPos, 
        Vector3 spawnPosOffset,
        Vector3 lookPoint, 
        int playerNumber, 
        GameObject sourceGameObject)
    {
        updateAltTriggerPulled(fire);
        if (altFireType < -1) return 0;

        if(altFireType == -1)
        {
            if (altTriggerPulled)
            {
                return -1;
            }
            return 0;
        }

        int fireState = 0;
        bool spawn = false;

        if (canAltFire())
        {
            if (!altFired && fireEnabled)
            {
                switch (altFireType)
                {
                    case 1:
                        if (altTriggerPulled)
                        {
                            currentAltMultishotCount = (int)altMultishot;
                            spawn = true;
                            altFired = false;
                        }
                        altChargeAmount = 1f;
                        break;
                    case 2:
                        if (justAltTriggerReleased)
                        {
                            spawn = true;
                            altFired = true;
                            currentAltMultishotCount = (int)altMultishot;
                            currentAltBurstFireCount = altBurstFireCount;
                            altBurstFireTimer = altBurstFireDelay;
                            break;
                        }

                        if (altTriggerPulled)
                        {
                            altChargeAmount += Time.fixedDeltaTime * altChargeRate;
                            if (altChargeAmount > 1f) { altChargeAmount = 1f; }
                            fireState = 1;
                        }
                        else
                        {
                            altChargeAmount = 0;
                        }
                        break;
                    default:
                        if (justAltTriggerPulled)
                        {
                            spawn = true;
                            altFired = true;
                            currentAltMultishotCount = (int)altMultishot;
                            currentAltBurstFireCount = altBurstFireCount;
                            altBurstFireTimer = altBurstFireDelay;
                            altChargeAmount = 1f;
                        }
                        altChargeAmount = 1f;
                        break;
                }
            }
            else
            {
                if (currentAltBurstFireCount > 0)
                {
                    if (altBurstFireTimer == 0f)
                    {
                        altBurstFireTimer = altBurstFireDelay;
                        currentAltMultishotCount = (int)altMultishot;
                        currentAltBurstFireCount--;

                        spawn = true;
                        altFired = !(currentAltBurstFireCount == 0);
                    }
                    else
                    {
                        altBurstFireTimer -= Time.fixedDeltaTime;
                        if (altBurstFireTimer <= 0) altBurstFireTimer = 0f;
                    }

                }
                else
                {
                    currentAltBurstFireCount = 0;
                    altFired = false;
                }

            }

            if (spawn)
            {
                while(currentAltMultishotCount>0)
                {
                    spawnAltBullet(relSpawnPos, spawnPosOffset, lookPoint, playerNumber, sourceGameObject);
                    currentAltMultishotCount--;
                }

                fireState = 2;

                clipAmmo -= ammoCostPerAltFire;
                if (autoAltReload && canReload())
                {
                    reload();
                }

                if (canAltFire())
                {
                    altFired = true;
                }
                else
                {
                    altFired = false;
                    currentAltBurstFireCount = 0;
                }

            }

        }

        return fireState;
    }

    public int fire(
        bool fire, 
        bool fireEnabled, 
        Vector3 relSpawnPos, 
        Vector3 spawnPosOffset, 
        Vector3 lookPoint, 
        int playerNumber, 
        GameObject sourceGameObject)
    {
        updateTriggerPulled(fire);

        bool spawn = false;
        int fireState = 0;
        if (canFire())
        {
            if (!fired && fireEnabled)
            {
                switch (fireType)
                {
                    case 1:
                        if (triggerPulled)
                        {
                            currentMultishotCount = (int)multishot;
                            spawn = true;
                            fired = true;
                        }
                        chargeAmount = 1f;
                        break;
                    case 2:
                        if (justTriggerReleased)
                        {
                            spawn = true;
                            fired = true;
                            currentMultishotCount = (int)multishot;
                            currentBurstFireCount = burstFireCount;
                            burstFireTimer = burstFireDelay;
                            break;
                        }

                        if (triggerPulled)
                        {
                            chargeAmount += Time.fixedDeltaTime * chargeRate;
                            if (chargeAmount > 1f) { chargeAmount = 1f; }
                            fireState = 1;
                        }
                        else
                        {
                            chargeAmount = 0;
                        }
                        break;
                    default:
                        if (justTriggerPulled)
                        {
                            spawn = true;
                            fired = true;
                            currentMultishotCount = (int)multishot;
                            currentBurstFireCount = burstFireCount;
                            burstFireTimer = burstFireDelay; ;
                            chargeAmount = 1f;
                        }
                        chargeAmount = 1f;
                        break;
                }
            }
            else
            {
                if( currentBurstFireCount>0 )
                {
                    if (burstFireTimer == 0f)
                    {
                        burstFireTimer = burstFireDelay;
                        currentMultishotCount = (int)multishot;
                        currentBurstFireCount--;
                        
                        spawn = true;
                        fired = !(currentBurstFireCount == 0);
                    }
                    else
                    {
                        if(!spawn) burstFireTimer -= Time.fixedDeltaTime;
                        if (burstFireTimer <= 0) burstFireTimer = 0f;
                    }
                    
                }
                else
                {
                    currentBurstFireCount = 0;
                    fired = false;
                }
                
            }

            if (spawn)
            {
                while (currentMultishotCount>0)
                {
                    spawnBullet(relSpawnPos, spawnPosOffset, lookPoint, playerNumber, sourceGameObject);
                    currentMultishotCount--;
                }
                    
                fireState = 2;

                clipAmmo -= ammoCostPerFire;
                if (autoReload && canReload())
                {
                    reload();
                }


                if (canFire())
                {
                    fired = true;
                }
                else
                {
                    fired = false;
                    currentBurstFireCount = 0;
                }
            }
                
        }
        return fireState;

    }

    public void spawnBullet(Vector3 relSpawnPos, Vector3 spawnPosOffset, Vector3 lookPoint, int playerNumber, GameObject sourceGameObject)
    {
        //Debug.Log(getWeaponDirectory()+getBulletName());
        GameObject bullet = GameManager.loadPrefab(getBulletName(),  ItemHelper.getWeaponPrefabDirectory(weaponIndex));
        bullet.SetActive(false);
        bullet.transform.SetParent(null);

        BulletData bd = bullet.GetComponent<BulletData>();
        //Debug.Log(bullet);
        bd.enabled = false;
        bd.resetData();

        Vector3 spawnPos = spawnPosOffset + relSpawnPos * getBulletSpawnDistance();

        Vector3 bulletDir = relSpawnPos;
        if (lookPoint.magnitude > 0) bulletDir = lookPoint - spawnPos;

        //apply spread
        Quaternion spreadRotation = new Quaternion();
        if (fireSpreadAngle > 0)
        {
            if (multishotFanRotationAngle > 0 && (int)getMultishot()>0)
            {
                spreadRotation =
                    Quaternion.Euler(0, 0, getMultishotFanRotationAngle())
                    * Quaternion.Euler(((2f * currentMultishotCount / (int)(1 + getMultishot())) - 1f) * getFireSpreadAngle(), 0, 0);
            }
            else
            {
                spreadRotation =
                    Quaternion.Euler(0, 0, Random.value * 180f)
                    * Quaternion.Euler((2f * Random.value - 1f) * getFireSpreadAngle(), 0, 0);
            }

            bulletDir =
                Quaternion.LookRotation(bulletDir)
                * spreadRotation
                * new Vector3(0, 0, 1);
        }

        bulletDir.Normalize();

        bullet.transform.position = spawnPos;
        bullet.transform.rotation = Quaternion.LookRotation(bulletDir );
        bullet.transform.localScale = new Vector3(1f,1f,1f);



        bd.setSourceEntityGameObject(sourceGameObject);
        bd.setSourcePlayer(playerNumber);

        bd.setImpactForce(bulletForce);
        bd.setDirection( bulletDir );
        bd.setDamage( getDamage()*chargeAmount );
        if(currentBurstFireCount==0 && currentMultishotCount==0) chargeAmount = 0f;
        bd.enabled = true;
        bullet.SetActive(true);
    }

    public void spawnAltBullet(Vector3 relSpawnPos, Vector3 spawnPosOffset, Vector3 lookPoint, int playerNumber, GameObject sourceGameObject)
    {
        GameObject bullet = GameManager.loadPrefab(getAltBulletName(), ItemHelper.getWeaponPrefabDirectory(weaponIndex));
        bullet.SetActive(false);
        bullet.transform.SetParent(null);

        BulletData bd = bullet.GetComponent<BulletData>();
        //Debug.Log(bullet);
        bd.enabled = false;
        bd.resetData();

        Vector3 spawnPos = spawnPosOffset + relSpawnPos * getAltBulletSpawnDistance();

        Vector3 bulletDir = relSpawnPos;
        if (lookPoint.magnitude > 0) bulletDir = lookPoint - spawnPos;

        //apply spread
        Quaternion spreadRotation = new Quaternion();
        if (this.altFireSpreadAngle > 0)
        {
            if (altMultishotFanRotationAngle > 0 && (int)getAltMultishot() > 0)
            {
                spreadRotation = 
                    Quaternion.Euler(0, 0, getAltMultishotFanRotationAngle())
                    * Quaternion.Euler( ( (2f * currentAltMultishotCount/(int)(1+getAltMultishot())) - 1f ) * getAltFireSpreadAngle(), 0, 0);
            }
            else
            {
                spreadRotation = 
                    Quaternion.Euler(0, 0, Random.value * 180f) 
                    * Quaternion.Euler( (2f * Random.value - 1f) * getAltFireSpreadAngle(), 0, 0 );
            }

            bulletDir =
                Quaternion.LookRotation(bulletDir)
                * spreadRotation
                * new Vector3(0, 0, 1);
        }
            
        bulletDir.Normalize();

        bullet.transform.position = spawnPos;
        bullet.transform.rotation = Quaternion.LookRotation(bulletDir);
        bullet.transform.localScale = new Vector3(1f, 1f, 1f);



        bd.setSourceEntityGameObject(sourceGameObject);
        bd.setSourcePlayer(playerNumber);


        bd.setImpactForce(altBulletForce);
        bd.setDirection(bulletDir);
        bd.setDamage(getAltDamage() * altChargeAmount);
        if (currentAltBurstFireCount == 0 && currentAltMultishotCount==0) altChargeAmount = 0f;
        bd.enabled = true;
        bullet.SetActive(true);
    }





    public void updateTriggerPulled(bool b)
    {
        triggerPulledLast = triggerPulled;
        triggerPulled = b;

        justTriggerPulled = false;
        justTriggerPulled = (b && !triggerPulledLast);

        justTriggerReleased = false;
        justTriggerReleased = (!b && triggerPulledLast);

    }

    public void updateAltTriggerPulled(bool b)
    {
        altTriggerPulledLast = altTriggerPulled;
        altTriggerPulled = b;

        justAltTriggerPulled = false;
        if (b && !altTriggerPulledLast) justAltTriggerPulled = true;

        this.justAltTriggerReleased = false;
        if (!b && altTriggerPulledLast) justAltTriggerReleased = true;

    }

    public void resetTriggerState()
    {
        triggerPulledLast = false;
        triggerPulled = false;
        justTriggerPulled = false;
        justTriggerReleased = false;
    }

    public void resetAltTriggerState()
    {
        altTriggerPulledLast = false;
        altTriggerPulled = false;
        justAltTriggerPulled = false;
        justAltTriggerReleased = false;
    }

}
