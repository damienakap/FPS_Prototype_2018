using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponIndex : uint
{
    TestAssaultRifle = 0,
    BurstRifle = 1,
    TestPistol = 2,
    Bow = 3,
    TestSword = 4
};

public class ItemHelper : MonoBehaviour
{

    private static string[] WeaponPrefabNames =
        new string[] 
        {
            "AssaultRife_Test_Prefab",
            "BurstRifle_Prefab",
            "Pistol_Test_Prefab",
            "Bow_Prefab",
            "Sword_Test_Prefab"
        };

    private static string[] WeaponPrefabDirectories =
        new string[]
        {
            "Items/Weapons/AssaultRifle_Test/",
            "Items/Weapons/BurstRifle/",
            "Items/Weapons/Pistol_Test/",
            "Items/Weapons/Bow/",
            "Items/Weapons/Sword_Test/"
        };

    public static string getWeaponPrefabName( WeaponIndex wi ) { return WeaponPrefabNames[(int)wi]; }
    public static string getWeaponPrefabDirectory( WeaponIndex wi ) { return WeaponPrefabDirectories[(int)wi]; }

    public static GameObject loadWeaponPrefab(WeaponIndex wi)
    {
        return GameManager.loadPrefab( WeaponPrefabNames[(int)wi], WeaponPrefabDirectories[(int)wi] );
    }

}
