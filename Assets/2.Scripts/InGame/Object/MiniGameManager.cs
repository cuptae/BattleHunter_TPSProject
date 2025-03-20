using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour
{
    public GameObject miniGameUI;
    public Image progressBar;  // 점수 게이지 UI
    public float score = 0;
    public float maxScore = 100;
    public float needleSpeed = 200f;  // 바늘 속도
    public float minHitAngle = 240f;  // 적중 범위 시작
    public float maxHitAngle = 300f;  // 적중 범위 끝
    public Transform needle;  // 바늘 오브젝트
    private bool isMoving = true;  // 바늘이 움직이고 있는지 여부
    private bool isIncreasing = true;  // 바늘이 증가 방향인지 감소 방향인지

    public Button stopButton;  // 버튼 추가

    void Start()
    {
        miniGameUI.SetActive(false);  // 처음에는 미니게임 UI 비활성화
        stopButton.onClick.AddListener(StopNeedle);  // 버튼 클릭 이벤트 추가
    }

    void Update()
    {
        if (miniGameUI.activeSelf && isMoving)
        {
            MoveNeedle();  // 바늘 움직임 처리
        }
    }

    void MoveNeedle()
    {
        float rotationSpeed = needleSpeed * Time.deltaTime;

        if (isIncreasing)
        {
            needle.Rotate(0, 0, -rotationSpeed);
            if (needle.eulerAngles.z <= minHitAngle)
            {
                isIncreasing = false;
            }
        }
        else
        {
            needle.Rotate(0, 0, rotationSpeed);
            if (needle.eulerAngles.z >= maxHitAngle)
            {
                isIncreasing = true;
            }
        }
    }

    void StopNeedle()
    {
        if (!isMoving) return;  // 이미 멈춘 상태라면 실행하지 않음
        isMoving = false;  // 바늘 멈춤
        CheckHit();  // 멈춘 위치 확인
    }

    void CheckHit()
    {
        float currentAngle = needle.eulerAngles.z;

        if (currentAngle >= minHitAngle && currentAngle <= maxHitAngle)
        {
            score += 10;  // 점수 증가
            progressBar.fillAmount = score / maxScore;  // UI 업데이트

            if (score >= maxScore)
            {
                CompleteMiniGame();
            }
        }
        else
        {
            Debug.Log("Miss! 점수 증가 없음");  // 범위 밖에 멈추면 아무 일도 안 일어남
        }
    }

    void CompleteMiniGame()
    {
        Debug.Log("미니게임 완료!");
        miniGameUI.SetActive(false);
        FindObjectOfType<AutoDoor>().CheckDoorStatus();
    }

    public void StartMiniGame()
    {
        miniGameUI.SetActive(true);
        isMoving = true;  // 게임 시작 시 바늘이 다시 움직이도록 설정
    }
}
