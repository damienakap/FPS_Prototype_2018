using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidAIInput : HumanoidInput
{


    public int aiState = 0;
    public GameObject followTarget;
    public GameObject lookTarget;

    public Vector3 lookDir = new Vector3();

    // controller variables



    // key variables
    private bool spacePressed = false;
    private bool mouse0Pressed = false;
    private bool fire2Pressed = false;
    private bool leftShiftPressed = false;
    private bool leftCtrlPressed = false;
    private bool rPressed = false;
    private bool gPressed = false;
    private bool zPressed = false;
    private bool xPressed = false;
    private bool ePressed = false;
    private bool qPressed = false;

    private bool spaceJustPressed = false;
    private bool mouse0JustPressed = false;
    private bool fire2JustPressed = false;
    private bool leftShiftJustPressed = false;
    private bool leftCtrlJustPressed = false;
    private bool rJustPressed = false;
    private bool gJustPressed = false;
    private bool zJustPressed = false;
    private bool xJustPressed = false;
    private bool eJustPressed = false;
    private bool qJustPressed = false;

    private bool spacePressedLast = false;
    private bool mouse0PressedLast = false;
    private bool fire2PressedLast = false;
    private bool leftShiftPressedLast = false;
    private bool leftCtrlPressedLast = false;
    private bool rPressedLast = false;
    private bool gPressedLast = false;
    private bool zPressedLast = false;
    private bool xPressedLast = false;
    private bool ePressedLast = false;
    private bool qPressedLast = false;

    // internal variables

    private void Awake()
    {
        
    }

    private void FixedUpdate()
    {

        switch (aiState)
        {
            case 1:
                followState();
                break;
            default:
                followState();
                break;
        }

    }

    public void followState()
    {
        if (lookTarget != null) {
            lookDir = (lookTarget.transform.position - transform.position).normalized;
        }
        else
        {
            lookDir = transform.forward;
        }


        if (followTarget != null)
        {
            Vector3 dir = followTarget.transform.position - transform.position;
            float dist = dir.magnitude;

            if (lookTarget == null)
                lookDir = dir;
            dir.Normalize();

            dir = Quaternion.LookRotation( new Vector3(-lookDir.x, 0, lookDir.z) ) * dir;

            if (dist > 3)
            {
                inputMoveDir = dir;
                inputMoveSpeed = 1f;
            }
            else
            {
                inputMoveDir = new Vector3();
                inputMoveSpeed = 0f;
            }
                
            
        }

        Vector3 eulerLookRotation = Quaternion.LookRotation(lookDir).eulerAngles;
        inputLookYaw = eulerLookRotation.y;
        inputLookPitch = eulerLookRotation.x;

    }


    public override bool justPressedSpace() { return spaceJustPressed; }
    public override bool justPressedMouse0() { return mouse0JustPressed; }
    public override bool justPressedFire2() { return fire2JustPressed; }
    public override bool justPressedX() { return xJustPressed; }
    public override bool justPressedZ() { return zJustPressed; }
    public override bool justPressedR() { return rJustPressed; }
    public override bool justPressedG() { return gJustPressed; }
    public override bool justPressedE() { return eJustPressed; }
    public override bool justPressedQ() { return qJustPressed; }
    public override bool justPressedLeftShift() { return leftShiftJustPressed; }
    public override bool justPressedLeftCtrl() { return leftCtrlJustPressed; }

    public override bool pressedX() { return xPressed; }
    public override bool pressedMouse0() { return mouse0Pressed; }
    public override bool pressedLeftShift() { return leftShiftPressed; }
    public override bool pressedLeftCtrl() { return leftCtrlPressed; }

    public override Vector3 getInputMoveDir() { return inputMoveDir; }
    public override float getInputMoveSpeed() { return inputMoveSpeed; }
    public override float getLookYaw() { return inputLookYaw ; }
    public override float getLookPitch() { return inputLookPitch; }
    public override Ray getLookRay() { return lookRay;  }
    


}
