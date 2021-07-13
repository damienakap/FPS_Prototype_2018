using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidInput : MonoBehaviour
{

    protected Vector3 inputMoveDir = new Vector3();
    protected Vector3 lookPoint = new Vector3();
    protected float inputLookYaw = 0f;
    protected float inputLookPitch = 0f;
    protected Ray lookRay = new Ray();
    protected float inputMoveSpeed = 0f;

    public virtual bool justPressedSpace() { return false; }
    public virtual bool justPressedMouse0() { return false; }
    public virtual bool justPressedFire2() { return false; }
    public virtual bool justPressedX() { return false; }
    public virtual bool justPressedZ() { return false; }
    public virtual bool justPressedR() { return false; }
    public virtual bool justPressedG() { return false; }
    public virtual bool justPressedE() { return false; }
    public virtual bool justPressedQ() { return false; }
    public virtual bool justPressedF() { return false; }
    public virtual bool justPressedLeftShift() { return false; }
    public virtual bool justPressedLeftCtrl() { return false; }

    public virtual bool pressedX() { return false; }
    public virtual bool pressedMouse0() { return false; }
    public virtual bool pressedFire2() { return false; }
    public virtual bool pressedLeftShift() { return false; }
    public virtual bool pressedLeftCtrl() { return false; }

    public virtual Vector3 getInputMoveDir() { return new Vector3(); }
    public virtual float getInputMoveSpeed() { return 0f; }
    public virtual float getLookYaw() { return 0f; }
    public virtual float getLookPitch() { return 0f; }
    public virtual Ray getLookRay() { return new Ray(); }
    public virtual Vector3 getLookPoint() { return lookPoint; }

}
