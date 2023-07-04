using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator animator;

    float velocityX = 0f, velocityZ = 0f;
    public float acceleration = 1f;
    float jump = 0f;

    void Start()
    {
        animator = GetComponent<Animator>(); //抓取動畫控制
    }

    void Update()
    {
        if(Input.GetKey("w") && velocityX < 1f) //若按下w且動畫尚未完全撥放，則持續增加velocityX使動畫完全撥放
        {
            velocityX += acceleration * Time.deltaTime;
        }

        if(Input.GetKey("s") && velocityX > -1f) //若按下s且動畫尚未完全撥放，則持續減少velocityX使動畫完全撥放
        {
            velocityX -= acceleration * Time.deltaTime;
        }

        if(Input.GetKey("a") && velocityZ > -1f) //同上述理論
        {
            velocityZ -= acceleration * Time.deltaTime;
        }

        if(Input.GetKey("d") && velocityZ < 1f) //同上述理論
        {
            velocityZ += acceleration * Time.deltaTime;
        }

        if(!Input.GetKey("d") && !Input.GetKey("a")) //若無按壓a與d則瞬間停止動畫(以此達到放開左右即停止移動的視覺效果)
        {
            velocityZ = 0f;
        }

        if(!Input.GetKey("w") && !Input.GetKey("s")) //若無按壓w與s則瞬間停止動畫(以此達到放開前後即停止移動的視覺效果)
        {
            velocityX = 0f;
        }

        if(Input.GetKey("left ctrl") && Input.GetKey("w") && GetComponent<PlayerMovement>().isGrounded) //若按下左ctrl則撥放奔跑動畫並增加移動速度。
        {
            animator.SetFloat("Running", 1f);
            velocityX = 0f;
            velocityZ = 0f;
            GetComponent<PlayerMovement>().speed = 15f;
        }
        else if(Input.GetKeyUp("left ctrl") && GetComponent<PlayerMovement>().isGrounded) //若無按壓左ctrl則瞬間停止動畫(以此達到放開左ctrl即停止移動的視覺效果)
        {
            if(Input.GetKey("w")) //若放開左ctrl時仍持續按壓向前鍵則撥放完整走路動畫。
            {
                velocityX = 1f;
            }
                animator.SetFloat("Running", 0f);
                GetComponent<PlayerMovement>().speed = 8f;
        }

        if(GetComponent<PlayerMovement>().Jumping && jump < 1)
        {
            animator.SetFloat("Running", 0f);
            velocityX = 0;
            velocityZ = 0;
            jump += acceleration / GetComponent<PlayerMovement>().maxTime;
        }
        else if(jump >= 0)
        {
            jump -= acceleration / GetComponent<PlayerMovement>().maxTime;
        }
        else
        {
            jump = 0;
        }

        animator.SetFloat("Jumpping", jump);
        animator.SetFloat("Velocity X", velocityX);
        animator.SetFloat("Velocity Z", velocityZ);
    }
}
