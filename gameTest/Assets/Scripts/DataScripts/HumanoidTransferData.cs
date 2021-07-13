using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidTransferData : MonoBehaviour
{
    
    private WeaponTransferData[] weaponTransferDatas;
    private MobData mobData;

    public HumanoidTransferData(WeaponTransferData[] wtds, MobData md)
    {
        weaponTransferDatas = wtds;
        mobData = md;
    }

    public WeaponTransferData[] getWeaponTransferDatas() { return weaponTransferDatas; }
    public MobData getMobData() { return mobData; }

}
