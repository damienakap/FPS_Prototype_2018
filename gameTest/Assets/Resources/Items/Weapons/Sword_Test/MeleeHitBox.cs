using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitBox : MonoBehaviour
{

    public BulletData data;
    MobBase parentEntity = null;

    public HitTriggerCollider hitBox;
    public float delay = 0.2f;
    private float delayTimer = 0f;
    
    private Rigidbody rigidbody;

    private List<GameObject> hitGameObjects = new List<GameObject>();

    public float maxTravelDistance = 0;
    public float moveSpeed = 0;

    // Start is called before the first frame update
    void Start()
    {
        reset();
    }

    public void reset()
    {
        delayTimer = delay;
        parentEntity = data.getSourceEntityGameObject().GetComponent<MobBase>();
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = new Vector3();
        rigidbody.angularVelocity = new Vector3();

        data.setDistanceTraveled(0);
        data.setMaxTravelDistance(maxTravelDistance);
        data.setSpeed(moveSpeed);

        hitGameObjects.Clear();

    }

    private void FixedUpdate()
    {

        delayTimer -= Time.deltaTime;
        if (delayTimer <= 0)
        {
            delayTimer = 0;

            if (hitBox.triggered)
            {

                HitBoxScript hitBoxHit;
                Rigidbody rigid;
                Collider[] hitObjects = hitBox.getHitObjects();
                bool hit = false;

                for (int i = 0; i < hitObjects.Length; i++)
                {
                    hitBoxHit = hitObjects[i].GetComponent<HitBoxScript>();
                    rigid = hitObjects[i].attachedRigidbody;
                    hit = false;
                    if (!hitGameObjects.Contains(hitObjects[i].gameObject))
                    {
                        hitGameObjects.Add(hitObjects[i].gameObject);

                        if (hitBoxHit != null)
                        {
                            if (hitBoxHit.getParentEntity().gameObject != data.getSourceEntityGameObject())
                            {
                                hitBoxHit.applyDamage(data.getDamage(), data.getDamageType());
                                applyForce(rigid, hitObjects[i].ClosestPoint(transform.position));
                            }
                            else
                            {
                                hit = true;
                            }

                        }

                        if (rigid && !hit)
                        {
                            applyForce(rigid, hitObjects[i].ClosestPoint(transform.position));
                        }
                    }
                    
                }

            }


            data.setDistanceTraveled(data.getDistanceTraveled() + rigidbody.velocity.magnitude * Time.fixedDeltaTime);
            if (data.getDistanceTraveled() >= data.getMaxTravelDistance())
            {
                Destroy(gameObject);
                return;
            }

            rigidbody.velocity = data.getDirection() * data.getSpeed();
            rigidbody.angularVelocity = new Vector3();


        }
        else
        {
            if (data.getSourceEntityGameObject())
            {
                if (parentEntity)
                {
                    transform.position = parentEntity.getBulletSpawnCenter() + parentEntity.getLookDirection() * data.getSpawnDistance();
                    data.setDirection((parentEntity.getLookPoint() - transform.position).normalized);
                    transform.rotation = Quaternion.LookRotation(data.getDirection());
                }
            }
            else
                Destroy(gameObject);

        }

        
    }

    void applyForce(Rigidbody rigid, Vector3 pos)
    {
        Vector3 force;
        force = data.getDirection();
        force.Normalize();
        force *= data.getImpactForce();
        rigid.AddForceAtPosition(force, pos, ForceMode.Force);
    }

}
