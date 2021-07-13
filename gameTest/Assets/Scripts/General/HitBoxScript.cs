using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxScript : MonoBehaviour
{

    [SerializeField] private float damageFactor = 1f;
    [SerializeField] private MobBase parentEntity;

    public void applyDamage( float damage, int type)
    {
        if (parentEntity == null) { return; }
        parentEntity.applyDamage(damage * damageFactor, type);

    }

    public MobBase getParentEntity() { return this.parentEntity; }

}
