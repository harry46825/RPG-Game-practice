using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator animator;

    float velocityX = 0f, velocityZ = 0f;
    public float acceleration = 1f;
    float jump = 0f, fall = 0f;

    void Start()
    {
        animator = GetComponent<Animator>(); //抓取動畫控制
    }

    void Update()
    {
        if (Input.GetKey("w") && velocityX < 1f && GetComponent<PlayerInformation>().isGrounded) //若按下w且動畫尚未完全撥放，則持續增加velocityX使動畫完全撥放
        {
            velocityX += acceleration * Time.deltaTime;
        }

        if (Input.GetKey("s") && velocityX > -1f && GetComponent<PlayerInformation>().isGrounded) //若按下s且動畫尚未完全撥放，則持續減少velocityX使動畫完全撥放
        {
            velocityX -= acceleration * Time.deltaTime;
        }

        if (Input.GetKey("a") && velocityZ > -1f && GetComponent<PlayerInformation>().isGrounded) //同上述理論
        {
            velocityZ -= acceleration * Time.deltaTime;
        }

        if (Input.GetKey("d") && velocityZ < 1f && GetComponent<PlayerInformation>().isGrounded) //同上述理論
        {
            velocityZ += acceleration * Time.deltaTime;

        }

        if (!Input.GetKey("d") && !Input.GetKey("a")) //若無按壓a與d則瞬間停止動畫(以此達到放開左右即停止移動的視覺效果)
        {
            velocityZ = 0f;
        }

        if (!Input.GetKey("w") && !Input.GetKey("s")) //若無按壓w與s則瞬間停止動畫(以此達到放開前後即停止移動的視覺效果)
        {
            velocityX = 0f;
        }

        if (Input.GetKey("left ctrl") && Input.GetKey("w") && GetComponent<PlayerInformation>().isGrounded && animator.GetFloat("Jumpping") == 0) //若按下左ctrl則撥放奔跑動畫並增加移動速度。
        {
            if (velocityX < 2f)
                velocityX += acceleration * Time.deltaTime;
            else
                velocityX = 2f;

            GetComponent<PlayerMovement>().CurrentSpeed = GetComponent<PlayerInformation>().PlayerRunningSpeed;
        }
        else if (Input.GetKeyUp("left ctrl")) //若無按壓左ctrl則瞬間停止動畫(以此達到放開左ctrl即停止移動的視覺效果)
        {
            if (Input.GetKey("w")) //若放開左ctrl時仍持續按壓向前鍵則撥放完整走路動畫。
            {
                velocityX = 1f;
            }
            GetComponent<PlayerMovement>().CurrentSpeed = GetComponent<PlayerInformation>().PlayerWalkingSpeed;
        }

        if (GetComponent<PlayerInformation>().PlayerJumpping)
        {
            velocityX = 0;
            velocityZ = 0;
            jump = 1;
        }
        else if (!GetComponent<PlayerInformation>().isGrounded)
        {
            fall += acceleration / GetComponent<PlayerMovement>().maxTime;
        }
        else
        {
            // jump = 0;
            if (jump > 0)
                jump -= acceleration / GetComponent<PlayerMovement>().maxTime;
            else
                jump = 0;

            if (fall > 0)
                fall -= acceleration / GetComponent<PlayerMovement>().maxTime;
            else
                fall = 0;
        }

        animator.SetFloat("Falling", fall);
        animator.SetFloat("Jumpping", jump);
        animator.SetFloat("Velocity X", velocityX);
        animator.SetFloat("Velocity Z", velocityZ);
    }
}
