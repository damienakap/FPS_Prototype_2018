using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelHelper : MonoBehaviour
{
    [Header("First Person Models")]
    [SerializeField] private SkinnedMeshRenderer[] firstPersonModels;

    [Header("Third Person Models")]
    [SerializeField] private SkinnedMeshRenderer[] thirdPersonModels;

    [Header("First Person Arm Models")]
    [SerializeField] private SkinnedMeshRenderer[] firstPersonArmModels;

    public void setFirstPersonModelLayers( int layer)
    {
        foreach(SkinnedMeshRenderer smr in firstPersonModels)
        {
            smr.gameObject.layer = layer;
        }

        foreach (SkinnedMeshRenderer smr in firstPersonArmModels)
        {
            smr.gameObject.layer = layer;
        }

    }

    public void setThirdPersonModelLayers(int layer)
    {
        foreach (SkinnedMeshRenderer smr in thirdPersonModels)
        {
            smr.gameObject.layer = layer;
        }
    }

}
