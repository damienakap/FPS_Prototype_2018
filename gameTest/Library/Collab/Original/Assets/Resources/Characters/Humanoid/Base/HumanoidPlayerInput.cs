using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidPlayerInput : HumanoidInput
{

    // controller variables
    public Transform cameraTarget;

    private float camInputTheta = 0f;
    private Ray cameraRay = new Ray();


    // key variables
    private bool spacePressed = false;
    private bool mouse0Pressed = false;
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

        inputDir.Set(0f, 0f, 0f);
        if (Input.GetKey(KeyCode.S))
        {
            inputDir.z += -1f;
        }
        if (Input.GetKey(KeyCode.W))
        {
            inputDir.z += 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputDir.x += -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputDir.x += 1f;
        }

        if( inputDir.magnitude > 0 ) inputDir.Normalize();

        // handle On Press events
        mouse0PressedLast = mouse0Pressed;
        mouse0Pressed = false;
        mouse0JustPressed = false;
        if (Input.GetKey(KeyCode.Mouse0))
        {
            mouse0Pressed = true;
            if (!mouse0PressedLast) mouse0JustPressed = true;
        }

        rPressedLast = rPressed;
        rPressed = false;
        rJustPressed = false;
        if (Input.GetKey(KeyCode.R))
        {
            rPressed = true;
            if (!rPressedLast) rJustPressed = true;
        }

        gPressedLast = gPressed;
        gPressed = false;
        gJustPressed = false;
        if (Input.GetKey(KeyCode.G))
        {
            gPressed = true;
            if (!gPressedLast) gJustPressed = true;
        }

        zPressedLast = zPressed;
        zPressed = false;
        zJustPressed = false;
        if (Input.GetKey(KeyCode.Z))
        {
            zPressed = true;
            if (!zPressedLast) zJustPressed = true;
        }

        xPressedLast = xPressed;
        xPressed = false;
        xJustPressed = false;
        if (Input.GetKey(KeyCode.X))
        {
            xPressed = true;
            if (!xPressedLast) xJustPressed = true;
        }

        ePressedLast = ePressed;
        ePressed = false;
        eJustPressed = false;
        if (Input.GetKey(KeyCode.E))
        {
            ePressed = true;
            if (!ePressedLast) eJustPressed = true;
        }

        qPressedLast = qPressed;
        qPressed = false;
        qJustPressed = false;
        if (Input.GetKey(KeyCode.Q))
        {
            qPressed = true;
            if (!qPressedLast) qJustPressed = true;
        }


        // jump
        spacePressedLast = spacePressed;
        spacePressed = false;
        spaceJustPressed = false;
        if (Input.GetKey(KeyCode.Space))
        {
            spacePressed = true;
            if (!spacePressedLast) spaceJustPressed = true;

        }

        leftShiftPressedLast = leftShiftPressed;
        leftShiftPressed = false;
        leftShiftJustPressed = false;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            leftShiftPressed = true;
            if (!leftShiftPressedLast) leftShiftJustPressed = true;

        }

        leftCtrlPressedLast = leftCtrlPressed;
        leftCtrlPressed = false;
        leftCtrlJustPressed = false;
        if (Input.GetKey(KeyCode.LeftControl))
        {
            leftCtrlPressed = true;
            if (!leftCtrlPressedLast) leftCtrlJustPressed = true;

        }



        // Handle Model and camera relative motions
        if (cameraTarget)
        {
            camInputTheta = inputTheta + cameraTarget.rotation.eulerAngles.y * Mathf.Deg2Rad;

            Camera camera = cameraTarget.gameObject.GetComponent<Camera>();
            cameraRay = camera.ScreenPointToRay(new Vector2( camera.pixelWidth / 2, camera.pixelHeight / 2 )) ;

        }

    }


    public override bool justPressedSpace() { return spaceJustPressed; }
    public override bool justPressedMouse0() { return mouse0JustPressed; }
    public override bool justPressedX() { return xJustPressed; }
    public override bool justPressedZ() { return zJustPressed; }
    public override bool justPressedR() { return rJustPressed; }
    public override bool justPressedG() { return gJustPressed; }
    public override bool justPressedE() { return eJustPressed; }
    public override bool justPressedQ() { return qJustPressed; }
    public override bool justPressedLeftShift() { return leftShiftJustPressed; }
    public override bool justPressedLeftCtrl() { return leftCtrlJustPressed; }

    public override bool pressedMouse0() { return mouse0Pressed; }
    public override bool pressedLeftShift() { return leftShiftPressed; }
    public override bool pressedLeftCtrl() { return leftCtrlPressed; }

    public override Vector3 getInputDir() { return inputDir; }
    public override float getCameraInputTheta() { return camInputTheta; }
    public override float getLookTheta() { return cameraTarget.rotation.eulerAngles.y; }
    public override float getLookPhi() { return cameraTarget.rotation.eulerAngles.x; }
    public override Ray getLookRay() { return cameraRay;  }
    


}
