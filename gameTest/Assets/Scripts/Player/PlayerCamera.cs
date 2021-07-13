using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Rendering.PostProcessing;

public class PlayerCamera : MonoBehaviour
{
    
    public RectTransform hudContainerRect;

    private int playerNumber = 1;
    public Transform target;
    public Vector3 targetPosLast = new Vector3();
    public static PostProcessVolume globalPostProcessVolume;
    private string lookHorizontalString = "Mouse X";
    private string lookVerticalString = "Mouse Y";

    public Transform lookPositionTransform;
    public Vector3 lookPoint = new Vector3();

    private float baseFOV = 90f;
    private float currentFOV = 90f;
    private float lastFOV = 90f;

    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float distanceMin = .5f;
    public float distanceMax = 15f;

    protected Vector2 offset3P = new Vector2(0.3f, 0.5f); // x-offset (right/left)  and  y-offset (up/down)
    protected Vector2 offset1P = new Vector2(0.3f, 0.5f);   // z-offset (forwards/backwards) and y-offset (up/down)

    private float mouseSensitivity = 2f;
    private float zoomMouseSensitivity = 1f;

    private float aimAssistSlowValue = 0.5f;
    private float aimAssistSlowAngle = 2f;
    private float currentAimAssistSlowValue = 0f;

    private float magnetizeStrength = 100f;
    private float magnetizeAngle = 15f;
    private float magnetizeCurvePower = 1f;


    private float magnetizeTargetSwitchTime = 0.5f;
    private float currentMagnetizeTargetSwitchTime = 0f;
    private float radialAngleDeltaLast = 0f;

    private Transform lastHitObject = null;
    private Transform lastHitPart = null;
    private MobBase lastHitMob = null;

    private Transform newHitObject = null;
    private Transform newHitPart = null;
    private MobBase newHitMob = null;


    public Camera myCamera;

    public bool lockCursor = true;
    public bool firstPerson = false;

    private float x = 0.0f;
    private float y = 0.0f;

    private float distanceOffset = 0f;
    private float distanceOffsetSmooth = 0f;

    private Vector3 currentPosition = new Vector3();
    private Vector3 relativePos = new Vector3();
    private Vector3 negDistance = new Vector3();

    private bool usingJoycon = false;

    private void OnValidate()
    {
        setLockCursor(lockCursor);
        myCamera.fieldOfView = baseFOV;
    }

    void setLockCursor( bool s )
    {
        lockCursor = s;
        if (s)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;
        
        // Hide cursor when locking
        Cursor.visible = !s;
    }

    public void setPlayerNumber(int n)
    {
        playerNumber = n;

        hudContainerRect.anchorMax = GameManager.getPlayerScreenRectAnchor(playerNumber);
        hudContainerRect.anchorMin = hudContainerRect.anchorMax;
        hudContainerRect.pivot = new Vector2(0.5f, 0.5f);

        hudContainerRect.localScale = GameManager.getPlayerScreenRectScale(playerNumber); ;

        Rect r = new Rect();
        r.min = GameManager.getCameraViewMin(playerNumber);
        r.max = GameManager.getCameraViewMax(playerNumber);
        myCamera.rect = r;
    }

    public void setJoyconNumber(int n)
    {
        lookHorizontalString = "LookHorizontalJ" + n;
        lookVerticalString = "LookVerticalJ" + n;
        usingJoycon = (n>0);
    }

    // Use this for initialization
    void Start()
    {

        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
        
        setLockCursor(lockCursor);
    }

