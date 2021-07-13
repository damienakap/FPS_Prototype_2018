using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNode : MonoBehaviour
{

    [SerializeField] protected float spawnRadius = 1f;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,0.5f);
    }

}
