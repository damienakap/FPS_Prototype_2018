using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HumanoidBaseScript : MobBase
{
    public PlayerCamera playerCamera;
    public Animator fpsAnimator;
    public Transform fpsArmatureRootTransform;
    protected Transform[] fpsArmatureTransforms;

    protected Rigidbody rigidBody;
    public AnimatedRagdoll animatedRagdoll;
    public Animator ragdollAnimator;
    public PlayerModelHelper modelHelper;

    protected CapsuleCollider bodyCollider;
    public PickupCollider pickupCollider;
    public TriggerCollider headTrigger;
    public HitTriggerCollider meleeHitCollider;

    protected float meleeHitDistance = 0.7f;

    protected const int weaponCount = 2;
    public GameObject[] equippedWeapons = new GameObject[weaponCount];
    protected WeaponData[] equippedWeaponScripts = new WeaponData[weaponCount];
    protected Animator[] equippedWeaponAnimators = new Animator[weaponCount];

    protected RaycastHit lookRaycastHitOutput;
    protected LayerMask playerLayerMask = 0;
    protected bool lookRaycastHit = false;


    /*
     * 0: none
     * 1: primary
     * 2: secondary
     */
    protected int equippedWeaponIndex = 0;
    protected int equippedWeaponIndexLast = 0;

    public Transform modelTarget;
    public Animator animator;
    public float modelTurnSpeed = 0.1f;

    public Text dodgeCounter;

    public HumanoidInput playerInput;

    protected bool ragdollEnabled = false;
    protected float modelYOffset = -0.78f;
    protected float modelCrouchYOffest = -0.5f;

    protected float colliderHeight = 1.5f;
    protected float colliderCrouchHeight = 1f;
    protected float crouchGain = 2.2f;

    protected float gravityModifier = 1f;
    protected float airMovementGain = 0.003f;
    protected float groundMovementGain = 0.25f;
    protected float baseSpeed = 15.0f;
    protected float crouchSpeed = 15.0f;
    protected float runSpeed = 25.0f;
    protected float jumpSpeed = 5.0f;
    protected float dodgeSpeed = 90.0f;
    protected float groundDodgeDragValue = 0.7f;
    protected float airDodgeDragValue = 0.7f;

    protected int doubleJumps = 1;
    protected int dodges = 1;
    protected float dodgeCooldownTime = 3f;
    protected float currentDodgeCooldownTime = 0f;
    protected float dodgeDurationTime = 0.5f;
    protected float currentDodgeDurationTime = 0f;
    protected int currentDodgeCount = 0;
    protected int currentDoubleJumpCount = 0;
    protected float dodgeAnimationLength = 60f/24f;

    protected float directionTheta = 0.0f;
    protected float moveSpeed = 0.0f;
    protected float moveSpeedLast = 0.0f;
    protected float inputMoveSpeedLast = 0.0f;

    //protected Vector3 velocity = new Vector3();
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

    protected float lookTheta = 0f;
    protected float lookPhi = 0f;

    protected bool grounded = false;
    protected bool landed = false;
    protected bool jumped = false;
    protected bool moveInput = false;
    protected bool walking = false;
    protected bool dodged = false;
    protected bool crouching = false;
    protected bool reload = false;
    protected bool melee = false;
    protected bool throwGrenade = false;
    protected bool fire = false;
    protected bool canFire = true;
    protected bool altFire = false;
    protected bool swapedWeapon = false;

    protected float grenadeThrowSpeed = 15f;
    protected bool grenadeThrown = false;
    protected float grenadeSpawnDelay = 0.4f;
    protected float grenadeSpawnDelayTimer = 0f;
    protected bool punched = false;
    protected float meleeHitboxDelay = 0.2f;
    protected float meleeHitboxDelayTimer = 0f;

    protected bool newActionState = false;
    protected float actionSetTimer = 0f;

    protected float fireDelay = 1f;
    protected float burstFireDelay = 1f;
    protected float chargeDelay = 1f;
    protected float fireAnimationPlayMultiplier = 1f;

    protected float altFireDelay = 1f;
    protected float altBurstFireDelay = 1f;
    protected float altChargeDelay = 1f;
    protected float altFireAnimationPlayMultiplier = 1f;

    protected float meleeDelay = 1f;
    protected float throwGrenadeDelay = 1f;
    protected float swapDelay = 1f;
    protected float reloadDelay = 1f;


    public bool disableInput = false;
    public bool canJump = true;
    public bool firstPerson = false;
    public bool lookInCameraDirection = true;

    // transfer data
    public int playerNumber = 0;
    public int joyconNumber = 0;

    // tempVariables
    protected GameObject closestPickup = null;

    private void OnValidate()
    {
        setFirstPersonCamera(firstPerson);
    }

    // Use this for initialization
    protected void Start()
    {
        if (!animatedRagdoll.isInitialized()) animatedRagdoll.initialize();


        mobData.mobType = 1;

        // check if player number is 1-4
        bool validPlayer = GameManager.playerNumberIsValid(playerNumber);
        
        // default hitbox layer
        if (validPlayer)
        {
            // get the players' 3rd person model layer
            modelHelper.setThirdPersonModelLayers(GameManager.get3PLayer(playerNumber));

            // get the players' 1st person model layer
            modelHelper.setFirstPersonModelLayers(GameManager.get1PLayer(playerNumber));

            // set ragdoll layers
            animatedRagdoll.setRagdollLayers(GameManager.get3PLayer(playerNumber));

            // get the layer mask to ignore this players' 3rd person layer
            playerLayerMask = GameManager.getPlayerLayerMask(playerNumber);
            

        }

        //if (playerCamera)playerCamera.setJoyconNumber(joyconNumber);

        // get player prefab body collider and rigidbody component
        rigidBody = GetComponent<Rigidbody>();
        //rigidBody.freezeRotation = true;
        bodyCollider = GetComponent<CapsuleCollider>();



        enableRagDoll(true);


        if (fpsAnimator)
        {
            // get the first person armature bone transforms for weapon parenting
            fpsArmatureTransforms = fpsArmatureRootTransform.GetComponentsInChildren<Transform>();
            
            // DEBUG stuff
            //GameManager.printAnimations(fpsAnimator);
            /*
            for (int i=0; i< fpsArmatureTransforms.Length; i++)
            {
                Debug.Log(i+": "+ fpsArmatureTransforms[i].gameObject.name);
            }
            */
        }

        // initialize vars
        currentDodgeCount = dodges;
        currentDoubleJumpCount = doubleJumps;

        swapWeapon(0);
    }


    public void FixedUpdate()
    {

        if (mobData.health <= 0) return;


        // ##### HANDLE MOVEMENT #####

        if (bodyCollider.enabled && !disableInput)
        {

            // reset input velocity
            inputVelLast = inputVel;
            inputVel.Set(0, 0, 0);

            // apply camera relative motion direction (normalized)
            inputVel.x = Mathf.Sin(camInputTheta);
            inputVel.z = Mathf.Cos(camInputTheta);

            // apply move speed to the input velocity
            inputVel *= moveSpeed * Time.fixedDeltaTime;


            CheckIfGrounded();

            // Set Speed limit
            float speedLimit = runSpeed;
            if (crouching || walking)
                speedLimit = baseSpeed;
            if (currentDodgeDurationTime > 0)
                speedLimit = dodgeSpeed;
            // Check if able to apply input movement
            Vector3 newVel = inputVel;
            Vector3 oldVel = new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z);
            if (grounded || currentDodgeDurationTime>0)
            {
                if ((newVel + oldVel).magnitude > speedLimit)
                    newVel = rigidBody.velocity;
                else
                    newVel += rigidBody.velocity;
            }
            else
            {
                newVel = GameManager.slerpDirections(oldVel, newVel, 0.1f);
                newVel *= oldVel.magnitude;
                newVel += new Vector3(0, rigidBody.velocity.y, 0);
            }
            

            // Apply Jump
            if (jumped)
                newVel += new Vector3(0, jumpSpeed, 0);

            // Apply Gravity
            newVel += gravityModifier * Physics.gravity * Time.fixedDeltaTime;

            // Apply Drag
            float drag = 0.99f;
            float dodgeDrag = airDodgeDragValue;
            if (grounded)
            {
                dodgeDrag = groundDodgeDragValue;
                drag = 0.9f;
            }

            
            if (currentDodgeDurationTime > 0)
            {
                float gain = (currentDodgeDurationTime/ dodgeDurationTime);
                drag = drag * gain + dodgeDrag * (1 - gain);
            }
                
                

            newVel = new Vector3( newVel.x* drag, newVel.y, newVel.z* drag);

            // Set Velocity
            rigidBody.velocity = newVel;


        }

        jumped = false;
        if (grounded) currentDoubleJumpCount = doubleJumps;





        // ##### UPDATE ANIMATION MOTION VARIABLES #####

        animator.SetBool("Grounded", grounded);
            // get model yaw
            float t = modelTarget.rotation.eulerAngles.y * Mathf.Deg2Rad;

            // apply move direction relative to the model rotation (yaw)
            Vector2 dir = new Vector2(Mathf.Sin(camInputTheta - t), Mathf.Cos(camInputTheta - t)) * moveSpeed;
            // get the last applied move direction relative to the model
            Vector2 dirLast = new Vector2(animator.GetFloat("MoveHorizontal"), animator.GetFloat("MoveVertical"));

            // blend direction change of the model
            dir = dir * groundMovementGain + dirLast * (1 - groundMovementGain);

            // set Move vector relative to the model (controls walk animation direction)
            animator.SetFloat("MoveHorizontal", dir.x);
            animator.SetFloat("MoveVertical", dir.y);


    }

    void CheckIfGrounded()
    {

        bool groundedLast = grounded;
        landed = false;
        grounded = false;


        LayerMask mask = (1 << 0) | (1 << 22) | (1 << 8);

        RaycastHit hit;
        Ray ray = new Ray();
        ray.origin = transform.position;
        ray.direction = transform.TransformDirection(Vector3.down);

        float offset = (bodyCollider.height / 2f) + 0.01f;
        float moveY = rigidBody.velocity.y * Time.fixedDeltaTime;
        if (moveY > 0) moveY = 0f;

        if (Physics.Raycast(ray, out hit, offset - moveY, mask))
        {

            //Debug.Log(rigidBody.velocity.y);
            //Debug.Log(jumped);
            if (hit.normal.y > 0.7f && rigidBody.velocity.y < 0.001f)
            {
                grounded = true;
                //Debug.Log("Grounded");
            }
            //Debug.Log(hit.transform.gameObject.name);
            //Debug.Log(hit.normal.y);

        }

    }

    public bool enableHitRagdoll = false;

    public void LateUpdate()
    {

    }

    public override void Update()
    {
        base.Update();

        // TEMPORARY: code for handling health
        if(mobData.health <= 0)
        {
            mobData.health = 0f;
            enableRagDoll(true);
        }
        else
        {
            enableRagDoll(false);
        }

       
            
            

        // ##### HANDLE INPUT #####
        if (!disableInput)
        {
            lookTheta = playerInput.getLookYaw();
            lookPhi = playerInput.getLookPitch();

            float theta = lookTheta * Mathf.Deg2Rad;
            float phi = -lookPhi * Mathf.Deg2Rad;
            lookDirection = new Vector3(Mathf.Sin(theta) * Mathf.Cos(phi), Mathf.Sin(phi), Mathf.Cos(theta) * Mathf.Cos(phi));
            lookDirection.Normalize();

            lookPoint = playerInput.getLookPoint();

            bulletSpawnCenter = new Vector3(0, 0.5f, 0) + transform.position;

            InputHandler();

        }


        // ##### HANDLE WEAPON PICKUP, SWAP, AND EQUIP #####
        // check pickups in range
        swapedWeapon = false;
        closestPickup = null;
        if (pickupCollider.triggered)
        {
            pickupItem();
        }

        // check if weapon swapped
        if (equippedWeaponIndex != equippedWeaponIndexLast)
        {
            swapWeapon(equippedWeaponIndex);
        }
        equippedWeaponIndexLast = equippedWeaponIndex;

        // ##### HANDLE ANIMATIONS #####
        AnimationHandler();


        // TEMPORARY: update hud 
        if (dodgeCounter)
        {
            dodgeCounter.text = "Dodges: " + currentDodgeCount;
            if (currentDodgeCount < dodges)
            {
                dodgeCounter.text += " Cooldown: " + Mathf.Round(currentDodgeCooldownTime * 1000.0f) / 1000.0f;
            }
            dodgeCounter.text += "\n";

            if (closestPickup)
            {
                dodgeCounter.text += "[E] Pick up "+ closestPickup.GetComponent<PickupData>().getItemName() +"\n";
            }

            if (equippedWeaponScripts[equippedWeaponIndex])
            {
                dodgeCounter.text += ItemHelper.getWeaponPrefabName( equippedWeaponScripts[equippedWeaponIndex].GetWeaponIndex() ) + "\n";
                dodgeCounter.text += "Ammo: " + equippedWeaponScripts[equippedWeaponIndex].getAmmo() + "    Clip: " + equippedWeaponScripts[equippedWeaponIndex].getClipAmmo() + "\n";
            }
        }

    }

    void InputHandler()
    {
        // Move Direction
        inputDirLast = inputDir;
        inputDir = playerInput.getInputMoveDir();

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
        //jumped = false;
        if (!jumped && playerInput.justPressedSpace() && canJump && !headTrigger.triggered)
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

        if (Input.GetKeyDown(KeyCode.H))
        {
            firstPerson = !firstPerson;
            setFirstPersonCamera(firstPerson);
        }

        // toggle walk
        if (playerInput.justPressedZ()) walking = !walking;

        // dodge
        dodged = false;
        if (playerInput.justPressedLeftShift() && moveInput && currentDodgeCount>0) dodged = true;

        // crouch
        crouching = false;
        if (playerInput.pressedLeftCtrl() && currentDodgeDurationTime==0) crouching = true;

        reload = false;
        throwGrenade = false;
        melee = false;
        fire = false;
        altFire = false;
        canFire = false;

        newActionState = false;

        actionSetTimer -= Time.deltaTime;
        if (actionSetTimer < 0f) actionSetTimer = 0f;

        fire = playerInput.pressedMouse0();
        altFire = playerInput.pressedFire2();

        while ( actionSetTimer == 0f )
        {
            canFire = true;
            actionSetTimer = -0.001f;
            // swap weapon
            if (playerInput.justPressedQ())
            {
                equippedWeaponIndex = switchWeaponIndex(equippedWeaponIndex);
                newActionState = true;
                Debug.Log("Switched to: " + equippedWeaponIndex);
                break;
            }

            // reload
            if (playerInput.justPressedR())
            {
                actionSetTimer = reloadDelay;
                newActionState = true;
                reload = true;
                break;
            }


            // throw grenade
            if (playerInput.justPressedG())
            {
                actionSetTimer = throwGrenadeDelay;
                throwGrenade = true;
                newActionState = true;
                break;
            }

            // melee
            if (playerInput.justPressedF())
            {
                actionSetTimer = meleeDelay;
                newActionState = true;
                melee = true;
                break;
            }
            break;
        }



        // Handle Model and camera relative motions

        float modelRotation = lookTheta;

        camInputThetaLast = camInputTheta;

        camInputTheta = inputTheta + playerInput.getLookYaw() * Mathf.Deg2Rad;

        if (currentDodgeDurationTime > 0)
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
                modelRotY + modelRotYDelta * modelTurnSpeed,
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
        animator.SetFloat("AirVertical", (Mathf.Clamp(rigidBody.velocity.y, -clapBounds, clapBounds) + clapBounds) / 2f);

        currentDodgeCooldownTime -= Time.deltaTime;
        currentDodgeCooldownTime = clampLower(0f, currentDodgeCooldownTime);

        currentDodgeDurationTime -= Time.deltaTime;
        currentDodgeDurationTime = clampLower(0f, currentDodgeDurationTime);

        if (currentDodgeCount < dodges && currentDodgeCooldownTime == 0)
        {
            currentDodgeCount += 1;
            if(currentDodgeCount < dodges)
                currentDodgeCooldownTime = dodgeCooldownTime;
        }
        // dodge
        if ( dodged && currentDodgeCount > 0 )
        {
            currentDodgeCount -= 1;
            currentDodgeDurationTime = dodgeDurationTime;
            currentDodgeCooldownTime = dodgeCooldownTime;
            dodgeTheta = camInputTheta;
            animator.SetFloat("DodgePlaySpeed", dodgeAnimationLength / dodgeDurationTime);
        }
        animator.SetBool("Dodge", dodged);

        // sprinting
        animator.SetBool("Running", !walking);

        // crouching
        float modelDelta = (modelYOffset - modelCrouchYOffest);
        float colliderDelta = (colliderHeight - colliderCrouchHeight);
        float modelGain = crouchGain;
        float colliderGain = modelGain;
        if (crouching)
        {
            animator.SetBool("Crouching", true);

            if (modelTarget.localPosition.y < modelCrouchYOffest)
                modelTarget.localPosition -= new Vector3(0f, modelDelta * (modelGain * Time.deltaTime), 0f);
            else
                modelTarget.localPosition = new Vector3(0, modelCrouchYOffest, 0);

            if (bodyCollider.height > colliderCrouchHeight)
            {
                float dy = colliderDelta * (colliderGain * Time.deltaTime);
                bodyCollider.height -= dy;
                if (grounded)
                {
                    transform.position -= new Vector3(0, dy/2, 0);
                }
                else
                {
                    transform.position += new Vector3(0, dy/2, 0);
                }
                if (bodyCollider.height < colliderCrouchHeight) bodyCollider.height = colliderCrouchHeight;
            }

        }
        else
        {

            if (!headTrigger.triggered)
            {
                animator.SetBool("Crouching", false);
                //modelTarget.localPosition = new Vector3(0, modelYOffset, 0)
                if (modelTarget.localPosition.y > modelYOffset)
                    modelTarget.localPosition += new Vector3(0f, modelDelta * modelGain * Time.deltaTime, 0f);
                else
                    modelTarget.localPosition = new Vector3(0, modelYOffset,0);
                if (bodyCollider.height < colliderHeight)
                {
                    float dy = colliderDelta * (colliderGain * Time.deltaTime);
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
            if (reload  && weaponScript.canReload())
            {
                weaponScript.reload();
                weaponAnimator.SetInteger("ActionState", 2);
                animator.SetInteger("ActionState", 2);

                if(fpsAnimator != null)
                {
                    fpsAnimator.SetInteger("ActionState", 2);
                }
            }

            // fire
            Vector3 lookRayCastPoint = new Vector3();
            if (lookRaycastHit)
            {
                lookRayCastPoint = lookRaycastHitOutput.point;
            }

            if(newActionState) canFire = false;
            int altFireState = weaponScript.altFire(altFire, canFire, lookDirection, bulletSpawnCenter, lookPoint, playerNumber, gameObject);
            
            if (altFireState > 0) canFire = false;
            int fireState = weaponScript.fire(fire, canFire, lookDirection, bulletSpawnCenter, lookPoint, playerNumber, gameObject);
            
            switch (fireState)
            {
                case 1:
                    weaponAnimator.SetInteger("ActionState",6);
                    animator.SetInteger("ActionState", 6);
                    if (fpsAnimator)
                        fpsAnimator.SetInteger("ActionState", 6);

                    break;
                case 2:
                    weaponAnimator.SetInteger("ActionState", 1);
                    animator.SetInteger("ActionState", 1);
                    if (fpsAnimator)
                        fpsAnimator.SetInteger("ActionState", 1);

                    fireAnimationPlayMultiplier = weaponScript.getFireAnimationLength() / burstFireDelay;
                    actionSetTimer = fireDelay;
                    newActionState = true;

                    break;
            }

            playerCamera.setFOV(playerCamera.getBaseFOV());
            switch (altFireState)
            {
                case -1:
                    playerCamera.setFOV(45f);
                    break;
                case 1:
                    weaponAnimator.SetInteger("ActionState", 8);
                    animator.SetInteger("ActionState", 8);
                    if (fpsAnimator)
                        fpsAnimator.SetInteger("ActionState", 8);

                    break;
                case 2:
                    weaponAnimator.SetInteger("ActionState", 7);
                    animator.SetInteger("ActionState", 7);
                    if (fpsAnimator)
                        fpsAnimator.SetInteger("ActionState", 7);

                    fireAnimationPlayMultiplier = weaponScript.getFireAnimationLength() / altBurstFireDelay;
                    actionSetTimer = altFireDelay;
                    newActionState = true;

                    break;
            }

            if (weaponAnimator) { weaponAnimator.SetBool("NewActionState", newActionState); }
            
        }
    }

    void AnimationMotionSmoothing()
    {
        AnimatorStateInfo asi = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo ansi = animator.GetNextAnimatorStateInfo(0);
        AnimatorTransitionInfo ati = animator.GetAnimatorTransitionInfo(0);


        float inputSpeed = playerInput.getInputMoveSpeed();

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
            else
            {
                moveSpeed = baseSpeed * inputSpeed;
            }
        }
        else if (asi.IsName("RunLoop"))
        {
            if (ansi.IsName("WalkLoop"))
            {
                moveSpeed = baseSpeed + (runSpeed - baseSpeed) * (1f - ati.normalizedTime);
            }
            else
            {
                moveSpeed = runSpeed * inputSpeed;
            }
        }
        else if (asi.IsName("CrouchLoop"))
        {
            if (ansi.IsName("WalkLoop"))
            {
                moveSpeed = baseSpeed - (baseSpeed - crouchSpeed) * (1f - ati.normalizedTime);
            }
            else
            {
                moveSpeed = crouchSpeed * inputSpeed;
            }
        }

        moveSpeed = playerInput.getInputMoveSpeed() * runSpeed;

        if (currentDodgeDurationTime >0f)
            moveSpeed = dodgeSpeed * (0.5f + currentDodgeDurationTime / dodgeDurationTime);

        inputMoveSpeedLast = playerInput.getInputMoveSpeed();

        moveSpeedLast = moveSpeed;

    }

    void AnimationHandler()
    {

        // set vertical aiming angle
        float v = -lookPhi;
        if (v < -180) { v += 360; }
        //Debug.Log(v);
        animator.SetFloat("AimVertical", v / 90);
        
        // set melee collider
        meleeHitCollider.transform.position = lookDirection * meleeHitDistance;
        meleeHitCollider.transform.position += bulletSpawnCenter;
        

        //animator.GetAnimatorTransitionInfo(0).;
        AnimationMovement();

        // Set action state
        animator.SetInteger("ActionState", 0);
        if (fpsAnimator)
        {
            fpsAnimator.SetInteger("ActionState", 0);
        }

        // grenade action
        grenadeSpawnDelayTimer -= Time.deltaTime;
        if (grenadeSpawnDelayTimer < 0) grenadeSpawnDelayTimer = 0f;

        if (grenadeThrown && grenadeSpawnDelayTimer == 0)
        {
            grenadeThrown = false;
            spawnGrenade();
        }

        // melee action
        meleeHitboxDelayTimer -= Time.deltaTime;
        if (meleeHitboxDelayTimer < 0) meleeHitboxDelayTimer = 0f;

        if (punched && meleeHitboxDelayTimer == 0)
        {
            punched = false;
            if (meleeHitCollider.triggered)
            {
                Collider[] hitColliders = meleeHitCollider.getHitboxObjects();
                HitBoxScript hitBox;
                for (int i = 0; i < hitColliders.Length; i++)
                {
                    hitBox = hitColliders[i].GetComponent<HitBoxScript>();
                    if (hitBox != null)
                    {
                        hitBox.applyDamage(100, 0);
                    }
                }
                hitColliders = meleeHitCollider.getHitObjects();
                Rigidbody tempRigidbody;
                Vector3[] tempVects = meleeHitCollider.getHitObjectPoints();
                for (int i = 0; i < hitColliders.Length; i++)
                {
                    tempRigidbody = hitColliders[i].GetComponent<Rigidbody>();
                    tempRigidbody.AddForceAtPosition( lookDirection*1000, tempVects[i], ForceMode.Force);


                }
            }
        }

        
        if (throwGrenade)
        {
            grenadeThrown = true;

            if (fpsAnimator)
            {
                fpsAnimator.SetInteger("ActionState", 3);
            }
            animator.SetInteger("ActionState", 3);
        }

        if (melee)
        {
            punched = true;
            animator.SetInteger("ActionState", 4);

            if (fpsAnimator)
            {
            fpsAnimator.SetInteger("ActionState", 4);
            }
        }


        if (swapedWeapon)
        {
            animator.SetInteger("ActionState", 5);
            if (fpsAnimator)
            {
                fpsAnimator.SetInteger("ActionState", 5);
            }
        }


        // set weapon action state
        if (equippedWeapons[equippedWeaponIndex] != null)
        {
            //Debug.Log(getWeaponAnimationAndScript(equippedWeaponIndex) );
        }
        if (equippedWeaponAnimators[equippedWeaponIndex] != null && equippedWeaponScripts[equippedWeaponIndex] != null)
        {
            AnimationWeaponActions(equippedWeaponAnimators[equippedWeaponIndex], equippedWeaponScripts[equippedWeaponIndex]);
        }


        float temp = 0;

        animator.speed = 1;
        switch (animator.GetInteger("ActionState"))
        {
            case 1:
                animator.Play("Fire", 2, 0f);
                break;
            case 2:
                animator.Play("Reload", 2, 0f);
                break;
            case 3:
                animator.Play("ThrowGrenade", 2, 0f);
                break;
            case 4:
                animator.Play("Melee", 2, 0f);
                break;
            case 5:
                animator.Play("Swap", 2, 0f);
                break;
            case 6:
                //animator.Play("Charge", 2, 0f);
                break;
            case 7:
                animator.Play("AltFire", 2, 0f);
                break;


        }

        if (equippedWeaponScripts[equippedWeaponIndex])
        {
            switch (animator.GetInteger("ActionState"))
            {
                case 1:
                    //animator.SetFloat("FirePlaySpeed", equippedWeaponScripts[equippedWeaponIndex].getFireAnimationLength() / burstFireDelay);
                    animator.SetFloat("FirePlaySpeed", fireAnimationPlayMultiplier);
                    break;
                case 2:
                    animator.SetFloat("ReloadPlaySpeed", equippedWeaponScripts[equippedWeaponIndex].getReloadAnimationLength() / reloadDelay);
                    break;
                case 3:
                    temp = 1f / throwGrenadeDelay;
                    animator.SetFloat("ThrowGrenadePlaySpeed", temp);
                    grenadeSpawnDelayTimer = grenadeSpawnDelay / temp;
                    break;
                case 4:
                    temp = equippedWeaponScripts[equippedWeaponIndex].getMeleeAnimationLength() / meleeDelay;
                    animator.SetFloat("MeleePlaySpeed", temp);
                    meleeHitboxDelayTimer = meleeHitboxDelay / temp;
                    break;
                case 5:
                    if (equippedWeaponScripts[equippedWeaponIndex])
                        animator.SetFloat("SwapPlaySpeed", equippedWeaponScripts[equippedWeaponIndex].getSwapAnimationLength() / swapDelay);
                    break;
                case 6:
                    if (equippedWeaponScripts[equippedWeaponIndex])
                        animator.SetFloat("ChargePlaySpeed", equippedWeaponScripts[equippedWeaponIndex].getChargeAnimationLength() / chargeDelay);
                    break;
                case 7:
                    animator.SetFloat("AltFirePlaySpeed", fireAnimationPlayMultiplier);
                    break;
            }
        }



        if (fpsAnimator)
        {
            switch (fpsAnimator.GetInteger("ActionState"))
            {
                case 1:
                    fpsAnimator.Play("Fire", 0, 0f);
                    break;
                case 2:
                    fpsAnimator.Play("Reload", 0, 0f);
                    break;
                case 3:
                    fpsAnimator.Play("ThrowGrenade", 0, 0f);
                    break;
                case 4:
                    fpsAnimator.Play("Melee", 0, 0f);
                    break;
                case 5:
                    fpsAnimator.Play("Swap", 0, 0f);
                    break;
                case 6:
                    //fpsAnimator.Play("Charge", 0, 0f);
                    break;
                case 7:
                    fpsAnimator.Play("AltFire", 0, 0f);
                    break;

            }

            if (equippedWeaponScripts[equippedWeaponIndex])
            {
                switch (fpsAnimator.GetInteger("ActionState"))
                {
                    case 1:
                        //fpsAnimator.SetFloat("FirePlaySpeed", equippedWeaponScripts[equippedWeaponIndex].getFpsFireAnimationLength() / burstFireDelay);
                        fpsAnimator.SetFloat("FirePlaySpeed", fireAnimationPlayMultiplier);
                        break;
                    case 2:
                        fpsAnimator.SetFloat("ReloadPlaySpeed", equippedWeaponScripts[equippedWeaponIndex].getFpsReloadAnimationLength() / reloadDelay);
                        break;
                    case 3:
                        fpsAnimator.SetFloat("ThrowGrenadePlaySpeed", 1f / throwGrenadeDelay);
                        break;
                    case 4:
                        fpsAnimator.SetFloat("MeleePlaySpeed", equippedWeaponScripts[equippedWeaponIndex].getFpsMeleeAnimationLength() / meleeDelay);
                        break;
                    case 5:
                        fpsAnimator.SetFloat("SwapPlaySpeed", equippedWeaponScripts[equippedWeaponIndex].getFpsSwapAnimationLength() / swapDelay);
                        break;
                    case 6:
                        fpsAnimator.SetFloat("ChargePlaySpeed", equippedWeaponScripts[equippedWeaponIndex].getFpsChargeAnimationLength() / chargeDelay);
                        break;
                    case 7:
                        //fpsAnimator.SetFloat("FirePlaySpeed", equippedWeaponScripts[equippedWeaponIndex].getFpsFireAnimationLength() / burstFireDelay);
                        fpsAnimator.SetFloat("AltFirePlaySpeed", fireAnimationPlayMultiplier);
                        break;
                }

            }
        }

        // smooth movement when transition to new movement state
        AnimationMotionSmoothing();

    }

   

    void enableRagDoll(bool v)
    {
        if( v == ragdollEnabled) { return; }

        

        int layer = 20;
        if (v)
            layer = 0;

        animator.enabled = !v;
        bodyCollider.enabled = !v;
        disableInput = v;

        if (v)
        {
            //modelTarget.parent = null;
            animatedRagdoll.setCopyArmature(false);
            animatedRagdoll.setLockToArmature(false);
            animatedRagdoll.setRagdollLayers(20);
            rigidBody.velocity = new Vector3();
            setFirstPersonCamera(false);
        }
        else
        {
            animatedRagdoll.setCopyArmature(true);
            animatedRagdoll.setLockToArmature(true);
            animatedRagdoll.setRagdollLayers(GameManager.get3PLayer(playerNumber));
            setFirstPersonCamera(firstPerson);
            //modelTarget.parent = transform;
            modelTarget.transform.localPosition = new Vector3(0f, modelYOffset ,0f);
            bodyCollider.transform.position += new Vector3(0, 0.1f, 0);
            rigidBody.velocity = new Vector3();
        }
        ragdollEnabled = v;

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

    void spawnGrenade() {
        GameObject grenade = GameManager.loadPrefab("Frag_Grenade", "Items/Weapons/Frag_Grenade/");
        grenade.transform.position = lookDirection + bulletSpawnCenter;

        Vector3 throwVec = lookDirection;
        throwVec.Normalize();
        grenade.GetComponent<Rigidbody>().velocity = throwVec * grenadeThrowSpeed;
        FragGrenadeScript fragScript = grenade.GetComponent<FragGrenadeScript>();
        fragScript.timer = 1f;
        fragScript.resetGrenade();
        grenade.SetActive(true);
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
        int[] weaponCheckOrder = getWeaponCheckOrder(weaponIndex);

        for (int i = 0; i < weaponCheckOrder.Length; i++)
        {
            if (!getWeaponAnimationAndScript(weaponCheckOrder[i]))
            {
                return weaponCheckOrder[i];

            }
        }

        return weaponIndex;

    }

    /// <summary>
    /// Swap to weapon in index/slot
    /// </summary>
    /// <param name="weaponIndex">weapon slot index</param>
    void swapWeapon( int weaponIndex)
    {

        // holster previously equipped weapon if not null
        if (equippedWeaponIndexLast >= 0 && equippedWeapons[equippedWeaponIndexLast])
        {
            holsterWeapon(equippedWeaponIndexLast);
        }

        // get an empty slot if available or use the current weapon slot/index
        equippedWeaponIndex = getBestEquippedWeaponIndex(weaponIndex);

        AnimatorOverrideController aoc = null;
        if (equippedWeaponAnimators[equippedWeaponIndex] != null && equippedWeaponScripts[equippedWeaponIndex] !=null)
        {
            // initialize weapon if needed
            if (!equippedWeaponScripts[equippedWeaponIndex].isInitialized()) equippedWeaponScripts[equippedWeaponIndex].initialize();

            
            // replace player action animations with weapon action animations
            aoc = GameManager.SwapAnimationInState(animator, 31, equippedWeaponScripts[equippedWeaponIndex].getAltFireAnimation(), aoc);  // alt fire
            aoc = GameManager.SwapAnimationInState(animator, 25, equippedWeaponScripts[equippedWeaponIndex].getFireAnimation(), aoc);  // primary fire
            aoc = GameManager.SwapAnimationInState(animator, 26, equippedWeaponScripts[equippedWeaponIndex].getReloadAnimation(), aoc);
            //aoc = GameManager.SwapAnimationInState(animator, 27, equippedWeaponScripts[equippedWeaponIndex].getReloadAnimation(), aoc);
            aoc = GameManager.SwapAnimationInState(animator, 28, equippedWeaponScripts[equippedWeaponIndex].getMeleeAnimation(), aoc);
            aoc = GameManager.SwapAnimationInState(animator, 29, equippedWeaponScripts[equippedWeaponIndex].getSwapAnimation(), aoc);
            aoc = GameManager.SwapAnimationInState(animator, 30, equippedWeaponScripts[equippedWeaponIndex].getChargeAnimation(), aoc); // primary charge
            aoc = GameManager.SwapAnimationInState(animator, 32, equippedWeaponScripts[equippedWeaponIndex].getAltChargeAnimation(), aoc); // alt charge

            aoc = GameManager.SwapAnimationInState(animator, 22, equippedWeaponScripts[equippedWeaponIndex].getAimDownAnimation(), aoc);
            aoc = GameManager.SwapAnimationInState(animator, 23, equippedWeaponScripts[equippedWeaponIndex].getIdleAnimation(), aoc);
            GameManager.SwapAnimationInState(animator, 24, equippedWeaponScripts[equippedWeaponIndex].getAimUpAnimation(), aoc);

            //GameManager.printAnimations(animator);

            WeaponData data = equippedWeaponScripts[equippedWeaponIndex];

            if (fpsAnimator)
            {
                // replace player first-person action animations with weapon first-person action animations
                aoc = GameManager.SwapAnimationInState(fpsAnimator, 0, equippedWeaponScripts[equippedWeaponIndex].getFpsIdleAnimation(), null);
                aoc = GameManager.SwapAnimationInState(fpsAnimator, 1, equippedWeaponScripts[equippedWeaponIndex].getFpsFireAnimation(), aoc);      // primary fire
                aoc = GameManager.SwapAnimationInState(fpsAnimator, 7, equippedWeaponScripts[equippedWeaponIndex].getFpsAltFireAnimation(), aoc);   // alt fire
                aoc = GameManager.SwapAnimationInState(fpsAnimator, 2, equippedWeaponScripts[equippedWeaponIndex].getFpsReloadAnimation(), aoc);
                //aoc = GameManager.SwapAnimationInState(fpsAnimator, 3, equippedWeaponScripts[equippedWeaponIndex].getFpsReloadAnimation(), aoc);
                aoc = GameManager.SwapAnimationInState(fpsAnimator, 4, equippedWeaponScripts[equippedWeaponIndex].getFpsMeleeAnimation(), aoc);
                aoc = GameManager.SwapAnimationInState(fpsAnimator, 5, equippedWeaponScripts[equippedWeaponIndex].getFpsSwapAnimation(), aoc);
                aoc = GameManager.SwapAnimationInState(fpsAnimator, 6, equippedWeaponScripts[equippedWeaponIndex].getFpsChargeAnimation(), aoc);    // primary charge
                GameManager.SwapAnimationInState(fpsAnimator, 8, equippedWeaponScripts[equippedWeaponIndex].getFpsAltChargeAnimation(), aoc);       // altCharge


                data.equip1P(fpsArmatureTransforms[data.getEquippedFpsBone1()]);

                //for (int i=0; i< fpsArmatureTransforms.Length; i++){ Debug.Log(i + ": " + fpsArmatureTransforms[i].name); }
                //GameManager.printAnimations(fpsAnimator);
            }
            swapedWeapon = true;

            data.equip3P(ragdollAnimator.GetBoneTransform(data.getEquippedBone1()));

            // set the weapon layers for the specific players first and third-person view layers
            if(GameManager.playerNumberIsValid(playerNumber))
                data.setModelLayers(playerNumber);

            // get the action time delays from the weapon data
            fireDelay = 1f / data.getFireRate();
            burstFireDelay = 1f / data.getBurstFireRate();
            chargeDelay = 1f / data.getChargeRate();

            altFireDelay = 1f / data.getAltFireRate();
            altBurstFireDelay = 1f / data.getAltBurstFireRate();
            altChargeDelay = 1f / data.getAltChargeRate();

            reloadDelay = 1f / data.getReloadSpeed();
            meleeDelay = 1f / data.getMeleeSpeed();
            swapDelay = 1f / data.getSwapSpeed();

            data.resetAltTriggerState();
            data.resetTriggerState();

            actionSetTimer = swapDelay;
            newActionState = true;

        }
        else
        {
            // set unarmed action animations
            string dir = "Animations/Humanoid/Motion/";
            aoc = GameManager.SwapAnimationInState(animator, 22, GameManager.loadAnimationClip("Unarmed_Aim_Down", dir), null);
            aoc = GameManager.SwapAnimationInState(animator, 23, GameManager.loadAnimationClip("Unarmed_Idle", dir), aoc);
            GameManager.SwapAnimationInState(animator, 24, GameManager.loadAnimationClip("Unarmed_Aim_Up", dir), aoc);
        }

    }

    void holsterWeapon( int index )
    {
        WeaponData data = equippedWeaponScripts[index];
        data.transform.parent = ragdollAnimator.GetBoneTransform(data.getHolsteredBone1());
        data.transform.localPosition = data.getHolsteredPositionOffset1();
        data.transform.localRotation = Quaternion.Euler(data.getHolsteredEulerRotationOffset1());
        data.holster();
        data.resetAltTriggerState();
        data.resetTriggerState();
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
        closestPickup = null;
        float dist = 0.0f;
        float distLast = -1;

        // get the closest pickup
        GameObject[] hitObjects = pickupCollider.getHitObjects();
        //Debug.Log("# of Pickups: " + hitObjects.Length);
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

            if (dodgeCounter != null)
            {
                dodgeCounter.text += "E " + closestPickupData.getItemName();
            }

            if (playerInput.justPressedE() && closestPickupData.getItemType() == 1)
            {
                equippedWeaponIndex = getEmptyEquippedWeaponIndex(equippedWeaponIndex);

                Debug.Log("Pickup: "+equippedWeaponIndex);

                equipWeaponToSlot(closestPickup, equippedWeaponIndex);

                if(equippedWeaponIndex == equippedWeaponIndexLast)
                {
                    equippedWeaponIndexLast = switchWeaponIndex(equippedWeaponIndexLast);
                }

                pickupCollider.removeObject(closestPickup);
                //Debug.Log(equippedWeaponAnimators[equippedWeaponIndex]);
                //Debug.Log(equippedWeaponIndex);
            }

        }
    }

    public void equipWeaponToSlot( GameObject wep,int index )
    {


        if (equippedWeapons[index])
        {
            GameObject go = equippedWeapons[index];
            equippedWeapons[index].GetComponent<Collider>().enabled = true;
            equippedWeapons[index].GetComponent<Rigidbody>().useGravity = true;
            equippedWeapons[index].GetComponent<Rigidbody>().isKinematic = false;
            equippedWeaponScripts[index].resetModelLayers();
            equippedWeaponScripts[index].holster();
            equippedWeaponScripts[index] = null;
            equippedWeaponAnimators[index] = null;
            equippedWeapons[index].transform.parent = null;
        }

        wep.GetComponent<Collider>().enabled = false;
        wep.GetComponent<Rigidbody>().useGravity = false;
        wep.GetComponent<Rigidbody>().isKinematic = true;
        
        equippedWeapons[index] = wep;
        equippedWeaponScripts[index] = wep.GetComponent<WeaponData>();
        equippedWeaponScripts[index].setModelLayers(playerNumber);

        equippedWeaponAnimators[index] = wep.GetComponent<Animator>();

        if (index != equippedWeaponIndex)
            holsterWeapon(index);


    }

    void setFirstPersonCamera(bool b)
    {
        if (playerCamera){ playerCamera.setFirstPerson(b); }
    }

    float clampLower(float lower, float value)
    {
        if (value < lower) return lower;
        return value;
    }

    public PlayerCamera GetPlayerCameraScript() { return playerCamera; }
    public Camera GetCamera() { return playerCamera.myCamera; }
    

    public void setup(int playerNum, int joyconNum)
    {
        playerNumber = playerNum;
        joyconNumber = joyconNum;
        HumanoidPlayerInput hpi = GetComponent<HumanoidPlayerInput>();
        if (hpi)
        {
            hpi.setup(playerNum, joyconNum);
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
