using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public float mouseSensitivity = 400f;

    float xRotation = 0f;
    public float YRotation = 0f;

    void Start()
    {
        //將滑鼠鎖在螢幕正中央並隱藏。
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //獲取滑鼠移動前後相差的值，介於-1到1之間。
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //視野往Z軸方向看過去，以x軸為水平線，旋轉角度大於零則Z軸向下看，反之，因此此處使用 "-="。
        xRotation -= mouseY;

        //限制x軸旋轉只能在-90至90之間
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //視野往Z軸方向看過去，以y軸為水平線，旋轉角度大於零則Z軸向右看，反之，因此此處使用 "+="。
        YRotation += mouseX;

        //避免YRotation無限增大或減小。
        if (YRotation >= 180)
            YRotation -= 360;
        else if (YRotation <= -180)
            YRotation += 360;

        //通過歐拉角來改變當前旋轉值。
        transform.rotation = Quaternion.Euler(xRotation, YRotation, 0f);
        GameObject.Find("Player").GetComponent<PlayerMovement>().transform.rotation = Quaternion.Euler(0f, YRotation, 0f);

        //重要經驗:localrotation會導致物件作相對於父物件的相對旋轉，rotation使物體作相對於整個世界的旋轉。
    }
}
