using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultDontCopyAnimatedRagdollProfile : AnimatedRagdollProfile
{

    public override void initialize()
    {
        torsoJointSpringStrength = 1000f;
        neckJointSpringStrength = 1000f;
        headJointSpringStrength = 1000f;
        upperLegLeftJointSpringStrength = 1000f;
        lowerLegLeftJointSpringStrength = 1000f;
        upperLegRightJointSpringStrength = 1000f;
        lowerLegRightJointSpringStrength = 1000f;
        upperArmLeftJointSpringStrength = 1000f;
        lowerArmLeftJointSpringStrength = 1000f;
        upperArmRightJointSpringStrength = 1000f;
        lowerArmRightJointSpringStrength = 1000f;

        torsoJointSpringDamp = 10f;
        neckJointSpringDamp = 10f;
        headJointSpringDamp = 10f;
        upperLegLeftJointSpringDamp = 10f;
        lowerLegLeftJointSpringDamp = 10f;
        upperLegRightJointSpringDamp = 10f;
        lowerLegRightJointSpringDamp = 10f;
        upperArmLeftJointSpringDamp = 10f;
        lowerArmLeftJointSpringDamp = 10f;
        upperArmRightJointSpringDamp = 10f;
        lowerArmRightJointSpringDamp = 10f;

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
