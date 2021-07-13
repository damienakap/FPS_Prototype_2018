using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

public class LoadTestScene : MonoBehaviour
{

    public void loadTestScene()
    {

        if (GameManager.playerJoined(1))
        {
            GameManager.loadScene(2);
            GameManager.unloadScene(1);
            GameManager.loadPlayerInstances();

            HumanoidBaseScript pi;
            GameObject w;

            for(int i=1; i<5; i++)
            {
                pi = GameManager.getPlayerInstance(i);

                if (pi)
                {

                    pi.transform.position = new Vector3( (2 * Random.value - 1)*3 , 2, -15 + (2*Random.value-1)*3);
                    pi.gameObject.SetActive(true);

                    w = ItemHelper.loadWeaponPrefab(WeaponIndex.TestSword);
                    w.SetActive(true);
                    w.GetComponent<WeaponData>().initialize();
                    pi.equipWeaponToSlot(w, 1);

                    w = ItemHelper.loadWeaponPrefab(WeaponIndex.BurstRifle);
                    w.SetActive(true);
                    w.GetComponent<WeaponData>().initialize();
                    pi.equipWeaponToSlot(w, 0);

                }

            }

            

        }

    }

}
