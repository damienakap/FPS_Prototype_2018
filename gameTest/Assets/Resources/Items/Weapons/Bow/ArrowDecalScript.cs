using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDecalScript : BulletDecalScript
{

    public GameObject arrowModel;

    private void Awake()
    {
        this.lifeTime = 5f;
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

    public override void resetDecal()
    {
        this.baseScale.Set(0.01f, 0.01f, 0.01f);
        base.resetDecal();
    }

    public override void setDecalRotation(Quaternion q)
    {
        base.setDecalRotation(q);
        arrowModel.transform.rotation = q;
    }

}
