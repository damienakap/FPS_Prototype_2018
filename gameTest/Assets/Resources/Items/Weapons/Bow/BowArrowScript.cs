using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowArrowScript : BulletData
{
    GameObject decal = null;

    bool destroyThis = false;

    List<GameObject> hitObjects = new List<GameObject>();

    public override void resetData()
    {
        base.resetData();
        setSpeed(300f);
        setDamage(10f);
        setDamageType(0);
        setMaxTravelDistance(200);
        setDistanceTraveled(0f);
        setPenetrationValue(0f);
        transform.parent = null;
    }

    private void Awake()
    {
        resetData();
    }

    private void FixedUpdate()
    {

        float distanceTraveled = getDistanceTraveled();

        transform.rotation = Quaternion.LookRotation(getDirection());

        Ray ray = new Ray();
        RaycastHit hit;
        ray.origin = transform.position;
        ray.direction = getDirection();

        LayerMask mask = GameManager.getBulletCollisionLayerMask();

        if (GameManager.playerNumberIsValid(getSourcePlayerNumber()))
        {
            mask |= getPlayerLayerMask();
        }


        float descreteTravelDistance = getSpeed() * Time.fixedDeltaTime;

        Vector3 move = descreteTravelDistance * getDirection();

        float offset = 0.1f;

        float distToHit = 0f;
        float currentDistanceTraveled = 0f;

        HitBoxScript hitBoxScriptTemp = null;

        for (int i = 0; i < 6; i++)
        {
            //Debug.Log(currentDistanceTraveled);
            if (Physics.Raycast(ray, out hit, offset + descreteTravelDistance - currentDistanceTraveled, mask))
            {
                distToHit = (hit.point - transform.position).magnitude;

                GameObject hitObject = hit.transform.gameObject;

                if (!hitObjects.Contains(hitObject))
                {

                    hitObjects.Add(hitObject);
                    if (getPenetrationValue() >= 1f)
                    {
                        setPenetrationValue(getPenetrationValue() - 1f);
                    }
                    else
                    {
                        hitObjects.Clear();
                        destroyThis = true;
                    }

                    Rigidbody hitRigidBody = hitObject.GetComponent<Rigidbody>();
                    if (hitRigidBody != null)
                    {
                        hitRigidBody.AddForceAtPosition(-hit.normal * getImpactForce(), hit.point, ForceMode.Force);
                    }

                    hitBoxScriptTemp = hitObject.GetComponent<HitBoxScript>();
                    if (hitBoxScriptTemp != null)
                    {
                        hitBoxScriptTemp.applyDamage(getDamage(), getDamageType());
                        //spawnDecal = false;
                    }

                    if (!destroyThis)
                    {
                        decal = GameManager.loadPrefab("Bullet_Decal", "Items/Weapons/Pistol_Test/");
                        decal.transform.position = hit.point + hit.normal * 0.01f;
                        decal.transform.rotation = Quaternion.LookRotation(hit.normal) * Quaternion.Euler(90f, 0f, 0f) * Quaternion.AngleAxis(Random.value * 360, new Vector3(0, 1, 0));
                        BulletDecalScript decalScript = decal.GetComponent<BulletDecalScript>();
                        decalScript.enabled = true;

                        decalScript.resetDecal();

                        decal.transform.parent = hit.transform;
                        decal.SetActive(true);
                    }
                    else
                    {
                        decal = GameManager.loadPrefab("Arrow_Decal", "Items/Weapons/Bow/");
                        decal.transform.position = hit.point + hit.normal * 0.01f;
                        decal.transform.rotation = Quaternion.LookRotation(hit.normal) * Quaternion.Euler(90f, 0f, 0f) * Quaternion.AngleAxis(Random.value * 360, new Vector3(0, 1, 0));
                        BulletDecalScript decalScript = decal.GetComponent<BulletDecalScript>();
                        decalScript.enabled = true;

                        decalScript.resetDecal();
                        decalScript.setDecalRotation(transform.rotation);

                        decal.transform.parent = hit.transform;
                        decal.SetActive(true);

                        
                    }
                    


                    currentDistanceTraveled += distToHit + 0.0001f;
                    ray.origin = transform.position + currentDistanceTraveled * (hit.point - transform.position).normalized;
                    
                    if (destroyThis) { break; }
                    

                }
                else
                {
                    transform.position += move * 0.99999f;
                    distanceTraveled += move.magnitude * 0.99999f;
                    break;
                }

            }
            else
            {
                transform.position += move * 0.99999f;
                distanceTraveled += move.magnitude * 0.99999f;
                break;
            }
        }

        setDistanceTraveled(distanceTraveled);

        if (distanceTraveled >= getMaxTravelDistance()) { destroyThis = true; }

        if (destroyThis)
        {
            gameObject.SetActive(false);
            this.enabled = false;
            Destroy(gameObject, 1f);
        }

    }

}
