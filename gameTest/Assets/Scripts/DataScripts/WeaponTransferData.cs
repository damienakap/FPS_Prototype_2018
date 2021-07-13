using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTransferData : MonoBehaviour
{

    private WeaponIndex weaponIndex;
    private int ammo;
    private int clipAmmo;

    public WeaponTransferData(){}

    /// <summary>
    /// Set/Create new weapon transfer data
    /// </summary>
    /// <param name="i">Weapon Index</param>
    /// <param name="a">Ammo</param>
    /// <param name="ca">Clip Ammo</param>
    public WeaponTransferData(WeaponIndex i, int a, int ca)
    {
        weaponIndex = i;
        ammo = a;
        clipAmmo = ca;
    }

    public void setWeaponIndex( WeaponIndex wi) { weaponIndex = wi;  }
    public void setAmmo(int i) { ammo = i; }
    public void setClipAmmo(int i) { clipAmmo = i; }

    public WeaponIndex getWeaponIndex() { return weaponIndex; }
    public int getAmmo() { return ammo; }
    public int getClipAmmo() { return clipAmmo; }

    public WeaponTransferData getCopy() { return new WeaponTransferData(weaponIndex, ammo, clipAmmo); }
}
