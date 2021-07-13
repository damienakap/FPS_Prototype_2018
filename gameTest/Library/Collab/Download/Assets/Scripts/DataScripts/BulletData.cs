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
    protected float spawnDistance = 1f;

    protected GameObject sourceEntityGameObject;

    protected int playerNumber = 0;
    protected LayerMask playerLayerMask;

    protected float impactForce = 100f;
    protected float maxTravelDistance = 500f;

    protected float distanceTraveled = 0f;

    public virtual void resetData() { }

    public void setFaction(string s) { faction = s; }
    public void setDirection(Vector3 v) { direction = v; direction.Normalize(); }
    public void setSpawnDistance( float f) { spawnDistance = f; }
    public void setSpeed(float f) { speed = f; }
    public void setDamage(float f) { damage = f; }
    public void setDamageType(int i) { damageType = i; }
    public void setPenetrationValue(float f) { penetrationValue = f; }
    public void setMaxTravelDistance(float f) { maxTravelDistance = f; }
    public void setImpactForce(float f) { impactForce = f; }
    public void setDistanceTraveled(float f) { distanceTraveled = f; }
    public void setSourceEntityGameObject(GameObject go) { sourceEntityGameObject = go; }
    public void setSourcePlayer(int i) {
        if (GameManager.playerNumberIsValid(i))
        {
            playerNumber = i;
            playerLayerMask = GameManager.getPlayerLayerMask(i);
        }
        else
        {
            playerNumber = 0;
            playerLayerMask = 0;
        }
        
    }

    public string getFaction() { return faction; }
    public Vector3 getDirection() { return direction; }
    public float getSpeed() { return speed; }
    public float getSpawnDistance() { return spawnDistance; }
    public float getDamage() { return damage; }
    public int getDamageType() { return damageType; }
    public float getPenetrationValue() { return penetrationValue; }
    public float getMaxTravelDistance() { return maxTravelDistance; }
    public float getImpactForce() { return impactForce; }
    public float getDistanceTraveled() { return distanceTraveled; }
    public GameObject getSourceEntityGameObject() { return sourceEntityGameObject; }
    public int getSourcePlayerNumber() { return playerNumber; }
    public LayerMask getPlayerLayerMask() { return playerLayerMask; }
    
}
