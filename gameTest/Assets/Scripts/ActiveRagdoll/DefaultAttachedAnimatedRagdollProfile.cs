using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultAttachedAnimatedRagdollProfile : AnimatedRagdollProfile
{

    public override void initialize()
    {
        torsoJointSpringStrength = 1000;
        neckJointSpringStrength = 1000;
        headJointSpringStrength = 1000;
        upperLegLeftJointSpringStrength = 1000;
        lowerLegLeftJointSpringStrength = 1000;
        upperLegRightJointSpringStrength = 1000;
        lowerLegRightJointSpringStrength = 1000;
        upperArmLeftJointSpringStrength = 1000;
        lowerArmLeftJointSpringStrength = 1000;
        upperArmRightJointSpringStrength = 1000;
        lowerArmRightJointSpringStrength = 1000;

        torsoJointSpringDamp = 50;
        neckJointSpringDamp = 50;
        headJointSpringDamp = 50;
        upperLegLeftJointSpringDamp = 50;
        lowerLegLeftJointSpringDamp = 50;
        upperLegRightJointSpringDamp = 50;
        lowerLegRightJointSpringDamp = 50f;
        upperArmLeftJointSpringDamp = 50;
        lowerArmLeftJointSpringDamp = 50;
        upperArmRightJointSpringDamp = 50;
        lowerArmRightJointSpringDamp = 50;

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

        torsoJointConnectedMass = 0f;
        neckJointConnectedMass = 0f;
        headJointConnectedMass = 0.1f;
        upperLegLeftJointConnectedMass = 0f;
        lowerLegLeftJointConnectedMass = 0f;
        upperLegRightJointConnectedMass = 0f;
        lowerLegRightJointConnectedMass = 0f;
        upperArmLeftJointConnectedMass = 0f;
        lowerArmLeftJointConnectedMass = 0f;
        upperArmRightJointConnectedMass = 0f;
        lowerArmRightJointConnectedMass = 0f;

        initialized = true;

    }

}
