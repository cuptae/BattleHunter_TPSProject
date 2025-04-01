using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    private Vector3 clampAngle;
    private float rotX;
    private float rotY;
    public float sensitivity = 10f;
    public float normalDist;
    public float minDist;
    public float attackDist;

    private float finalDist;

    public float height;
    public float followSpeed = 10f;
    public bool cusorVisible;

    public Transform target;
    public Vector3 dirOffSet;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        finalDist = normalDist;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None; // 마우스 해제
        }

        if (Cursor.visible && Input.GetMouseButtonDown(0)) // 마우스 클릭 시 숨기기
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked; // 화면 중앙 고정
        }
    }

    private void LateUpdate()
    {

        // 타겟 거리 설정
        float targetDistance = target.GetComponentInParent<PlayerCtrl>().isAttack ? attackDist : normalDist;
        finalDist = Mathf.Lerp(finalDist, targetDistance, 0.05f);

        // 마우스 회전 처리
        rotX -= Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        rotY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        rotX = Mathf.Clamp(rotX, -60f, 60f);

        // 회전 계산
        Quaternion rotation = Quaternion.Euler(rotX, rotY, 0);

        // 타겟 위치 계산 (height 적용)
        Vector3 targetPosition = target.position + Vector3.up * height;

        // 카메라 원래 위치 계산
        Vector3 desiredPosition = targetPosition + rotation * new Vector3(0, 0, -finalDist);

        // 장애물 감지 및 위치 보정
        RaycastHit hit;
        if (Physics.Linecast(targetPosition, desiredPosition, out hit))
        {
            // 장애물이 있으면 카메라를 장애물 앞쪽으로 이동
            transform.position = hit.point + hit.normal * 0.3f;
        }
        else
        {
            // 장애물이 없으면 원래 위치 사용
            transform.position = desiredPosition;
        }

        // 카메라 회전 적용
        Vector3 dir = target.position - transform.position;
        transform.rotation = Quaternion.LookRotation(dir + dirOffSet);
    }
    void OnDrawGizmos()
    {
         if (target == null) return;

    // 타겟 위치 계산 (height 적용)
    Vector3 targetPosition = target.position + Vector3.up * height;

    // 카메라 원래 위치 계산
    Quaternion rotation = Quaternion.Euler(rotX, rotY, 0);
    Vector3 desiredPosition = targetPosition + rotation * new Vector3(0, 0, -finalDist);

    // 장애물 감지
    RaycastHit hit;
    // 원래 카메라 위치 (초록색)
    Gizmos.color = Color.green;
    Gizmos.DrawSphere(desiredPosition, 0.1f);

    // 장애물 체크용 Ray (노란색)
    Gizmos.color = Color.yellow;
    Gizmos.DrawLine(targetPosition, desiredPosition);

    if (Physics.Linecast(targetPosition, desiredPosition, out hit))
    {
        // 충돌한 지점 (빨간색)
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(hit.point, 0.15f);
        
        // 장애물 표면 법선 벡터 (파란색)
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(hit.point, hit.point + hit.normal * 0.5f);
    }
    }
}
