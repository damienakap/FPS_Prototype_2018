using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HumanoidPlayerInput : HumanoidInput
{

    // controller variables
    public Camera camera;
    private PlayerCamera cameraScript;
    public int joyconNumber = 0;
    public int playerNumber = 1;

    private float holdDuration = 0.2f;
    private float holdInteractTime = 0f;

    private bool interact = false;

    private string moveHorizontalString;
    private string moveVerticalString;
    private string jumpString;
    private string dodgeString;
    private string crouchString;
    private string meleeString;
    private string fire1String;
    private string fire2String;
    private string swapWeaponString;
    private string throwGrenadeString;
    private string reloadString;

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
    private bool fPressed = false;

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
    private bool fJustPressed = false;

    private bool rJustReleased = false;

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
    private bool fPressedLast = false;

    public MobBase playerEntity;
    public RectTransform healthBar;
    public RectTransform shieldBar;

    // internal variables

    private void OnValidate()
    {
        setup(playerNumber, joyconNumber);
    }

    private void Start()
    {
        setup(playerNumber, joyconNumber);
    }

    public void setup( int playerNum, int joyconNum )
    {
        playerNumber = playerNum;
        joyconNumber = joyconNum;
        cameraScript = camera.GetComponent<PlayerCamera>();
        cameraScript.setPlayerNumber(playerNumber);
        setJoyconNumber(joyconNumber);
    }

    public void setJoyconNumber( int n)
    {
        if (n < 0 || n >4) n = 0;
        joyconNumber = n;

        moveHorizontalString = "MoveHorizontalJ" + n;
        moveVerticalString = "MoveVerticalJ" + n;
        jumpString = "JumpJ" + n;
        dodgeString = "DodgeJ" + n;
        crouchString = "CrouchJ" + n;
        meleeString = "MeleeJ" + n;
        fire1String = "Fire1J" + n;
        fire2String = "Fire2J" + n;
        swapWeaponString = "SwapWeaponJ" + n;
        throwGrenadeString = "ThrowGrenadeJ" + n;
        reloadString = "ReloadJ" + n;

        cameraScript.setJoyconNumber(n);
    }

    private void Update()
    {
        updateInput();

        healthBar.localScale = new Vector3( playerEntity.getScaledHealth(), 1, 1 );
        shieldBar.localScale = new Vector3( playerEntity.getScaledShields(), 1, 1 );


    }

    void updateInput()
    {

        if (Input.GetKeyDown(KeyCode.J))
        {
            if (joyconNumber > 0)
                joyconNumber = 0;
            else
                joyconNumber = 1;
            setJoyconNumber(joyconNumber);
        }

        inputMoveDir.Set(Input.GetAxis(moveHorizontalString), 0f, Input.GetAxis(moveVerticalString));

        inputMoveSpeed = inputMoveDir.magnitude;

        lookPoint = cameraScript.getLookPoint();

        // handle On Press events
        mouse0PressedLast = mouse0Pressed;
        mouse0Pressed = Input.GetButton(fire1String);
        mouse0JustPressed = false;
        if (mouse0Pressed)
        {
            if (!mouse0PressedLast) mouse0JustPressed = true;
        }

        fire2PressedLast = fire2Pressed;
        fire2Pressed = Input.GetButton(fire2String);
        fire2JustPressed = false;
        if (fire2Pressed)
        {
            if (!fire2PressedLast) fire2JustPressed = true;
        }

        rPressedLast = rPressed;
        rJustPressed = false;
        rPressed = Input.GetButton(reloadString);


        ePressedLast = ePressed;
        ePressed = false;
        eJustPressed = false;

        if (joyconNumber == 0)
        {
            if (rPressed)
            {
                if (!rPressedLast) rJustPressed = true;
            }

            ePressed = Input.GetButton("Interact");
            if (ePressed)
            {
                if (!ePressedLast) eJustPressed = true;
            }
        }
        else
        {
            rJustReleased = false;
            if (rPressed)
            {
                holdInteractTime += Time.deltaTime;
            }
            else if (rPressedLast)
            {
                interact = false;
                rJustPressed = true;
                holdInteractTime = 0f;
            }

            if (holdInteractTime >= holdDuration && !interact)
            {
                holdInteractTime = holdDuration;
                eJustPressed = true;
                interact = true;
            }
        }



        gPressedLast = gPressed;
        gPressed = Input.GetButton(throwGrenadeString);
        gJustPressed = false;
        if (gPressed)
        {
            if (!gPressedLast) gJustPressed = true;
        }

        zPressedLast = zPressed;
        zPressed = Input.GetButton("ToggleWalk");
        zJustPressed = false;
        if (zPressed)
        {
            if (!zPressedLast) zJustPressed = true;
        }

        xPressedLast = xPressed;
        xPressed = Input.GetKey(KeyCode.X);
        xJustPressed = false;
        if (xPressed)
        {
            if (!xPressedLast) xJustPressed = true;
        }

        qPressedLast = qPressed;
        qPressed = Input.GetButton(swapWeaponString);
        qJustPressed = false;
        if (qPressed)
        {
            if (!qPressedLast) qJustPressed = true;
        }

        fPressedLast = fPressed;
        fPressed = Input.GetButton(meleeString);
        fJustPressed = false;
        if (fPressed)
        {
            if (!fPressedLast) fJustPressed = true;
        }


        // jump
        spacePressedLast = spacePressed;
        spacePressed = Input.GetButton(jumpString);
        spaceJustPressed = false;
        if (spacePressed)
        {
            if (!spacePressedLast) spaceJustPressed = true;
        }

        leftShiftPressedLast = leftShiftPressed;
        leftShiftPressed = Input.GetButton(dodgeString);
        leftShiftJustPressed = false;
        if (leftShiftPressed)
        {
            if (!leftShiftPressedLast) leftShiftJustPressed = true;
        }

        leftCtrlPressedLast = leftCtrlPressed;
        leftCtrlPressed = Input.GetButton(crouchString);
        leftCtrlJustPressed = false;
        if (leftCtrlPressed)
        {
            if (!leftCtrlPressedLast) leftCtrlJustPressed = true;

        }



        // Handle Model and camera relative motions
        if (camera != null)
        {
            inputLookYaw = camera.transform.rotation.eulerAngles.y;
            inputLookPitch = camera.transform.rotation.eulerAngles.x;

            lookRay = camera.ScreenPointToRay(new Vector2(camera.pixelWidth / 2, camera.pixelHeight / 2));

        }
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
    public override bool justPressedF() { return fJustPressed; }
    public override bool justPressedLeftShift() { return leftShiftJustPressed; }
    public override bool justPressedLeftCtrl() { return leftCtrlJustPressed; }

    public override bool pressedX() { return xPressed; }
    public override bool pressedMouse0() { return mouse0Pressed; }
    public override bool pressedFire2() { return fire2Pressed; }
    public override bool pressedLeftShift() { return leftShiftPressed; }
    public override bool pressedLeftCtrl() { return leftCtrlPressed; }

    public override Vector3 getInputMoveDir() { return inputMoveDir; }
    public override float getInputMoveSpeed() { return inputMoveSpeed; }
    public override float getLookYaw() { return inputLookYaw ; }
    public override float getLookPitch() { return inputLookPitch; }
    public override Ray getLookRay() { return lookRay;  }
    


}
