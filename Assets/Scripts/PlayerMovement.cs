using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
 
    public float speed = 8f;
    public float gravity = 9.81f;
    public float jumpHeight = 4.9f;
 
    public Transform groundCheck, ceilingCheck;
    public float groundDistance = 0.4f, ceilingDistance = 0.2f;
    public LayerMask groundMask, ceilingMask;
    float time = 0f;
    public float FPS = 60f;
    bool Jumping = false;
    
 
    Vector3 velocity;

    public bool isCeiling;
    bool isGrounded;
    
    void Start() 
    {
        QualitySettings.vSyncCount = 0;   // 把垂直同步關掉
        Application.targetFrameRate = (int)FPS;
    }

    // Update is called once per frame
    void Update()
    {
        //Physics.CheckSphere()函式用以偵測給定位置周圍是否有碰撞器。 重點:groundMask負責檢測哪一個層級(Layer)的碰撞器才會被偵測。
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //抓取水平和垂直軸的移動比例(-1 <= x,z <= 1)
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
 
        //transform.right、transform.forward表示x與z軸，與x、z相乘後代表當前的位移比例。
        Vector3 move = transform.right * x + transform.forward * z;

        //利用CharacterController的內建函式來處理角色移動(位移比例*移動速度*幀數修正)。
        controller.Move(move * speed * 1 / FPS);
 
        //check if the player is on the ground so he can jump
        
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            //the equation for jumping
            time = 60;
            Jumping = true;
        }

        
        
        if(Jumping)
        {
            velocity.y = ((FPS - time + 1f/2f) * gravity) / (FPS * FPS);
            velocity.y -= ((time - 1f/2f) * gravity) / (FPS * FPS); //每一次update使玩家移動此距離
            controller.Move(velocity);
            time--;

            if(!isCeiling) //偵測是否頂到天花板
                isCeiling = Physics.CheckSphere(ceilingCheck.position, ceilingDistance, ceilingMask);
            else if(isGrounded) //若頂到天花板則只有再次觸碰地面會使跳躍重置
                isCeiling = Physics.CheckSphere(ceilingCheck.position, ceilingDistance, ceilingMask);

            if(time == 0 || isCeiling)
            {
                if(isCeiling) //如果是頂到天花板則使玩家做自由落體，計算掉落距離。
                    time = 0;
                else
                    time = FPS - 1; //完整的跳躍結束後若尚未接觸到地面則使time回到最高速的狀態。

                Jumping = false;
            }
        }
        else if(!Jumping)
        {
            velocity.y = ((time + 1f/2f) * gravity) / (FPS * FPS); //每一次update使玩家移動此距離
            controller.Move(velocity);
            time++;

            if(time >= FPS - 1 && isGrounded) //如果已經接觸地面則重置當前的time
            {
                time = 0;
            }
        }
    }
}
