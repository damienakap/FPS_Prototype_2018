using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidInput : MonoBehaviour
{

    protected Vector3 inputDir = new Vector3();
    protected float inputTheta = 0f;

    public virtual bool justPressedSpace() { return false; }
    public virtual bool justPressedMouse0() { return false; }
    public virtual bool justPressedX() { return false; }
    public virtual bool justPressedZ() { return false; }
    public virtual bool justPressedR() { return false; }
    public virtual bool justPressedG() { return false; }
    public virtual bool justPressedE() { return false; }
    public virtual bool justPressedQ() { return false; }
    public virtual bool justPressedLeftShift() { return false; }
    public virtual bool justPressedLeftCtrl() { return false; }

    public virtual bool pressedMouse0() { return false; }
    public virtual bool pressedLeftShift() { return false; }
    public virtual bool pressedLeftCtrl() { return false; }

    public virtual Vector3 getInputDir() { return new Vector3(); }
    public virtual float getCameraInputTheta() { return 0f; }
    public virtual float getLookTheta() { return 0f; }
    public virtual float getLookPhi() { return 0f; }
    public virtual Ray getLookRay() { return new Ray(); }

}
