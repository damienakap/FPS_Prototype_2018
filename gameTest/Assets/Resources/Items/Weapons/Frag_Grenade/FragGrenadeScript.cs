using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragGrenadeScript : MonoBehaviour
{

    protected float damage = 50;
    public HitTriggerCollider hitBox;
    public float timer = 1;
    public bool timerActive = false;

    // Start is called before the first frame update
    void Start()
    {
        resetGrenade();
    }

    public void resetGrenade()
    {
        this.timerActive = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        this.timerActive = true;
    }

    private void FixedUpdate()
    {
        if (timerActive)
        {
            timer -= Time.deltaTime;
        }
        
        if (timer <= 0)
        {
            timer = 0;

            GameObject go;
            if ( hitBox.triggered ) 
            {
                
                HitBoxScript entity;
                Rigidbody rigid;
                Vector3 force;
                Collider[] hitObjects = hitBox.getHitObjects();
                
                for ( int i=0; i< hitObjects.Length; i++)
                {
                    go = hitObjects[i].gameObject;
                    entity = hitObjects[i].GetComponent<HitBoxScript>();

                    if ( entity != null)
                    {
                        entity.applyDamage(damage, 0);
                    }
                    rigid = hitObjects[i].attachedRigidbody;
                    if (rigid)
                    {
                        force = rigid.transform.position - transform.position;
                        force.Normalize();
                        force *= 300;
                        rigid.AddForceAtPosition( force, hitObjects[i].ClosestPoint(transform.position),ForceMode.Force );
                    }
                }
                

            }
            go = GameManager.loadPrefab("ExplosionSmoke","Items/Weapons/Frag_Grenade/");
            go.transform.position = transform.position;
            go.SetActive(true);
            Destroy(gameObject);

        }
    }
}
