using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject Player;
    CharacterController controller;
    Vector3 moveDirection;
    public float moveSpeed = 5f;
    
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        moveDirection = new Vector3(horizontalInput, 0f, verticalInput);
        moveDirection.Normalize(); // 归一化，确保移动速度一致

        moveDirection *= moveSpeed;

        controller.Move(moveDirection * Time.deltaTime);
    }
}
