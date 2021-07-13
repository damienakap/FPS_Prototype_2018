
/*
 * 
 * Author: Damien Apilando
 * Email : damienakap@gmail.com
 * 
 * Last Update: 2/6/2019
 * 
 * Notes:
 * - ragdoll need to be in the default pose ( T-pose for
 * humanoid rigs ).
 * 
 * - Each ragdoll rigid body uses the Unity configurable 
 * joint. ( not recommended to change any settings except
 * for setting the connected rigidbody )
 * 
 * - Needs animated ragdoll profile classes to set the joint
 * spring strength, spring damp, connected scaled mass, and 
 * scaled mass.
 * 
 * - Currently, the hard lock settings are not functional.
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedRagdoll : MonoBehaviour
{


    [SerializeField] private Transform ragdollPrefabTransform;

    [SerializeField] private AnimatedRagdollProfile attachedProfile;
    [SerializeField] private AnimatedRagdollProfile detachedProfile;
    //[SerializeField] private AnimatedRagdollProfile detachedProfile;

    [Header("Controls")]
    [SerializeField] private bool lockToArmature = true;
    [SerializeField] private bool copyArmature = true;

    [Header("Enabled Hard Locked Ragdoll Parts [Not Working] ")]
    [SerializeField] private bool hardLockHead = false;
    [SerializeField] private bool hardLockTorso = false;
    [SerializeField] private bool hardLockLegs = false;

    [Header("Ragdoll Rigidbodies")]
    [SerializeField] [Tooltip("*Needs configurable Joint")] private Rigidbody ragdollHips = null;
    [SerializeField] [Tooltip("*Needs configurable Joint")] private Rigidbody ragdollTorso = null;
    [SerializeField] [Tooltip("*Needs configurable Joint")] private Rigidbody ragdollNeck = null;
    [SerializeField] [Tooltip("*Needs configurable Joint")] private Rigidbody ragdollHead = null;
    [SerializeField] [Tooltip("*Needs configurable Joint")] private Rigidbody ragdollUpperLegLeft = null;
    [SerializeField] [Tooltip("*Needs configurable Joint")] private Rigidbody ragdollLowerLegLeft = null;
    [SerializeField] [Tooltip("*Needs configurable Joint")] private Rigidbody ragdollUpperLegRight = null;
    [SerializeField] [Tooltip("*Needs configurable Joint")] private Rigidbody ragdollLowerLegRight = null;
    [SerializeField] [Tooltip("*Needs configurable Joint")] private Rigidbody ragdollUpperArmLeft = null;
    [SerializeField] [Tooltip("*Needs configurable Joint")] private Rigidbody ragdollLowerArmLeft = null;
    [SerializeField] [Tooltip("*Needs configurable Joint")] private Rigidbody ragdollUpperArmRight = null;
    [SerializeField] [Tooltip("*Needs configurable Joint")] private Rigidbody ragdollLowerArmRight = null;

    [Header("Animated Armature Bones for Ragdoll")]
    [SerializeField] private Transform armatureHips = null;
    [SerializeField] private Transform armatureTorso = null;
    [SerializeField] private Transform armatureNeck = null;
    [SerializeField] private Transform armatureHead = null;
    [SerializeField] private Transform armatureUpperLegLeft = null;
    [SerializeField] private Transform armatureLowerLegLeft = null;
    [SerializeField] private Transform armatureUpperLegRight = null;
    [SerializeField] private Transform armatureLowerLegRight = null;
    [SerializeField] private Transform armatureUpperArmLeft = null;
    [SerializeField] private Transform armatureLowerArmLeft = null;
    [SerializeField] private Transform armatureUpperArmRight = null;
    [SerializeField] private Transform armatureLowerArmRight = null;

    
    private Quaternion armatureTorsoInitialRotation;
    private Quaternion armatureNeckInitialRotation;
    private Quaternion armatureHeadInitialRotation;
    private Quaternion armatureUpperLegLeftInitialRotation;
    private Quaternion armatureLowerLegLeftInitialRotation;
    private Quaternion armatureUpperLegRightInitialRotation;
    private Quaternion armatureLowerLegRightInitialRotation;
    private Quaternion armatureUpperArmLeftInitialRotation;
    private Quaternion armatureLowerArmLeftInitialRotation;
    private Quaternion armatureUpperArmRightInitialRotation;
    private Quaternion armatureLowerArmRightInitialRotation;
    

    private ConfigurableJoint ragdollTorsoJoint = null;
    private ConfigurableJoint ragdollNeckJoint = null;
    private ConfigurableJoint ragdollHeadJoint = null;
    private ConfigurableJoint ragdollUpperLegLeftJoint = null;
    private ConfigurableJoint ragdollLowerLegLeftJoint = null;
    private ConfigurableJoint ragdollUpperLegRightJoint = null;
    private ConfigurableJoint ragdollLowerLegRightJoint = null;
    private ConfigurableJoint ragdollUpperArmLeftJoint = null;
    private ConfigurableJoint ragdollLowerArmLeftJoint = null;
    private ConfigurableJoint ragdollUpperArmRightJoint = null;
    private ConfigurableJoint ragdollLowerArmRightJoint = null;

    private float ragdollTorsoJointLastSpringStrength;
    private float ragdollNeckJointLastSpringStrength;
    private float ragdollHeadJointLastSpringStrength;
    private float ragdollUpperLegLeftJointLastSpringStrength;
    private float ragdollLowerLegLeftJointLastSpringStrength;
    private float ragdollUpperLegRightJointLastSpringStrength;
    private float ragdollLowerLegRightJointLastSpringStrength;
    private float ragdollUpperArmLeftJointLastSpringStrength;
    private float ragdollLowerArmLeftJointLastSpringStrength;
    private float ragdollUpperArmRightJointLastSpringStrength;
    private float ragdollLowerArmRightJointLastSpringStrength;

    private float ragdollTorsoJointLastSpringDamp;
    private float ragdollNeckJointLastSpringDamp;
    private float ragdollHeadJointLastSpringDamp;
    private float ragdollUpperLegLeftJointLastSpringDamp;
    private float ragdollLowerLegLeftJointLastSpringDamp;
    private float ragdollUpperLegRightJointLastSpringDamp;
    private float ragdollLowerLegRightJointLastSpringDamp;
    private float ragdollUpperArmLeftJointLastSpringDamp;
    private float ragdollLowerArmLeftJointLastSpringDamp;
    private float ragdollUpperArmRightJointLastSpringDamp;
    private float ragdollLowerArmRightJointLastSpringDamp;


    // include hands, feet, and face bones
    private Transform[] ragdollOtherBones; 
    private Transform[] armatureOtherBones; 


    private bool initialized = false;

    // Start is called before the first frame update
    void Start()
    {
        if (!initialized) initialize();

    }

    public bool isInitialized() { return initialized; }
    public void initialize()
    {

        // initialize profiles
        if (!attachedProfile.initialized)
            attachedProfile.initialize();
        if (!detachedProfile.initialized)
            detachedProfile.initialize();

        // get ragdoll hand bones
        ragdollOtherBones =
            combineTransformArrays(
                removeFirstTransformFromArray(ragdollLowerArmLeft.GetComponentsInChildren<Transform>()),
                removeFirstTransformFromArray(ragdollLowerArmRight.GetComponentsInChildren<Transform>())
            );
        // get ragdoll feet bones
        ragdollOtherBones =
            combineTransformArrays(
                ragdollOtherBones,
                removeFirstTransformFromArray(ragdollLowerLegLeft.GetComponentsInChildren<Transform>())
            );
        ragdollOtherBones =
            combineTransformArrays(
                ragdollOtherBones,
                removeFirstTransformFromArray(ragdollLowerLegRight.GetComponentsInChildren<Transform>())
            );
        // get ragdoll face bones
        ragdollOtherBones =
            combineTransformArrays(
                ragdollOtherBones,
                removeFirstTransformFromArray(ragdollHead.GetComponentsInChildren<Transform>())
            );



        // get armature hand bones
        armatureOtherBones =
            combineTransformArrays(
                removeFirstTransformFromArray(armatureLowerArmLeft.GetComponentsInChildren<Transform>()),
                removeFirstTransformFromArray(armatureLowerArmRight.GetComponentsInChildren<Transform>())
            );
        // get armature feet bones
        armatureOtherBones =
            combineTransformArrays(
                armatureOtherBones,
                removeFirstTransformFromArray(armatureLowerLegLeft.GetComponentsInChildren<Transform>())
            );
        armatureOtherBones =
            combineTransformArrays(
                armatureOtherBones,
                removeFirstTransformFromArray(armatureLowerLegRight.GetComponentsInChildren<Transform>())
            );
        // get armature face bones
        armatureOtherBones =
            combineTransformArrays(
                armatureOtherBones,
                removeFirstTransformFromArray(armatureHead.GetComponentsInChildren<Transform>())
            );


        // get the initial ragdoll rotations
        armatureTorsoInitialRotation            = ragdollTorso.transform.localRotation;
        armatureNeckInitialRotation             = ragdollNeck.transform.localRotation;
        armatureHeadInitialRotation             = ragdollHead.transform.localRotation;

        armatureUpperLegLeftInitialRotation     = ragdollUpperLegLeft.transform.localRotation;
        armatureLowerLegLeftInitialRotation     = ragdollLowerLegLeft.transform.localRotation;
        armatureUpperLegRightInitialRotation    = ragdollUpperLegRight.transform.localRotation;
        armatureLowerLegRightInitialRotation    = ragdollLowerLegRight.transform.localRotation;

        armatureUpperArmLeftInitialRotation     = ragdollUpperArmLeft.transform.localRotation;
        armatureLowerArmLeftInitialRotation     = ragdollLowerArmLeft.transform.localRotation;
        armatureUpperArmRightInitialRotation    = ragdollUpperArmRight.transform.localRotation;
        armatureLowerArmRightInitialRotation    = ragdollLowerArmRight.transform.localRotation;


        ragdollTorsoJoint           = ragdollTorso.GetComponent<ConfigurableJoint>();
        ragdollNeckJoint            = ragdollNeck.GetComponent<ConfigurableJoint>();
        ragdollHeadJoint            = ragdollHead.GetComponent<ConfigurableJoint>();

        ragdollUpperLegLeftJoint    = ragdollUpperLegLeft.GetComponent<ConfigurableJoint>();
        ragdollLowerLegLeftJoint    = ragdollLowerLegLeft.GetComponent<ConfigurableJoint>();
        ragdollUpperLegRightJoint   = ragdollUpperLegRight.GetComponent<ConfigurableJoint>();
        ragdollLowerLegRightJoint   = ragdollLowerLegRight.GetComponent<ConfigurableJoint>();

        ragdollUpperArmLeftJoint    = ragdollUpperArmLeft.GetComponent<ConfigurableJoint>();
        ragdollLowerArmLeftJoint    = ragdollLowerArmLeft.GetComponent<ConfigurableJoint>();
        ragdollUpperArmRightJoint   = ragdollUpperArmRight.GetComponent<ConfigurableJoint>();
        ragdollLowerArmRightJoint   = ragdollLowerArmRight.GetComponent<ConfigurableJoint>();

        setAllJointMotionDefaults();

        initialized = true;
    }

    private void OnValidate()
    {
        if (!initialized) initialize();
        
        updateLockToArmature();
        updateCopyArmature();
    }

    void updateLockToArmature()
    {

        setRagdollUsingGravity(!lockToArmature);


        if (lockToArmature)
            setAllJointConnectedMass(attachedProfile);
        else
            setAllJointConnectedMass(detachedProfile);
    }

    void updateCopyArmature()
    {
        if (copyArmature)
        {
            setAllDriveSpringStrength(attachedProfile);
            setAllDriveSpringDampStrength(attachedProfile);
        }
        else
        {
            setAllDriveSpringStrength(detachedProfile);
            setAllDriveSpringDampStrength(detachedProfile);
        }
    }



    bool lastLockToArmature = false;

    // Update is called once per frame
    void LateUpdate()
    {
        if (lockToArmature)
        {

            ragdollHips.transform.position = armatureHips.position;
            ragdollHips.transform.rotation = armatureHips.rotation;

            if (hardLockTorso)
                ragdollNeck.transform.localPosition = armatureNeck.localPosition;
            if (hardLockLegs)
                ragdollTorso.transform.localPosition = armatureTorso.localPosition;

        }
        else
        {

            if (lastLockToArmature != lockToArmature)
            {
                
                ragdollNeck.velocity = new Vector3();
                ragdollHead.velocity = new Vector3();

                ragdollUpperLegLeft.velocity = new Vector3();
                ragdollLowerLegLeft.velocity = new Vector3();
                ragdollUpperLegRight.velocity = new Vector3();
                ragdollLowerLegRight.velocity = new Vector3();

                ragdollTorso.velocity = new Vector3();
                ragdollUpperArmLeft.velocity = new Vector3();
                ragdollLowerArmLeft.velocity = new Vector3();
                ragdollUpperArmRight.velocity = new Vector3();
                ragdollLowerArmRight.velocity = new Vector3();
                
            }

        }
        lastLockToArmature = lockToArmature;



        ragdollNeck.isKinematic = hardLockHead && lockToArmature;
        ragdollHead.isKinematic = hardLockHead && lockToArmature;

        ragdollUpperLegLeft.isKinematic = hardLockLegs && lockToArmature;
        ragdollLowerLegLeft.isKinematic = hardLockLegs && lockToArmature;
        ragdollUpperLegRight.isKinematic = hardLockLegs && lockToArmature;
        ragdollLowerLegRight.isKinematic = hardLockLegs && lockToArmature;

        ragdollTorso.isKinematic = hardLockTorso && lockToArmature;
        ragdollUpperArmLeft.isKinematic = hardLockTorso && lockToArmature;
        ragdollLowerArmLeft.isKinematic = hardLockTorso && lockToArmature;
        ragdollUpperArmRight.isKinematic = hardLockTorso && lockToArmature;
        ragdollLowerArmRight.isKinematic = hardLockTorso && lockToArmature;


        if (copyArmature)
        {

            if (hardLockHead)
            {
                ragdollNeck.transform.localRotation = armatureNeck.localRotation;
                ragdollHead.transform.localRotation = armatureHead.localRotation;
            }
            else
            {
                ragdollNeckJoint.targetRotation = Quaternion.Inverse(armatureNeck.localRotation) * armatureNeckInitialRotation;
                ragdollHeadJoint.targetRotation = Quaternion.Inverse(armatureHead.localRotation) * armatureHeadInitialRotation;
            }


            if (hardLockLegs)
            {
                
                ragdollUpperLegLeft.transform.localRotation     = armatureUpperLegLeft.localRotation;
                ragdollLowerLegLeft.transform.localRotation     = armatureLowerLegLeft.localRotation;
                ragdollUpperLegRight.transform.localRotation    = armatureUpperLegRight.localRotation;
                ragdollLowerLegRight.transform.localRotation    = armatureLowerLegRight.localRotation;
            }
            else
            {
                ragdollUpperLegLeftJoint.targetRotation = Quaternion.Inverse(armatureUpperLegLeft.localRotation) * armatureUpperLegLeftInitialRotation;
                ragdollLowerLegLeftJoint.targetRotation = Quaternion.Inverse(armatureLowerLegLeft.localRotation) * armatureLowerLegLeftInitialRotation;
                ragdollUpperLegRightJoint.targetRotation = Quaternion.Inverse(armatureUpperLegRight.localRotation) * armatureUpperLegRightInitialRotation;
                ragdollLowerLegRightJoint.targetRotation = Quaternion.Inverse(armatureLowerLegRight.localRotation) * armatureLowerLegRightInitialRotation;
            }



            if (hardLockTorso)
            {
                ragdollTorso.transform.localRotation = armatureTorso.localRotation;
                ragdollUpperArmLeft.transform.localRotation     = armatureUpperArmLeft.localRotation;
                ragdollLowerArmLeft.transform.localRotation     = armatureLowerArmLeft.localRotation;
                ragdollUpperArmRight.transform.localRotation    = armatureUpperArmRight.localRotation;
                ragdollLowerArmRight.transform.localRotation    = armatureLowerArmRight.localRotation;
            }
            else
            {
                ragdollTorsoJoint.targetRotation = Quaternion.Inverse(armatureTorso.localRotation) * armatureTorsoInitialRotation;
                ragdollUpperArmLeftJoint.targetRotation = Quaternion.Inverse(armatureUpperArmLeft.localRotation) * armatureUpperArmLeftInitialRotation;
                ragdollLowerArmLeftJoint.targetRotation = Quaternion.Inverse(armatureLowerArmLeft.localRotation) * armatureLowerArmLeftInitialRotation;
                ragdollUpperArmRightJoint.targetRotation = Quaternion.Inverse(armatureUpperArmRight.localRotation) * armatureUpperArmRightInitialRotation;
                ragdollLowerArmRightJoint.targetRotation = Quaternion.Inverse(armatureLowerArmRight.localRotation) * armatureLowerArmRightInitialRotation;
            }


            for (int i = 0; i < armatureOtherBones.Length; i++)
                ragdollOtherBones[i].localRotation = armatureOtherBones[i].localRotation;

        }
        


    }





    void setRagdollUsingGravity(bool b)
    {
        ragdollHips.useGravity = b;
        ragdollNeck.useGravity = b;
        ragdollHead.useGravity = b;

        ragdollUpperLegLeft.useGravity = b;
        ragdollLowerLegLeft.useGravity = b;
        ragdollUpperLegRight.useGravity = b;
        ragdollLowerLegRight.useGravity = b;

        ragdollTorso.useGravity = b;
        ragdollUpperArmLeft.useGravity = b;
        ragdollLowerArmLeft.useGravity = b;
        ragdollUpperArmRight.useGravity = b;
        ragdollLowerArmRight.useGravity = b;
    }

    
    void setAllJointMotionDefaults()
    {
        setJointMotionDefaults(ragdollTorsoJoint);
        setJointMotionDefaults(ragdollNeckJoint);
        setJointMotionDefaults(ragdollHeadJoint);

        setJointMotionDefaults(ragdollUpperLegLeftJoint);
        setJointMotionDefaults(ragdollLowerLegLeftJoint);
        setJointMotionDefaults(ragdollUpperLegRightJoint);
        setJointMotionDefaults(ragdollLowerLegRightJoint);

        setJointMotionDefaults(ragdollUpperArmLeftJoint);
        setJointMotionDefaults(ragdollLowerArmLeftJoint);
        setJointMotionDefaults(ragdollUpperArmRightJoint);
        setJointMotionDefaults(ragdollLowerArmRightJoint);
    }

    void setAllDriveSpringStrength( AnimatedRagdollProfile profile )
    {
        setDriveSpringStrength(ragdollTorsoJoint,           profile.torsoJointSpringStrength);
        setDriveSpringStrength(ragdollNeckJoint,            profile.neckJointSpringStrength);
        setDriveSpringStrength(ragdollHeadJoint,            profile.headJointSpringStrength);

        setDriveSpringStrength(ragdollUpperLegLeftJoint,    profile.upperLegLeftJointSpringStrength);
        setDriveSpringStrength(ragdollLowerLegLeftJoint,    profile.lowerLegLeftJointSpringStrength);
        setDriveSpringStrength(ragdollUpperLegRightJoint,   profile.upperLegRightJointSpringStrength);
        setDriveSpringStrength(ragdollLowerLegRightJoint,   profile.lowerLegRightJointSpringStrength);

        setDriveSpringStrength(ragdollUpperArmLeftJoint,    profile.upperArmLeftJointSpringStrength);
        setDriveSpringStrength(ragdollLowerArmLeftJoint,    profile.lowerArmLeftJointSpringStrength);
        setDriveSpringStrength(ragdollUpperArmRightJoint,   profile.upperArmRightJointSpringStrength);
        setDriveSpringStrength(ragdollLowerArmRightJoint,   profile.lowerArmRightJointSpringStrength);
    }

    void setAllDriveSpringDampStrength( AnimatedRagdollProfile profile )
    {

        setDriveSpringDampStrength(ragdollTorsoJoint,           profile.torsoJointSpringDamp);
        setDriveSpringDampStrength(ragdollNeckJoint,            profile.neckJointSpringDamp);
        setDriveSpringDampStrength(ragdollHeadJoint,            profile.headJointSpringDamp);

        setDriveSpringDampStrength(ragdollUpperLegLeftJoint,    profile.upperLegLeftJointSpringDamp);
        setDriveSpringDampStrength(ragdollLowerLegLeftJoint,    profile.lowerLegLeftJointSpringDamp);
        setDriveSpringDampStrength(ragdollUpperLegRightJoint,   profile.upperLegRightJointSpringDamp);
        setDriveSpringDampStrength(ragdollLowerLegRightJoint,   profile.lowerLegRightJointSpringDamp);

        setDriveSpringDampStrength(ragdollUpperArmLeftJoint,    profile.upperArmLeftJointSpringDamp);
        setDriveSpringDampStrength(ragdollLowerArmLeftJoint,    profile.lowerArmLeftJointSpringDamp);
        setDriveSpringDampStrength(ragdollUpperArmRightJoint,   profile.upperArmRightJointSpringDamp);
        setDriveSpringDampStrength(ragdollLowerArmRightJoint,   profile.lowerArmRightJointSpringDamp);

    }

    void setAllJointMass(AnimatedRagdollProfile profile)
    {

        ragdollTorsoJoint.massScale = profile.torsoJointMass;
        ragdollNeckJoint.massScale = profile.neckJointMass;
        ragdollHeadJoint.massScale = profile.headJointMass;

        ragdollUpperLegLeftJoint.massScale = profile.upperLegLeftJointMass;
        ragdollLowerLegLeftJoint.massScale = profile.lowerLegLeftJointMass;
        ragdollUpperLegRightJoint.massScale = profile.upperLegRightJointMass;
        ragdollLowerLegRightJoint.massScale = profile.lowerLegRightJointMass;

        ragdollUpperArmLeftJoint.massScale = profile.upperArmLeftJointMass;
        ragdollLowerArmLeftJoint.massScale = profile.lowerArmLeftJointMass;
        ragdollUpperArmRightJoint.massScale = profile.upperArmRightJointMass;
        ragdollLowerArmRightJoint.massScale = profile.lowerArmRightJointMass;
    }

    void setAllJointConnectedMass(AnimatedRagdollProfile profile)
    {

        ragdollTorsoJoint.connectedMassScale = profile.torsoJointConnectedMass;
        ragdollNeckJoint.connectedMassScale = profile.neckJointConnectedMass;
        ragdollHeadJoint.connectedMassScale = profile.headJointConnectedMass;

        ragdollUpperLegLeftJoint.connectedMassScale = profile.upperLegLeftJointConnectedMass;
        ragdollLowerLegLeftJoint.connectedMassScale = profile.lowerLegLeftJointConnectedMass;
        ragdollUpperLegRightJoint.connectedMassScale = profile.upperLegRightJointConnectedMass;
        ragdollLowerLegRightJoint.connectedMassScale = profile.lowerLegRightJointConnectedMass;

        ragdollUpperArmLeftJoint.connectedMassScale = profile.upperArmLeftJointConnectedMass;
        ragdollLowerArmLeftJoint.connectedMassScale = profile.lowerArmLeftJointConnectedMass;
        ragdollUpperArmRightJoint.connectedMassScale = profile.upperArmRightJointConnectedMass;
        ragdollLowerArmRightJoint.connectedMassScale = profile.lowerArmRightJointConnectedMass;
    }

    void setJointMotionDefaults(ConfigurableJoint joint)
    {
        joint.rotationDriveMode = RotationDriveMode.Slerp;

        joint.xMotion = ConfigurableJointMotion.Locked;
        joint.yMotion = ConfigurableJointMotion.Locked;
        joint.zMotion = ConfigurableJointMotion.Locked;

        joint.angularXMotion = ConfigurableJointMotion.Free;
        joint.angularYMotion = ConfigurableJointMotion.Free;
        joint.angularZMotion = ConfigurableJointMotion.Free;
    }

    void setDriveSpringStrength(ConfigurableJoint joint, float strength)
    {
        JointDrive drive = joint.slerpDrive;
        drive.positionSpring = strength;
        joint.slerpDrive = drive;
    }

    void setDriveSpringDampStrength(ConfigurableJoint joint, float damp)
    {
        JointDrive drive = joint.slerpDrive;
        drive.positionDamper = damp;
        joint.slerpDrive = drive;
    }



    // Array Helper functions
    Transform[] combineTransformArrays(Transform[] A, Transform[] B)
    {
        Transform[] temp = new Transform[A.Length + B.Length];
        A.CopyTo(temp, 0);
        B.CopyTo(temp, A.Length);
        return temp;
    }

    Transform[] removeFirstTransformFromArray(Transform[] T)
    {
        List<Transform> A = new List<Transform>(T);
        A.RemoveAt(0);
        return A.ToArray();
    }

    public void setLockToArmature(bool b)
    {
        lockToArmature = b;
        updateLockToArmature();
    }
    public void setCopyArmature(bool b)
    {
        copyArmature = b;
        updateCopyArmature();
    }

    public bool isLockedToArmature() { return lockToArmature; }
    public bool copyingArmature() { return copyArmature; }

    public void setRagdollLayers( int layer)
    {
        ragdollHips.gameObject.layer = layer;
        ragdollNeck.gameObject.layer = layer;
        ragdollHead.gameObject.layer = layer;

        ragdollUpperLegLeft.gameObject.layer = layer;
        ragdollLowerLegLeft.gameObject.layer = layer;
        ragdollUpperLegRight.gameObject.layer = layer;
        ragdollLowerLegRight.gameObject.layer = layer;

        ragdollTorso.gameObject.layer = layer;
        ragdollUpperArmLeft.gameObject.layer = layer;
        ragdollLowerArmLeft.gameObject.layer = layer;
        ragdollUpperArmRight.gameObject.layer = layer;
        ragdollLowerArmRight.gameObject.layer = layer;
    }


}
