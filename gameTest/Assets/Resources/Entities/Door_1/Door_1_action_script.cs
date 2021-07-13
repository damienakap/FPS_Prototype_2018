using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_1_action_script : MonoBehaviour
{

    public static int playerLayer = 8;

    private float enteredTimeLast = 0f;

    public float closeTime = 5f;
    public Animator animator;
    public Collider block;
    public string key = null;
    public bool Locked = false;
    

    private void Awake()
    {
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (animator)
        {
            bool isOpen = animator.GetCurrentAnimatorStateInfo(0).IsName("Open");
            if (isOpen)
            {
                float timeDelta = Time.realtimeSinceStartup - enteredTimeLast;
                if (timeDelta < 0) { timeDelta = Time.realtimeSinceStartup; }

                if (timeDelta >= closeTime)
                {
                    animator.SetBool("Entered", false);
                    if (block) { block.enabled = true; }
                }
            }
        }
    }
    
    private void OnTriggerStay(Collider c)
    {
        if (c.tag != "Player") { return; }

        if ( animator ) { animator.SetBool("Entered", true); }

        if (block) { block.enabled = false; }
        enteredTimeLast = Time.realtimeSinceStartup;

    }

}