    private void FixedUpdate()
    {

        checkIfTargetIsVisible();

        if (usingJoycon)
        {
            getHitEntity();
            applyAimAssist();
        }

        RaycastHit hit;
        //LayerMask mask = (1 << 8) | GameManager.getPlayerLayerMask(playerNumber);
        LayerMask mask =
            (1 << 0)
            | (1 << 17)
            | (1 << 22)
            | (1 << 20)
            | GameManager.getPlayerLayerMask(playerNumber);

        if (Physics.Raycast(transform.position, transform.forward, out hit, 1000, mask))
        {
            lookPoint = hit.point;
        }
        else
        {
            lookPoint = transform.position + transform.forward * 1000f;
        }

        lookPositionTransform.position = lookPoint;

    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            setLockCursor(!lockCursor);

        float gain = 0.1f;
        myCamera.fieldOfView = currentFOV * gain + myCamera.fieldOfView * (1f - gain);


        if (!globalPostProcessVolume)
            globalPostProcessVolume = GameManager.globalPostProcessVolume;
        Vignette vignette = null;
        globalPostProcessVolume.profile.TryGetSettings(out vignette);

        if (vignette)
        {
            vignette.intensity.value = 0.2f + 0.2f*Mathf.Abs(baseFOV - myCamera.fieldOfView) / baseFOV;
            vignette.smoothness.value = 1f + Mathf.Abs(baseFOV - myCamera.fieldOfView) / baseFOV;
        }


        currentMagnetizeTargetSwitchTime -= Time.deltaTime;
        if (lastHitObject)
            if (lastHitMob.mobData.health == 0 || currentMagnetizeTargetSwitchTime <= 0)
            {
                lastHitMob = null;
                lastHitObject = null;
            }
            
        //shadowsOnlyRender = false;

        if (target && lockCursor)
        {

            float slowValue = 1f;
            if(usingJoycon)
                slowValue = 1f - currentAimAssistSlowValue;

            float sensitivity = mouseSensitivity;
            if (currentFOV < baseFOV)
                sensitivity = zoomMouseSensitivity;


            x += Input.GetAxis(lookHorizontalString) * xSpeed * slowValue * sensitivity * Time.deltaTime;
            y -= Input.GetAxis(lookVerticalString) * ySpeed * slowValue * sensitivity * Time.deltaTime;

            y = ClampAngle(y, yMinLimit, yMaxLimit);
            x = ClampAngle(x, -360, 360);

            applyTransforms();

            targetPosLast = target.position;
        }
    }

    void getHitEntity()
    {

        RaycastHit hit;
        //LayerMask mask = (1 << 8) | GameManager.getPlayerLayerMask(playerNumber);
        LayerMask mask =
            (1 << 20)
            | GameManager.getPlayerLayerMask(playerNumber);

        if (Physics.Raycast(currentPosition, transform.forward, out hit, 30, mask))
        {
            // check if hit object is valid entity
            Transform hitObject = hit.transform;
            MobBase hitMob = hitObject.gameObject.GetComponent<HitBoxScript>().getParentEntity();
            if (hitMob)
            {
                if (hitMob.mobData.health == 0)
                {
                    hitObject = null;
                    hitMob = null;
                }
            }
            else
            {
                hitObject = null;
            }

            // set hit object as a new target
            if (hitObject)
            {

                if (!lastHitObject)
                {
                    lastHitPart = hitObject;
                    lastHitObject = hitMob.transform;
                    lastHitMob = hitMob;
                    currentMagnetizeTargetSwitchTime = magnetizeTargetSwitchTime;
                    return;
                }

                if(lastHitObject == hitObject)
                {
                    currentMagnetizeTargetSwitchTime = magnetizeTargetSwitchTime;
                    return;
                }
            }

        }

    }

