using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletData : MonoBehaviour
{
    protected string faction = "null";
    protected Vector3 direction = new Vector3(0f, 0f, 1f);
    protected float speed = 1f;

    protected float damage = 0f;
    protected int damageType = 0;
    protected float penetrationValue = 0f;

    
    protected float impactForce = 100f;
    protected float maxTravelDistance = 500f;

    protected float distanceTraveled = 0f;

    public void setFaction(string s) { this.faction = s; }
    public void setDirection(Vector3 v) { this.direction = v; this.direction.Normalize(); }
    public void setSpeed(float f) { this.speed = f; }
    public void setDamage(float f) { this.damage = f; }
    public void setDamageType(int i) { this.damageType = i; }
    public void setPenetrationValue(float f) { this.penetrationValue = f; }
    public void setMaxTravelDistance(float f) { this.maxTravelDistance = f; }
    public void setImpactForce(float f) { this.impactForce = f; }
    public void setDistanceTraveled(float f) { this.distanceTraveled = f; }

    public string getFaction() { return this.faction; }
    public Vector3 getDirection() { return this.direction; }
    public float getSpeed() { return this.speed; }
    public float getDamage() { return this.damage; }
    public int getDamageType() { return this.damageType; }
    public float getPenetrationValue() { return this.penetrationValue; }
    public float getMaxTravelDistance() { return this.maxTravelDistance; }
    public float getImpactForce() { return this.impactForce; }
    public float getDistanceTraveled() { return this.distanceTraveled; }

}
