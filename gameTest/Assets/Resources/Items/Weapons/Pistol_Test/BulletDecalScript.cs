using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDecalScript : MonoBehaviour
{


    public float lifeTime = 0f;
    protected Quaternion decalRotation;
    protected Vector3 baseScale = new Vector3(0.005f,0.005f,0.005f);

    private void Awake()
    {
        this.resetDecal();
    }

    private void FixedUpdate()
    {
        if (this.lifeTime <=0)
        {
            this.gameObject.SetActive(false);
            this.enabled = false;
            Destroy(this.gameObject,1f);
        }
        else
        {
            this.lifeTime -= Time.deltaTime;
        }
    }

    public virtual void resetDecal()
    {
        this.lifeTime = 5f;
        transform.localScale = this.baseScale;
    }
    public virtual void setDecalRotation(Quaternion q) { this.decalRotation = q; }

}
