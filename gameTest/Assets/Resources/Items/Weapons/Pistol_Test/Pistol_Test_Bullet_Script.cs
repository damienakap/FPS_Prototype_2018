using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol_Test_Bullet_Script : MonoBehaviour
{

    public BulletData data;

    bool destroyThis = false;
    
    List<GameObject> hitObjects = new List<GameObject>();

    private void Awake()
    {
        data.setSpeed(600f);
        data.setDamage(10f);
        data.setDamageType(0);
        data.setMaxTravelDistance(200);
        data.setDistanceTraveled(0f);
        data.setPenetrationValue(1);
    }

    private void FixedUpdate()
    {
        float distanceTraveled = data.getDistanceTraveled();

        transform.rotation = Quaternion.LookRotation(data.getDirection());

        Ray ray = new Ray();
        RaycastHit hit;
        ray.origin = transform.position;
        ray.direction = data.getDirection();

        LayerMask mask = GameManager.getBulletCollisionLayerMask();
        
        if (GameManager.playerNumberIsValid(data.getSourcePlayerNumber()))
        {
            mask |= data.getPlayerLayerMask();
        }
        else
        {
            mask |= GameManager.getAllPlayersLayerMask();
        }
            

        float descreteTravelDistance = data.getSpeed() * Time.fixedDeltaTime;

        Vector3 move = descreteTravelDistance * data.getDirection();

        float offset = 0.1f;

        float distToHit = 0f;
        float currentDistanceTraveled = 0f;

        HitBoxScript hitBoxScriptTemp = null;
        bool spawnDecal = true;

        for ( int i=0; i<6; i++)
        {
            //Debug.Log(currentDistanceTraveled);
            if (Physics.Raycast(ray, out hit, offset + descreteTravelDistance - currentDistanceTraveled, mask))
            {
                spawnDecal = true;
                distToHit = (hit.point - transform.position).magnitude;

                GameObject hitObject = hit.transform.gameObject;

                if (!hitObjects.Contains(hitObject))
                {

                    hitObjects.Add(hitObject);
                    if (data.getPenetrationValue() >= 1f)
                    {
                        data.setPenetrationValue(data.getPenetrationValue() - 1f);
                    }
                    else
                    {
                        hitObjects.Clear();
                        destroyThis = true;
                    }

                    hitBoxScriptTemp = hitObject.GetComponent<HitBoxScript>();
                    if (hitBoxScriptTemp != null)
                    {
                        hitBoxScriptTemp.applyDamage(data.getDamage(), data.getDamageType());
                        //spawnDecal = false;
                    }

                    Rigidbody hitRigidBody = hitObject.GetComponent<Rigidbody>();
                    if (hitRigidBody != null)
                    {
                        hitRigidBody.AddForceAtPosition(-hit.normal * data.getImpactForce(), hit.point, ForceMode.Force);
                    }

                    if (spawnDecal)
                    {
                        GameObject decal = GameManager.loadPrefab("Bullet_Decal", "Items/Weapons/Pistol_Test/");
                        decal.transform.position = hit.point + hit.normal * 0.01f;
                        decal.transform.rotation = Quaternion.LookRotation(hit.normal) * Quaternion.Euler(90f, 0f, 0f) * Quaternion.AngleAxis(Random.value * 360, new Vector3(0, 1, 0));
                        decal.transform.localScale = new Vector3(.005f, .005f, .005f);
                        BulletDecalScript decalScript = decal.GetComponent<BulletDecalScript>();
                        decalScript.enabled = true;
                        decalScript.lifeTime = GameManager.getBulletDecalLifeTime();
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

        data.setDistanceTraveled(distanceTraveled);

        if (distanceTraveled >= data.getMaxTravelDistance()) { destroyThis = true; }

        if (destroyThis) {
            gameObject.SetActive(false);
            this.enabled = false;
            Destroy(gameObject, 1f);
        }

    }
    
}
