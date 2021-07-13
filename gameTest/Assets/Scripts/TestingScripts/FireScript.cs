using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireScript : MonoBehaviour
{

    float damage = 10f;

    private void OnTriggerStay(Collider other)
    {
        HitBoxScript hitbox = other.GetComponent<HitBoxScript>();
        if (hitbox)
            hitbox.applyDamage( damage*Time.fixedDeltaTime,0 );
    }

}
