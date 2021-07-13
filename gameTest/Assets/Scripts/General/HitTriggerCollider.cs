using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitTriggerCollider : TriggerCollider
{

    protected List<Collider> hitObjects = new List<Collider>();
    protected List<Vector3> hitObjectPoints = new List<Vector3>();
    protected List<Collider> hitboxObjects = new List<Collider>();
    public MobBase parentEntity;
    protected HitBoxScript tempHitbox;
    protected Rigidbody tempRigidbody;


    private void OnTriggerEnter(Collider other)
    {
        tempHitbox = other.gameObject.GetComponent<HitBoxScript>();
        if (!hitboxObjects.Contains(other) && tempHitbox != null)
        {
            if(tempHitbox.getParentEntity() != this.parentEntity)
            {
                hitboxObjects.Add(other);
            }
            
        }

        tempRigidbody = other.gameObject.GetComponent<Rigidbody>();
        if (!hitObjects.Contains(other) && tempRigidbody != null)
        {
            hitObjects.Add(other);
            hitObjectPoints.Add(other.ClosestPointOnBounds(transform.position));
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (hitObjects.Contains(other))
        {
            hitObjectPoints.RemoveAt(hitObjects.IndexOf(other));
            hitObjects.Remove(other);
        }
        if (hitboxObjects.Contains(other))
        {
            hitboxObjects.Remove(other);
        }

    }

    public Collider[] getHitboxObjects() { return hitboxObjects.ToArray(); }
    public Collider[] getHitObjects() { return hitObjects.ToArray(); }
    public Vector3[] getHitObjectPoints() { return hitObjectPoints.ToArray(); }

}
