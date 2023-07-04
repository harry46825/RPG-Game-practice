using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
 
    public float speed = 8f;
    public float gravity = 9.81f;
    public float jumpHeight = 10f;
 
    public Transform groundCheck, ceilingCheck;
    public float groundDistance = 0.4f, ceilingDistance = 0.2f;
    public LayerMask groundMask, ceilingMask;
    float time = 0f;
    public float FPS = 60f;
    public bool Jumping = false;
    public float deltaTime, maxTime;
 
    Vector3 velocity;

    public bool isCeiling;
    public bool isGrounded;
    
    void Start() 
    {
        QualitySettings.vSyncCount = 0;   // 把垂直同步關掉
        Application.targetFrameRate = (int)FPS; //設定畫面幀數
        deltaTime = Mathf.Sqrt(2f * jumpHeight / gravity); //根據跳躍高度、重力計算此跳躍需要t秒
        maxTime = FPS * deltaTime; //等比例計算t秒代表要跑幾次Update()函式    
    }

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
        
        //偵測是否在地面按下空白鍵(跳躍鍵)
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            time = (float)((int)maxTime + 1); //設定當前time值(用來計算跑幾次Update後結束跳躍)
            Jumping = true; //跳躍開始
        }

        if(Jumping)
        {
            velocity.y = (gravity * deltaTime - (maxTime - time) * gravity / maxTime) / maxTime - 1f / 2f * gravity / maxTime / maxTime; //=號後方代表向上移動的距離
            controller.Move(velocity); //velocity代表每一次update使玩家移動此距離
            time--;

            if(!isCeiling) //偵測是否頂到天花板
                isCeiling = Physics.CheckSphere(ceilingCheck.position, ceilingDistance, ceilingMask);
            else if(isGrounded) //若頂到天花板則只有再次觸碰地面會使跳躍重置
                isCeiling = Physics.CheckSphere(ceilingCheck.position, ceilingDistance, ceilingMask);

            if(time == 0 || isCeiling)
            {
                Jumping = false;
            }
        }
        else if(!Jumping)
        {
            velocity.y = ((time + 1f/2f) * gravity) / (maxTime * maxTime); //每一次update使玩家移動此距離
            controller.Move(-velocity);
            time++;

            if(isGrounded) //如果已經接觸地面則重置當前的time，否則持續掉落
            {
                time = 0;
            }
        }
    }
}
