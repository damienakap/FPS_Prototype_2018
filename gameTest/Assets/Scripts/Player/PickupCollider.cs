using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupCollider : MonoBehaviour
{
    public bool triggered = false;
    private bool onTrigger = false;
    List<GameObject> hitObjects = new List<GameObject>();

    private void OnTriggerStay(Collider other)
    {
        if (!hitObjects.Contains(other.gameObject))
        {
            hitObjects.Add(other.gameObject);
        }
        triggered = true;
        onTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (hitObjects.Contains(other.gameObject))
        {
            hitObjects.Remove(other.gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (!onTrigger) triggered = false;
        onTrigger = false;
        //hitObjects.Clear();
    }

    public GameObject[] getHitObjects() { return hitObjects.ToArray(); }
    public void removeObject( GameObject go ) { hitObjects.Remove(go); }

}
