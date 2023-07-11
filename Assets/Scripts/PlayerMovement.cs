using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    float time = 0f;
    public float deltaTime, maxTime, CurrentSpeed;

    Vector3 velocity;

    float FPS, Gravity, JumpHeight, JumpSpeed;

    void Start()
    {
        FPS = GetComponent<PlayerInformation>().GameFPS;
        Gravity = GetComponent<PlayerInformation>().EnvirnomentGravity;
        CurrentSpeed = GetComponent<PlayerInformation>().PlayerWalkingSpeed;
        JumpHeight = GetComponent<PlayerInformation>().PlayerJumpHeight;

        QualitySettings.vSyncCount = 0;   // 把垂直同步關掉
        Application.targetFrameRate = (int)FPS; //設定畫面幀數

        float n = Mathf.Sqrt(2 * JumpHeight / Gravity) / Time.deltaTime;
        JumpSpeed = ((n + 1f / 2f) * Gravity * Time.deltaTime);
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
            velocity.y = JumpSpeed;
            GetComponent<PlayerInformation>().PlayerJumpping = true; //跳躍開始
        }

        velocity.y -= (Gravity * Time.deltaTime);
        controller.Move(velocity * Time.deltaTime); //velocity代表每一次update使玩家移動此距離

        if(GetComponent<PlayerInformation>().PlayerJumpping)
            if(velocity.y < 0 && GetComponent<PlayerInformation>().isGrounded)
                GetComponent<PlayerInformation>().PlayerJumpping = false;
    }
}
