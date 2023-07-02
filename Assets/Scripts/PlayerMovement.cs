using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
 
    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
 
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
 
    Vector3 velocity;
 
    bool isGrounded;
 
    // Update is called once per frame
    void Update()
    {
        //Physics.CheckSphere()函式用以偵測給定位置周圍是否有碰撞器。 重點:groundMask負責檢測哪一個層級(Layer)的碰撞器才會被偵測。
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
 
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -5f;
        }

        //抓取水平和垂直軸的移動比例(-1 <= x,z <= 1)
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
 
        //transform.right、transform.forward表示x與z軸，與x、z相乘後代表當前的位移比例。
        Vector3 move = transform.right * x + transform.forward * z;

        //利用CharacterController的內建函式來處理角色移動(位移比例*移動速度*幀數修正)。
        controller.Move(move * speed * Time.deltaTime);
 
        //check if the player is on the ground so he can jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            //the equation for jumping
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
 
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
