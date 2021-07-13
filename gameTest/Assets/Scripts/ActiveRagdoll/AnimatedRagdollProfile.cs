using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedRagdollProfile : MonoBehaviour
{

    public bool initialized = false;

    public float torsoJointSpringStrength;
    public float neckJointSpringStrength;
    public float headJointSpringStrength;
    public float upperLegLeftJointSpringStrength;
    public float lowerLegLeftJointSpringStrength;
    public float upperLegRightJointSpringStrength;
    public float lowerLegRightJointSpringStrength;
    public float upperArmLeftJointSpringStrength;
    public float lowerArmLeftJointSpringStrength;
    public float upperArmRightJointSpringStrength;
    public float lowerArmRightJointSpringStrength;

    public float torsoJointSpringDamp;
    public float neckJointSpringDamp;
    public float headJointSpringDamp;
    public float upperLegLeftJointSpringDamp;
    public float lowerLegLeftJointSpringDamp;
    public float upperLegRightJointSpringDamp;
    public float lowerLegRightJointSpringDamp;
    public float upperArmLeftJointSpringDamp;
    public float lowerArmLeftJointSpringDamp;
    public float upperArmRightJointSpringDamp;
    public float lowerArmRightJointSpringDamp;

    public float torsoJointMass;
    public float neckJointMass;
    public float headJointMass;
    public float upperLegLeftJointMass;
    public float lowerLegLeftJointMass;
    public float upperLegRightJointMass;
    public float lowerLegRightJointMass;
    public float upperArmLeftJointMass;
    public float lowerArmLeftJointMass;
    public float upperArmRightJointMass;
    public float lowerArmRightJointMass;

    public float torsoJointConnectedMass;
    public float neckJointConnectedMass;
    public float headJointConnectedMass;
    public float upperLegLeftJointConnectedMass;
    public float lowerLegLeftJointConnectedMass;
    public float upperLegRightJointConnectedMass;
    public float lowerLegRightJointConnectedMass;
    public float upperArmLeftJointConnectedMass;
    public float lowerArmLeftJointConnectedMass;
    public float upperArmRightJointConnectedMass;
    public float lowerArmRightJointConnectedMass;

    public virtual void initialize() { }

}
