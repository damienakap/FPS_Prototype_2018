using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HumanoidBaseScript : MonoBehaviour
{
    protected Rigidbody rigidBody;
    protected Collider[] ragdollColliders;
    protected CapsuleCollider bodyCollider;
    public PickupCollider pickupCollider;
    public TriggerCollider headTrigger;
    protected GameManager gameManager;

    protected const int weaponCount = 2;
    public GameObject[] equippedWeapons = new GameObject[weaponCount];
    protected WeaponData[] equippedWeaponScripts = new WeaponData[weaponCount];
    protected Animator[] equippedWeaponAnimators = new Animator[weaponCount];


    /*
     * 0: none
     * 1: primary
     * 2: secondary
     */
    protected int equippedWeaponIndex = 0;
    protected int equippedWeaponIndexLast = 0;

    public Transform modelTarget;
    public Animator animator;

    public Text dodgeCounter;

    public HumanoidInput playerInput;

    public float modelYOffset = -0.78f;
    public float modelCrouchYOffest = -0.5f;

    public float colliderHeight = 1.5f;
    public float colliderCrouchHeight = 1f;

    public float gravityModifier = 1f;
    public float airMovementGain = 0.02f;
    public float baseSpeed = 1.0f;
    public float crouchSpeed = 0.75f;
    public float runSpeed = 5.0f;
    public float jumpSpeed = 5.0f;
    public float dodgeSpeed = 12f;

    public int doubleJumps = 1;
    public int dodges = 1;
    public float dodgeCooldownTime = 0.25f;
    protected float currentDodgeCooldownTime = 0f;
    protected int currentDodgeCount = 0;
    protected int currentDoubleJumpCount = 0;

    public bool canJump = true;
    public bool lookInCameraDirection = true;
    public bool disableInput = false;

    protected float directionTheta = 0.0f;
    protected float moveSpeed = 0.0f;

    protected Vector3 velocity;
    protected Vector3 inputVel = new Vector3(0f, 0f, 0f);
    protected Vector3 inputVelLast = new Vector3(0f, 0f, 0f);
    protected Vector3 inputDir = new Vector3(0f, 0f, 0f);
    protected Vector3 inputDirLast = new Vector3(0f, 0f, 0f);

    protected float inputTheta = 0f;
    protected float inputThetaLast = 0f;
    protected float camInputTheta = 0f;
    protected float camInputThetaLast = 0f;
    protected float dodgeTheta = 0f;

    protected float inputSpeed = 0f;
    protected float inputSpeedLast = 0f;
    protected Vector3 rotation = new Vector3(0f, 0f, 0f);
    protected Vector3 groundNormal;

    public float lookTheta = 0f;
    public float lookPhi = 0f;

    protected bool grounded = false;
    protected bool landed = false;
    protected bool jumped = false;
    protected bool moveInput = false;

    protected float shootTimeLast = 0f;
    protected bool fire = false;




    private void OnValidate()
    {

        

    }




    private void OnEnable()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.freezeRotation = true;
        bodyCollider = GetComponent<CapsuleCollider>();

        ragdollColliders = modelTarget.GetComponentsInChildren<Collider>();
        enableRagDoll(false);

        currentDodgeCount = dodges;
        currentDoubleJumpCount = doubleJumps;
    }




    // Use this for initialization
    void Start()
    {
        gameManager = GameManager.instance;
        equipWeapon(0);
    }




    private void FixedUpdate()
    {

        // reset variables
        inputVelLast = inputVel;
        inputVel = new Vector3();

        // Handle Input
        if (!disableInput)
        {
            InputHandler();
            lookTheta = playerInput.getLookTheta();
            lookPhi = playerInput.getLookPhi();
        }

        // Handle Animations
        if (animator)
        {
            AnimationHandler();
        }


        dodgeCounter.text = "Dodges: " + currentDodgeCount;
        if (currentDodgeCount < dodges)
        {
            float t = dodgeCooldownTime - currentDodgeCooldownTime;
            dodgeCounter.text += " Cooldown: " + Mathf.Round(t * 1000.0f) / 1000.0f;
        }
        dodgeCounter.text += "\n";

        // check pickups in range
        if (pickupCollider.triggered)
        {
            pickupItem();
        }

        if( equippedWeaponIndex != equippedWeaponIndexLast)
        {
            equipWeapon(equippedWeaponIndex);
        }
        equippedWeaponIndexLast = equippedWeaponIndex;

        // Handle Movement
        inputVel.x = Mathf.Sin(camInputTheta);
        inputVel.z = Mathf.Cos(camInputTheta);

        if (!moveInput && grounded)
        {
            moveSpeed *= 0.9f;
        }
        if (moveSpeed < 0.02f)
        {
            moveSpeed = 0f;
        }

        if (animator)
        {

            //AnimatorStateInfo[] clips = animator.clip .GetCurrentAnimatorStateInfo( animator.GetLayerIndex("Aim") );
            //GameManager.SwapAnimationInState( animator, );

            //GameManager.printAnimations(animator);

            

            if (moveSpeed > 0.0f)
            {
                float t = modelTarget.rotation.eulerAngles.y * Mathf.Deg2Rad;

                Vector2 dir = new Vector2(Mathf.Sin(camInputTheta - t), Mathf.Cos(camInputTheta - t));
                animator.SetFloat("MoveHorizontal", dir.x * moveSpeed);
                animator.SetFloat("MoveVertical", dir.y * moveSpeed);
                if (animator.GetBool("Dodge"))
                {
                    animator.SetFloat("AimHorzDirX", dir.x * moveSpeed);
                    animator.SetFloat("AimHorzDirZ", dir.y * moveSpeed);
                }

            }
            else
            {
                animator.SetFloat("MoveHorizontal", 0f);
                animator.SetFloat("MoveVertical", 0f);

            }
        }
        inputVel *= moveSpeed;

        // in air movement
        if (!grounded)
        {
            inputVel = inputVel * airMovementGain + inputVelLast * (1 - airMovementGain);
        }
        else if (!animator.GetBool("Dodge"))
        {
            float gain = 0.1f;
            inputVel = inputVel * gain + inputVelLast * (1 - gain);
        }


        velocity += gravityModifier * Physics.gravity * Time.deltaTime;
        if (jumped)
        {
            //if(velocity.y + jumpSpeed <= jumpSpeed)
            velocity.y += jumpSpeed;
        }

        Vector3 deltaPos = (velocity + inputVel) * Time.deltaTime;

        if (bodyCollider.enabled)
        {
            if (!disableInput)
            {
                Vector3 move = new Vector3(deltaPos.x, 0, deltaPos.z);
                Movement(move, false);

                move = Vector3.up * deltaPos.y;
                Movement(move, true);
            }


            Vector3 rigidVelocity = new Vector3(rigidBody.velocity.x, 0f, rigidBody.velocity.z);
            if (grounded)
            {
                rigidVelocity *= 0.9f;
                rigidVelocity.y = rigidBody.velocity.y;
                rigidBody.velocity = rigidVelocity;
            }
        }
        else
        {
            bodyCollider.transform.position = ragdollColliders[1].transform.position + new Vector3(0f, 0f, 0f);
        }

        if (animator)
        {
            if (grounded)
            {
                animator.SetBool("Grounded", true);
                currentDoubleJumpCount = doubleJumps;
            }
            else
            {
                animator.SetBool("Grounded", false);
            }
        }

    }




    void InputHandler()
    {
        // Move Direction
        inputDirLast.Set(0,0,0);
        if (inputDir.magnitude > 0)
        {
            inputDirLast = inputDir;
        }

        inputDir = playerInput.getInputDir();
        
        if (inputDir.magnitude > 0f)
        {
            moveInput = true;
            
            inputThetaLast = inputTheta;

            inputSpeedLast = inputSpeed;
            inputSpeed = inputDir.magnitude;
            inputTheta = Mathf.Atan2(inputDir.x, inputDir.z);
            inputDir.Normalize();
        }
        else
        {
            moveInput = false;
            inputSpeed = 0f;
            //inputDir = inputDirLast;
            //inputTheta = inputThetaLast;
        }

        // handle On Press events
        jumped = false;
        if (playerInput.justPressedSpace() && canJump && !headTrigger.triggered)
        {
            if (grounded)
            {
                jumped = true;
            }
            else if (currentDoubleJumpCount > 0)
            {
                currentDoubleJumpCount -= 1;
                jumped = true;
            }
        }

        if (playerInput.justPressedQ()) {
            equippedWeaponIndex = switchWeaponIndex( equippedWeaponIndex );

        }

        // Handle Model and camera relative motions
        float modelRotation = lookTheta;

        camInputThetaLast = camInputTheta;

        camInputTheta = inputTheta + playerInput.getCameraInputTheta();

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("DodgeLoop"))
        {
            camInputTheta = dodgeTheta;
        }

        if (!lookInCameraDirection)
        {
            modelRotation += inputTheta * Mathf.Rad2Deg;
        }

        if (modelTarget && (moveInput || lookInCameraDirection) && bodyCollider.enabled)
        {

            float modelRotY = clipAngle(modelTarget.rotation.eulerAngles.y);
            modelRotation = clipAngle(modelRotation);

            float modelRotYDelta = modelRotation - modelRotY;
            modelRotYDelta = clipAngleHalf(modelRotYDelta);

            modelTarget.rotation = Quaternion.Euler(
                0,
                modelRotY + modelRotYDelta * 0.1f,
                0);
        }

        camInputTheta = clipAngle(camInputTheta * Mathf.Rad2Deg) * Mathf.Deg2Rad;
        


    }


    int switchWeaponIndex( int i )
    {
        int index = i + 1;
        if (index >= weaponCount) index -= weaponCount;
        return index;
    }

    void AnimationMovement()
    {
        //in air
        float clapBounds = 4.5f;
        animator.SetFloat("AirVertical", (Mathf.Clamp(velocity.y, -clapBounds, clapBounds) + clapBounds) / 2f);

        if (currentDodgeCount < dodges)
        {
            currentDodgeCooldownTime += Time.deltaTime;
            if (currentDodgeCooldownTime >= dodgeCooldownTime)
            {
                currentDodgeCount += 1;
                currentDodgeCooldownTime = 0f;
            }
        }

        // dodge
        if (
                (animator.GetCurrentAnimatorStateInfo(0).IsName("WalkLoop") ||
                    animator.GetCurrentAnimatorStateInfo(0).IsName("RunLoop"))
                &&
                (playerInput.justPressedZ() || playerInput.justPressedX()) && moveInput
                && currentDodgeCount > 0
            )
        {
            currentDodgeCount -= 1;
            animator.SetBool("Dodge", true);
            dodgeTheta = camInputTheta;
        }
        else
        {
            animator.SetBool("Dodge", false);
        }

        // sprinting
        if (playerInput.pressedLeftShift())
        {
            animator.SetBool("Running", true);

        }
        else
        {
            animator.SetBool("Running", false);
        }

        // crouching
        if (playerInput.pressedLeftCtrl())
        {

            if (!animator.GetBool("Running"))
            {
                if (!animator.GetBool("Crouching"))
                {
                    animator.SetBool("Crouching", true);
                }

                float modelDelta = (modelYOffset - modelCrouchYOffest);
                float colliderDelta = (colliderHeight - colliderCrouchHeight);

                if (modelTarget.localPosition.y < modelCrouchYOffest)
                {
                    modelTarget.localPosition -= new Vector3(0f, modelDelta * (3 * Time.deltaTime), 0f);
                }
                if (bodyCollider.height > colliderCrouchHeight)
                {
                    float dy = colliderDelta * (3 * Time.deltaTime);
                    bodyCollider.height -= dy;
                    bodyCollider.transform.position -= new Vector3(0, dy / 2, 0);
                    if (bodyCollider.height < colliderCrouchHeight) bodyCollider.height = colliderCrouchHeight;
                }
            }

        }
        else
        {

            float modelDelta = (modelYOffset - modelCrouchYOffest);
            float colliderDelta = (colliderHeight - colliderCrouchHeight);

            if (!headTrigger.triggered)
            {
                if (animator.GetBool("Crouching"))
                {
                    animator.SetBool("Crouching", false);
                }



                if (modelTarget.localPosition.y > modelYOffset)
                {
                    modelTarget.localPosition += new Vector3(0f, modelDelta * (3 * Time.deltaTime), 0f);
                }
                if (bodyCollider.height < colliderHeight)
                {
                    float dy = colliderDelta * (3 * Time.deltaTime);
                    bodyCollider.height += dy;
                    bodyCollider.transform.position += new Vector3(0, dy / 2, 0);
                    if (bodyCollider.height > colliderHeight) bodyCollider.height = colliderHeight;
                }
            }

        }
    }

    void AnimationWeaponActions(Animator weaponAnimator, WeaponData weaponScript)
    {
        if (weaponAnimator != null)
        {
            AnimatorStateInfo casi = animator.GetCurrentAnimatorStateInfo(2);

            weaponAnimator.SetInteger("ActionState", 0);

            // reload
            if (playerInput.justPressedR())
            {
                weaponAnimator.SetInteger("ActionState", 2);
                animator.SetInteger("ActionState", 2);
            }

            // fire
            shootTimeLast += Time.deltaTime;
            if (shootTimeLast > 600f) shootTimeLast = 600f;

            fire = false;
            if (casi.IsName("Fire")) { fire = true; }

            bool fireInput = false;
            switch (weaponScript.getFireType())
            {
                case 1:
                    fireInput = playerInput.pressedMouse0();
                    break;
                default:
                    fireInput = playerInput.justPressedMouse0();
                    break;
            }

            if (fireInput && !fire)
            {
                fire = true;
                shootTimeLast = 0f;
                weaponAnimator.SetInteger("ActionState", 1);
                animator.SetInteger("ActionState", 1);

                spawnBullet(weaponScript);

            }
        }
    }

    void AnimationMotionSmoothing()
    {
        AnimatorStateInfo asi = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo ansi = animator.GetNextAnimatorStateInfo(0);
        AnimatorTransitionInfo ati = animator.GetAnimatorTransitionInfo(0);

        if (asi.IsName("WalkLoop"))
        {
            if (ansi.IsName("RunLoop"))
            {
                moveSpeed = baseSpeed + (runSpeed - baseSpeed) * ati.normalizedTime;
            }
            else if (ansi.IsName("CrouchLoop"))
            {
                moveSpeed = crouchSpeed + (baseSpeed - crouchSpeed) * (1f - ati.normalizedTime);
            }
            else if (moveInput)
            {
                moveSpeed = baseSpeed;
            }
        }
        else if (asi.IsName("RunLoop"))
        {
            if (ansi.IsName("WalkLoop"))
            {
                moveSpeed = baseSpeed + (runSpeed - baseSpeed) * (1f - ati.normalizedTime);
            }
            else if (moveInput)
            {
                moveSpeed = runSpeed;
            }
        }
        else if (asi.IsName("CrouchLoop"))
        {
            if (ansi.IsName("WalkLoop"))
            {
                moveSpeed = baseSpeed - (baseSpeed - crouchSpeed) * (1f - ati.normalizedTime);
            }
            else if (moveInput)
            {
                moveSpeed = crouchSpeed;
            }
        }

        if (asi.IsName("DodgeLoop"))
        {
            moveSpeed = dodgeSpeed * (1.1f - asi.normalizedTime);
        }

    }

    void AnimationHandler()
    {

        // set vertical aiming angle
            float v = -lookPhi;
            if (v < -180) { v += 360; }
            //Debug.Log(v);
            animator.SetFloat("AimVertical", v / 90);

        //animator.GetAnimatorTransitionInfo(0).;
        AnimationMovement();

        // Set action state
        animator.SetInteger("ActionState", 0);

        if (playerInput.justPressedG())
        {
            animator.SetInteger("ActionState", 3);
        }


        // set weapon action state
        if (equippedWeapons[equippedWeaponIndex] != null)
        {
            //Debug.Log(getWeaponAnimationAndScript(equippedWeaponIndex) );
        }
        if (equippedWeaponAnimators[equippedWeaponIndex]!=null && equippedWeaponScripts[equippedWeaponIndex] != null)
        {
            AnimationWeaponActions(equippedWeaponAnimators[equippedWeaponIndex], equippedWeaponScripts[equippedWeaponIndex]);
        }
        

        // smooth movement when transition to new movement state
        AnimationMotionSmoothing();

    }

    void Movement(Vector3 move, bool vertical)
    {

        bool groundedLast = grounded;
        landed = false;
        grounded = false;

        LayerMask mask = 1 << 0;

        RaycastHit hit;
        Ray ray = new Ray();
        ray.origin = rigidBody.position;
        ray.direction = -Vector3.up;

        float offset = (bodyCollider.height / 2f) + 0.02f;

        if (Physics.Raycast(ray, out hit, offset + move.magnitude, mask))
        {

            float dist = (hit.point - rigidBody.position).magnitude;



            groundNormal = hit.normal;
            if (groundNormal.y > 0.5 && velocity.y <= 0)
            {
                if (!groundedLast) landed = true;
                grounded = true;
                velocity.y = 0f;
                rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0.0f, rigidBody.velocity.z);

            }

        }
        rigidBody.position = rigidBody.position + move;
    }



    void enableRagDoll(bool v)
    {
        for (int i = 0; i < ragdollColliders.Length; i++)
        {

            animator.enabled = !v;
            bodyCollider.enabled = !v;
            disableInput = v;

            GameObject bone = ragdollColliders[i].gameObject;
            Rigidbody rBody = bone.GetComponent<Rigidbody>();

            rBody.useGravity = v;
            rBody.isKinematic = !v;

            if (v)
            {
                bone.layer = 0;
                modelTarget.parent = null;
            }
            else
            {
                bone.layer = 20;
                modelTarget.parent = transform;
            }

            //Debug.Log(bone.name);
        }
    }

    float clipAngle(float angle)
    {
        float a = angle;
        while (a < 0) { a += 360f; }
        while (a > 360) { a -= 360f; }
        return a;
    }

    float clipAngleHalf(float angle)
    {
        float a = clipAngle(angle);
        while (a < -180) { a += 360f; }
        while (a > 180) { a -= 360f; }
        return a;
    }

    void spawnBullet(WeaponData weaponScript )
    {
        GameObject bullet = GameManager.loadPrefab(weaponScript.getBulletName(), weaponScript.getWeaponDirectory());
        bullet.SetActive(false);
        BulletData bd = bullet.GetComponent<BulletData>();
        bd.enabled = false;

        bd.setDistanceTraveled(0f);

        float theta = lookTheta * Mathf.Deg2Rad;
        float phi = -lookPhi * Mathf.Deg2Rad;
        Vector3 relSpawnPos = new Vector3(Mathf.Sin(theta) * Mathf.Cos(phi), Mathf.Sin(phi), Mathf.Cos(theta) * Mathf.Cos(phi)) * weaponScript.getBulletSpawnDistance();

        bullet.transform.rotation = Quaternion.LookRotation(relSpawnPos);

        if (animator.GetBool("Crouching"))
        {
            bullet.transform.position = transform.position + new Vector3(0, 0f, 0f) + relSpawnPos;
        }
        else
        {
            bullet.transform.position = transform.position + new Vector3(0, 0.5f, 0f) + relSpawnPos;
        }

        Vector3 bulletDir = relSpawnPos;

        RaycastHit hit;
        LayerMask mask = (1 << 0 | 1 << 20 | 1 << 17);
        if (Physics.Raycast(playerInput.getLookRay(), out hit, 1000f, mask))
        {
            bulletDir = hit.point - bullet.transform.position;
            bulletDir.Normalize();
            // Do something with the object that was hit by the raycast.
        }

        bd.setDirection(bulletDir);
        bd.setFaction("Player");
        bd.enabled = true;
        bullet.SetActive(true);
    }

    int[] getWeaponCheckOrder( int startIndex)
    {
        int[] weaponCheckOrder = new int[weaponCount];

        weaponCheckOrder[0] = startIndex;
        int currentCheckIndex = 1;
        for (int i = 0; i < weaponCount; i++)
        {
            if (i != startIndex)
            {
                weaponCheckOrder[currentCheckIndex] = i;
                currentCheckIndex++;
            }

        }
        return weaponCheckOrder;
    }

    int getBestEquippedWeaponIndex(int weaponIndex)
    {
        int bestEquippedWeaponIndex = 0;
        int[] weaponCheckOrder = getWeaponCheckOrder(weaponIndex);
        
        for (int i = 0; i < weaponCheckOrder.Length; i++)
        {
            if (getWeaponAnimationAndScript(weaponCheckOrder[i]))
            {
                bestEquippedWeaponIndex = weaponCheckOrder[i];
                break;

            }
        }

        return bestEquippedWeaponIndex;

    }

    int getEmptyEquippedWeaponIndex(int weaponIndex)
    {
        int bestEquippedWeaponIndex = 0;
        int[] weaponCheckOrder = getWeaponCheckOrder(weaponIndex);

        for (int i = 0; i < weaponCheckOrder.Length; i++)
        {
            if (!getWeaponAnimationAndScript(weaponCheckOrder[i]))
            {
                bestEquippedWeaponIndex = weaponCheckOrder[i];
                break;

            }
        }

        return bestEquippedWeaponIndex;

    }


    void equipWeapon( int weaponIndex)
    {
        WeaponData data = null;
        if (equippedWeaponIndexLast >= 0 && equippedWeapons[equippedWeaponIndexLast])
        {
            data = equippedWeaponScripts[equippedWeaponIndexLast];
            equippedWeapons[equippedWeaponIndexLast].transform.parent = animator.GetBoneTransform(data.getHolsteredBone1());
            equippedWeapons[equippedWeaponIndexLast].transform.localPosition = data.getHolsteredPositionOffset1();
            equippedWeapons[equippedWeaponIndexLast].transform.localRotation = Quaternion.Euler(data.getHolsteredEulerRotationOffset1());
        }

        equippedWeaponIndex = getBestEquippedWeaponIndex(weaponIndex);

        AnimatorOverrideController aoc;
        if (equippedWeaponAnimators[equippedWeaponIndex] != null && equippedWeaponScripts[equippedWeaponIndex] !=null)
        {
            //GameManager.printAnimations(animator);
            if (!equippedWeaponScripts[equippedWeaponIndex].isInitialized()) equippedWeaponScripts[equippedWeaponIndex].initialize();
            //equippedWeaponAnimators[equippedWeaponIndex].enabled = true;
            //gameObject.SetActive(true);
            //Debug.Log(equippedWeaponAnimators[equippedWeaponIndex].name);

            //aoc = GameManager.SwapAnimationInState(animator, 0, equippedWeaponScripts[equippedWeaponIndex].getIdleAnimation(), null);
            aoc = GameManager.SwapAnimationInState(animator, 25, equippedWeaponScripts[equippedWeaponIndex].getFireAnimation(), null);
            aoc = GameManager.SwapAnimationInState(animator, 26, equippedWeaponScripts[equippedWeaponIndex].getReloadAnimation(), aoc);
            

            aoc = GameManager.SwapAnimationInState(animator, 22, equippedWeaponScripts[equippedWeaponIndex].getAimDownAnimation(), aoc);
            aoc = GameManager.SwapAnimationInState(animator, 23, equippedWeaponScripts[equippedWeaponIndex].getIdleAnimation(), aoc);
            GameManager.SwapAnimationInState(animator, 24, equippedWeaponScripts[equippedWeaponIndex].getAimUpAnimation(), aoc);
            //GameManager.printAnimations(animator);

            data = equippedWeaponScripts[equippedWeaponIndex];
            equippedWeapons[equippedWeaponIndex].transform.SetParent( animator.GetBoneTransform(data.getEquippedBone1()) );
            //equippedWeapons[equippedWeaponIndex].transform.SetParent(transform);
            equippedWeapons[equippedWeaponIndex].transform.localPosition = data.getEquippedPositionOffset1();
            equippedWeapons[equippedWeaponIndex].transform.localRotation = Quaternion.Euler(data.getEquippedEulerRotationOffset1());

        }
        else
        {
            string dir = "Animations/Humanoid/Motion/";
            //GameManager.printAnimations(animator);
            aoc = GameManager.SwapAnimationInState(animator, 22, GameManager.loadAnimationClip("Unarmed_Aim_Down", dir), null);
            aoc = GameManager.SwapAnimationInState(animator, 23, GameManager.loadAnimationClip("Unarmed_Idle", dir), aoc);
            GameManager.SwapAnimationInState(animator, 24, GameManager.loadAnimationClip("Unarmed_Aim_Up", dir), aoc);
        }

    }

    bool getWeaponAnimationAndScript(int weaponIndex)
    {
        if( weaponIndex<0)
        {
            equippedWeaponAnimators[weaponIndex] = null;
            equippedWeaponScripts[weaponIndex] = null;
            return false;
        }

        if (equippedWeapons[weaponIndex] != null)
        {

            equippedWeaponAnimators[weaponIndex] = equippedWeapons[weaponIndex].GetComponent<Animator>();
            equippedWeaponScripts[weaponIndex] = equippedWeapons[weaponIndex].GetComponent<WeaponData>();
            equippedWeaponAnimators[weaponIndex].enabled = true;

            if (equippedWeaponAnimators[weaponIndex]==null || equippedWeaponScripts[weaponIndex] == null)
            {
                return false;
            }

            return true;
        }
        else
        {
            equippedWeaponAnimators[weaponIndex] = null;
            equippedWeaponScripts[weaponIndex] = null;
        }
        return false;
    }

    void pickupItem()
    {
        GameObject closestPickup = null;
        float dist = 0.0f;
        float distLast = -1;

        // get the closest pickup
        GameObject[] hitObjects = pickupCollider.getHitObjects();
        Debug.Log("# of Pickups: " + hitObjects.Length);
        for (int i = 0; i < hitObjects.Length; i++)
        {

            dist = (hitObjects[i].transform.position - transform.position).magnitude;
            if (distLast > 0)
            {
                if (distLast > dist && closestPickup.GetComponent<Collider>().enabled)
                {
                    closestPickup = hitObjects[i];
                }
            }
            else
            {
                closestPickup = hitObjects[i];
            }
            //Debug.Log(closestPickup.GetComponent<Collider>().name);
            //Debug.Log(closestPickup.GetComponent<Collider>().enabled);
            distLast = dist;
        }
        hitObjects = null;

        if (closestPickup != null)
        {

            PickupData closestPickupData = closestPickup.GetComponent<PickupData>();

            //Debug.Log(closestPickupData.getItemName());

            dodgeCounter.text += "E " + closestPickupData.getItemName();

            if (playerInput.justPressedE() && closestPickupData.getItemType() == 1)
            {
                int index = equippedWeaponIndex;
                equippedWeaponIndex = getEmptyEquippedWeaponIndex(equippedWeaponIndex);

                GameObject go = null;

                if (equippedWeapons[equippedWeaponIndex])
                {
                    go = equippedWeapons[equippedWeaponIndex];
                    go.GetComponent<Collider>().enabled = true;
                    go.GetComponent<Rigidbody>().useGravity = true;
                    go.GetComponent<Rigidbody>().isKinematic = false;
                    equippedWeaponScripts[equippedWeaponIndex] = null;
                    equippedWeaponAnimators[equippedWeaponIndex] = null;
                    go.transform.parent = null;
                }

                closestPickup.GetComponent<Collider>().enabled = false;
                closestPickup.GetComponent<Rigidbody>().useGravity = false;
                closestPickup.GetComponent<Rigidbody>().isKinematic = true;
                //if (!closestPickup.GetComponent<WeaponScript>().isInitialized()) closestPickup.GetComponent<WeaponScript>().initialize();
                equippedWeapons[equippedWeaponIndex] = closestPickup;
                pickupCollider.removeObject(closestPickup);
                equippedWeaponScripts[equippedWeaponIndex] = closestPickup.GetComponent<WeaponData>();
                equippedWeaponAnimators[equippedWeaponIndex] = closestPickup.GetComponent<Animator>();
                //equippedWeapons[equippedWeaponIndex].transform.parent = gameObject.transform;
                //equipWeapon(equippedWeaponIndex);
                //equippedWeaponIndexLast = equippedWeaponIndex;
                if (equippedWeaponIndexLast == equippedWeaponIndex)
                {
                    equippedWeaponIndexLast = switchWeaponIndex(equippedWeaponIndexLast);
                }
                //Debug.Log(equippedWeaponAnimators[equippedWeaponIndex]);
                Debug.Log(equippedWeaponIndex);
            }

        }
    }

}


/*
 * 
 *  layerMask = 1 << LayerMask.NameToLayer ("layerX"); // only check for collisions with layerX
 
 layerMask = ~(1 << LayerMask.NameToLayer ("layerX")); // ignore collisions with layerX
 
 LayerMask layerMask = ~(1 << LayerMask.NameToLayer ("layerX") | 1 << LayerMask.NameToLayer ("layerY")); // ignore both layerX and layerY
 * 
 * 
 */
