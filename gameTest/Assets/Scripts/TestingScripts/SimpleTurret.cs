using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTurret : MonoBehaviour
{

    public float fireRate = 5;
    private float fireDelay;
    private float fireTimer = 0;

    private void OnValidate()
    {
        setFireRate(fireRate);
    }

    // Start is called before the first frame update
    void Start()
    {
        setFireRate(fireRate);
    }

    void setFireRate(float f)
    {
        fireRate = f;
        fireDelay = 1 / f;
    }

    void spawnBullet()
    {
        GameObject bullet = GameManager.loadPrefab("Pistol_Test_Bullet", "Items/Weapons/Pistol_Test/");
        BulletData bd = bullet.GetComponent<BulletData>();

        bullet.transform.position = transform.position;
        bullet.transform.rotation = transform.rotation;

        bd.setDamage(10);
        bd.setImpactForce(500);
        bd.setDirection(-transform.forward);

        bullet.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0)
        {
            fireTimer = fireDelay;
            spawnBullet();
        }
    }
}
