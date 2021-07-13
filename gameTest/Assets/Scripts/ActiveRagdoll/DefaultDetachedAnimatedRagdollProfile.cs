using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultDetachedAnimatedRagdollProfile : AnimatedRagdollProfile
{

    public override void initialize()
    {
        torsoJointSpringStrength = 50;
        neckJointSpringStrength = 50;
        headJointSpringStrength = 50;
        upperLegLeftJointSpringStrength = 50;
        lowerLegLeftJointSpringStrength = 50;
        upperLegRightJointSpringStrength = 50;
        lowerLegRightJointSpringStrength = 50;
        upperArmLeftJointSpringStrength = 20;
        lowerArmLeftJointSpringStrength = 20;
        upperArmRightJointSpringStrength = 20;
        lowerArmRightJointSpringStrength = 20;

        torsoJointSpringDamp = 0;
        neckJointSpringDamp = 0;
        headJointSpringDamp = 0;
        upperLegLeftJointSpringDamp = 0;
        lowerLegLeftJointSpringDamp = 0;
        upperLegRightJointSpringDamp = 0;
        lowerLegRightJointSpringDamp = 0;
        upperArmLeftJointSpringDamp = 0;
        lowerArmLeftJointSpringDamp = 0;
        upperArmRightJointSpringDamp = 0;
        lowerArmRightJointSpringDamp = 0;

        torsoJointMass = 1f;
        neckJointMass = 1f;
        headJointMass = 1f;
        upperLegLeftJointMass = 1f;
        lowerLegLeftJointMass = 1f;
        upperLegRightJointMass = 1f;
        lowerLegRightJointMass = 1f;
        upperArmLeftJointMass = 1f;
        lowerArmLeftJointMass = 1f;
        upperArmRightJointMass = 1f;
        lowerArmRightJointMass = 1f;

        torsoJointConnectedMass = 1f;
        neckJointConnectedMass = 1f;
        headJointConnectedMass = 1f;
        upperLegLeftJointConnectedMass = 1f;
        lowerLegLeftJointConnectedMass = 1f;
        upperLegRightJointConnectedMass = 1f;
        lowerLegRightJointConnectedMass = 1f;
        upperArmLeftJointConnectedMass = 1f;
        lowerArmLeftJointConnectedMass = 1f;
        upperArmRightJointConnectedMass = 1f;
        lowerArmRightJointConnectedMass = 1f;

        initialized = true;

    }

}
