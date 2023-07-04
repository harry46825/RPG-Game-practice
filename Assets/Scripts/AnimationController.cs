using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            animator.SetBool("IsRunning", true);
        }

        if(Input.GetKeyDown("w"))
        {
            animator.SetBool("IsWalking", true);
        }
    }
}
