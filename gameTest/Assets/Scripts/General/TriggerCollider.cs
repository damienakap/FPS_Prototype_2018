using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCollider : MonoBehaviour
{
    public bool triggered = false;
    private bool onTrigger = false;

    protected virtual void OnTriggerStay(Collider other)
    {
        triggered = true;
        onTrigger = true;
    }

    public virtual void resetTrigger()
    {
        triggered = false;
        onTrigger = false;
    }

    protected virtual void FixedUpdate()
    {
        if (!onTrigger)
        {
            triggered = false;
        }
        onTrigger = false;
    }

}
