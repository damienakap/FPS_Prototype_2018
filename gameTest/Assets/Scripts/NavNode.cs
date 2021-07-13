using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavNode : MonoBehaviour
{
    
    [SerializeField] protected NavNode[] connectedNavNodes;

    private void OnDrawGizmos()
    {
        foreach (NavNode n in connectedNavNodes)
            Gizmos.DrawLine(transform.position, n.transform.position);
    }

    public NavNode[] getConnectedNavNodes() { return connectedNavNodes;  }

}
