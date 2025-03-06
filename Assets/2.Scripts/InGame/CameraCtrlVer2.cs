using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrlVer2 : MonoBehaviour
{
    private Vector3 clampAngle;
    private float rotX;
    private float rotY;
    public float sensitivity = 10f;
    public float normalDist;

    public float finalRot;

    private float finalDist;

    public float height;
    public float followSpeed = 10f;
    public bool cusorVisible;

    
    public Transform target;

    private float distanceVelocity;

    // Start is called before the first frame update
    void Start()
    {

        finalDist = normalDist;
    }

    private void LateUpdate() {

        if(cusorVisible)
        {
            return;
        }

        // 타겟 거리 설정
        float targetDistance = normalDist;

        // 부드럽게 거리 보간 (SmoothDamp 사용)
        //finalDist = Mathf.SmoothDamp(finalDist, targetDistance, ref distanceVelocity, 0.2f); // 0.2초 감속 시간

        finalDist = Mathf.Lerp(finalDist,targetDistance,0.05f);

    // 카메라 회전 처리
        
        //rotX -= Input.GetAxis("Mouse Y")* sensitivity * Time.deltaTime; 
        //rotY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -60f, 80f);
        // 회전 계산
        transform.rotation = Quaternion.Euler(finalRot, 0, 0);

        // 타겟 위치 계산 (height 적용)
        Vector3 targetPosition = target.position + Vector3.up * height;

        // 카메라 위치 계산 (구면 좌표계)
        Vector3 offset = new Vector3(0, 0, -finalDist);

        // 위치 및 회전 적용
        transform.position = target.position+offset+height*Vector3.up;
        //transform.LookAt(targetPosition);   
    }
}
