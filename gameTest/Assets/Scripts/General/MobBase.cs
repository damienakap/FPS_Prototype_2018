using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobBase : MonoBehaviour
{

    public MobData mobData;

    protected Vector3 lookDirection = new Vector3();
    protected Vector3 lookPoint = new Vector3();
    protected Vector3 bulletSpawnCenter = new Vector3();

    public virtual void Update()
    {
        mobData.currentRegenCooldown += Time.deltaTime;
        if(mobData.currentRegenCooldown >= mobData.regenCooldown)
        {
            mobData.currentRegenCooldown = mobData.regenCooldown;

            if(mobData.health < mobData.baseHealth){ mobData.health += mobData.healthRegenRate * Time.deltaTime; }
            if(mobData.shields < mobData.baseShields) { mobData.shields += mobData.shieldRegenRate * Time.deltaTime; }

            if (mobData.health > mobData.baseHealth) { mobData.health = mobData.baseHealth; }
            if (mobData.shields > mobData.baseShields) { mobData.shields = mobData.baseShields; }

        }
    }

    /// <summary>
    /// Apply damage of a given damage type to this entity.
    /// </summary>
    /// <param name="damage">Damage Value</param>
    /// <param name="type">Damage Type</param>
    public void applyDamage( float damage, int type )
    {
        mobData.currentRegenCooldown = 0f;
        float armorRuduction = mobData.armor / (mobData.armor + 300);

        switch (type)
        {
            default:
                mobData.shields -= damage;
                if (mobData.shields < 0)
                {
                    damage = -mobData.shields;
                    mobData.shields = 0;
                }
                if (mobData.shields <= 0){
                    //Debug.Log(damage);
                    mobData.health -= damage * armorRuduction;
                }
                
                break;
        }

        if (mobData.health < 0f) { mobData.health = 0f; }

    }

    public float getHealth() { return mobData.health; }
    public float getShields() { return mobData.shields; }
    public float getScaledHealth() { return mobData.health / mobData.baseHealth; }
    public float getScaledShields() { return mobData.shields / mobData.baseShields; }

    public int getEntityType() { return mobData.mobType; }

    public virtual Vector3 getLookDirection() { return lookDirection; }
    public virtual Vector3 getLookPoint() { return lookPoint; }
    public virtual Vector3 getBulletSpawnCenter() { return bulletSpawnCenter; }

}