    void applyAimAssist()
    {

        if (lastHitObject)
        {
            Vector3 targDir = lastHitPart.position - currentPosition;
            Vector3 camDir = transform.forward;
            targDir.Normalize();
            camDir.Normalize();

            // get angle delta between aim and target
            radialAngleDeltaLast = Mathf.Acos(Vector3.Dot(targDir,camDir))*Mathf.Rad2Deg;
            //Debug.Log(radialAngleDeltaLast);
            if (radialAngleDeltaLast > magnetizeAngle)
            {
                radialAngleDeltaLast = 360;
                lastHitObject = null;
                lastHitMob = null;
                return;
            }

            // apply magnetize
            if((target.position - targetPosLast).magnitude > 0.01f * Time.deltaTime){
                targDir = lastHitObject.position - currentPosition;
                Vector3 v = GameManager.slerpDirections(camDir, targDir, 1f);
                Vector3 e = Quaternion.LookRotation(v).eulerAngles;
                Vector3 ec = Quaternion.LookRotation(camDir).eulerAngles;

                v = new Vector3(e.x - ec.x, e.y - ec.y);
                v.Normalize();
                v *= magnetizeStrength * Time.deltaTime;
                v *= Mathf.Pow(radialAngleDeltaLast / magnetizeAngle,magnetizeCurvePower);

                Vector2 mag = new Vector2( v.x, v.y);

                //y += mag.x;
                x += mag.y;
            }

            // apply aim slow
            currentAimAssistSlowValue = 0f;
            if (radialAngleDeltaLast <= aimAssistSlowAngle)
                currentAimAssistSlowValue = aimAssistSlowValue * Mathf.Pow(1f - radialAngleDeltaLast / aimAssistSlowAngle, 2f);
            

        }

        
    }

    void checkIfTargetIsVisible()
    {
        RaycastHit hit;
        LayerMask mask =
            ~(
                (1 << 20)
                | (1 << 21)
                | (1 << 23)
                | (1 << 8)
                | (1 << GameManager.get1PLayer(playerNumber))
                | (1 << GameManager.get3PLayer(playerNumber))
            );

        distanceOffset = 0;

        if (Physics.Raycast(currentPosition, relativePos, out hit, distance, mask))
        {
            //Debug.Log(hit.collider.gameObject.layer);
            //Debug.DrawLine(target.position, hit.point);
            distanceOffset = distance - hit.distance + 0.5f;
            distanceOffset = Mathf.Clamp(distanceOffset, 0, distance);
        }

        float gain = 0.1f;
        negDistance = new Vector3();

        if (!firstPerson)
        {
            distanceOffsetSmooth = distanceOffset * gain + distanceOffsetSmooth * (1 - gain);
            negDistance = new Vector3(0.0f, 0.0f, -distance + distanceOffsetSmooth);
        }
    }

    void applyTransforms()
    {
        Quaternion rotation = Quaternion.Euler(y, x, 0);

        distance = Mathf.Clamp(distance, distanceMin, distanceMax);

        float theta = transform.rotation.eulerAngles.y * Mathf.Deg2Rad;

        // set camera position in the world
        if (firstPerson)
            currentPosition = new Vector3(offset1P.x * Mathf.Sin(theta), offset1P.y, offset1P.x * Mathf.Cos(theta));
        else
            currentPosition = new Vector3(offset3P.x * Mathf.Cos(theta), offset3P.y, -offset3P.x * Mathf.Sin(theta));

        currentPosition += target.position;


        if (usingJoycon)
            applyAimAssist();

        relativePos = (transform.position) - (currentPosition);

        Vector3 position = rotation * negDistance + currentPosition;
        transform.rotation = rotation;
        if(position != null)
            transform.position = position;
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        while (angle < -360F)
            angle += 360F;
        while (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

    public static float clampAngleTo180(float angle)
    {
        while (angle < -360F)
            angle += 360F;
        while (angle > 360F)
            angle -= 360F;
        if (angle > 180) angle -= 360;
        if (angle < -180) angle += 360;
        return angle;
    }



    public void setFirstPerson(bool b)
    {
        this.firstPerson = b;

        if (firstPerson)
        {
            int mask = 1 << GameManager.get3PLayer(playerNumber);
            for (int i = 1; i <= 4; i++)
            {
                if (i != playerNumber)
                    mask |= 1 << GameManager.get1PLayer(i);
            }
            myCamera.cullingMask = ~mask;
        }
        else
        {
            int mask = 0;
            for (int i = 1; i <= 4; i++)
            {
                mask |= 1 << GameManager.get1PLayer(i);
            }
            myCamera.cullingMask = ~mask;
        }
    }

    public void setFOV( float f )
    {
        this.currentFOV = f;
    }
    public float getBaseFOV() { return baseFOV; }

    public Vector3 getLookPoint() { return lookPoint; }

}