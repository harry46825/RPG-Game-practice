using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInformation : MonoBehaviour
{
    public float PlayerWalkingSpeed = 3f;
    public float PlayerRunningSpeed = 5f;
    public float PlayerJumpHeight = 10f;
    public bool PlayerJumpping = false;
    public float PlayerHealth = 100f;

    public float EnvirnomentGravity = 9.81f;

    public float GameFPS = 60f;

    public Transform GroundDetectLeft, GroundDetectRight, GroundDetectForward, GroundDetectBackward, CeilingDetect;
    float GroundDetectDistance = 0.4f, CeilingDetectDistance = 0.2f;
    public LayerMask groundMask, ceilingMask;

    public bool isCeiling;
    public bool isGrounded;

    public void DetectGround()
    {
        //Physics.CheckSphere()函式用以偵測給定位置周圍是否有碰撞器。 重點:groundMask負責檢測哪一個層級(Layer)的碰撞器才會被偵測。
        if (Physics.CheckSphere(GroundDetectLeft.position, GroundDetectDistance, groundMask) || Physics.CheckSphere(GroundDetectRight.position, GroundDetectDistance, groundMask)
        || Physics.CheckSphere(GroundDetectForward.position, GroundDetectDistance, groundMask) || Physics.CheckSphere(GroundDetectBackward.position, GroundDetectDistance, groundMask))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;

        }

        return;
    }

    public void DetectCeiling()
    {
        isCeiling = Physics.CheckSphere(CeilingDetect.position, CeilingDetectDistance, ceilingMask);
    }
}
