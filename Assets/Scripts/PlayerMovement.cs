using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    float time = 0f;
    public float deltaTime, maxTime, CurrentSpeed;
 
    Vector3 velocity;

    float FPS, JumpHeight, Gravity;

    void Start() 
    {
        FPS = GetComponent<PlayerInformation>().GameFPS;
        JumpHeight = GetComponent<PlayerInformation>().PlayerJumpHeight;
        Gravity = GetComponent<PlayerInformation>().EnvirnomentGravity;
        CurrentSpeed = GetComponent<PlayerInformation>().PlayerWalkingSpeed;

        QualitySettings.vSyncCount = 0;   // 把垂直同步關掉
        Application.targetFrameRate = (int)FPS; //設定畫面幀數
        deltaTime = Mathf.Sqrt(2f * JumpHeight / Gravity); //根據跳躍高度、重力計算此跳躍需要t秒
        maxTime = FPS * deltaTime; //等比例計算t秒代表要跑幾次Update()函式
    }

    void Update()
    {
        GetComponent<PlayerInformation>().DetectGround();

        //抓取水平和垂直軸的移動比例(-1 <= x,z <= 1)
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
 
        //transform.right、transform.forward表示x與z軸，與x、z相乘後代表當前的位移比例。
        Vector3 move = transform.right * x + transform.forward * z;

        //利用CharacterController的內建函式來處理角色移動(位移比例*移動速度*幀數修正)。
        controller.Move(move * CurrentSpeed * 1 / FPS);
        
        //偵測是否在地面按下空白鍵(跳躍鍵)
        if (Input.GetKeyDown(KeyCode.Space) && GetComponent<PlayerInformation>().isGrounded)
        {
            time = (float)((int)maxTime + 1); //設定當前time值(用來計算跑幾次Update後結束跳躍)
            GetComponent<PlayerInformation>().PlayerJumpping = true; //跳躍開始
        }

        if(GetComponent<PlayerInformation>().PlayerJumpping)
        {
            velocity.y = (Gravity * deltaTime - (maxTime - time) * Gravity / maxTime) / maxTime - 1f / 2f * Gravity / maxTime / maxTime; //=號後方代表向上移動的距離
            controller.Move(velocity); //velocity代表每一次update使玩家移動此距離
            time--;

            if(!GetComponent<PlayerInformation>().isCeiling) //偵測是否頂到天花板
                GetComponent<PlayerInformation>().DetectCeiling();
            else if(GetComponent<PlayerInformation>().isGrounded) //若頂到天花板則只有再次觸碰地面會使跳躍重置
                GetComponent<PlayerInformation>().DetectCeiling();

            if(time == 0 || GetComponent<PlayerInformation>().isCeiling)
            {
                GetComponent<PlayerInformation>().PlayerJumpping = false;
            }
        }
        else if(!GetComponent<PlayerInformation>().PlayerJumpping)
        {
            velocity.y = ((time + 1f/2f) * Gravity) / (maxTime * maxTime); //每一次update使玩家移動此距離
            controller.Move(-velocity);
            time++;

            if(GetComponent<PlayerInformation>().isGrounded) //如果已經接觸地面則重置當前的time，否則持續掉落
            {
                time = 0;
            }
        }
    }
}
