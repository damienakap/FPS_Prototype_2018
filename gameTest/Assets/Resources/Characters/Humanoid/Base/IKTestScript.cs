using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKTestScript : MonoBehaviour
{

    Animator animator;

    public Transform IkTargetLeft;

    private void OnAnimatorIK(int layerIndex)
    {
        if (IkTargetLeft)
        {
            Debug.Log("IK");
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, IkTargetLeft.position);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
