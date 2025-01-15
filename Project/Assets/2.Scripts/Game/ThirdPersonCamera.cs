using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    private Vector3 clampAngle;
    private float rotX;
    private float rotY;
    public float sensitivity = 10f;
    public float distance= 10f;
    [Header("height만큼 y position을 올려야 하는 코드 짜야함 2정도 올리면 적당!")]
    public float height;
    public float followSpeed = 10f;
    public bool cusorVisible;

    
    public Transform target;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape))
            cusorVisible = true;
        else if(Input.GetMouseButtonDown(0))
            cusorVisible = false;
        
    }

    private void LateUpdate() {

        if(cusorVisible)
        {
            return;
        }
        rotX -= Input.GetAxis("Mouse Y")* sensitivity * Time.deltaTime; 
        rotY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -60f, 80f);
        // 회전 계산
        Quaternion rotation = Quaternion.Euler(rotX, rotY, 0);

        // 카메라 위치 계산 (구면 좌표계)
        Vector3 offset = rotation * new Vector3(0, 0, -distance);

        // 위치 및 회전 적용
        transform.position = target.position+offset+height*Vector3.up;
        transform.LookAt(target);   
    }
}
