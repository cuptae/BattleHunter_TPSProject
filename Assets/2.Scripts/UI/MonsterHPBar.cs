using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHPBar : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider hpSlider; // HP 바 슬라이더
    public Text enemyNameText; // 몬스터 이름 표시 UI
    public Transform enemyTransform; // HP 바가 따라다닐 몬스터
    public CanvasGroup hpBarCanvasGroup; // HP 바 투명도 조절

    [Header("Visibility Settings")]
    public float hideDistance = 15f; // 플레이어와 일정 거리 이상이면 숨김
    public float smoothSpeed = 0.1f; // HP 바 변화 속도

    [Header("Monster HP Reference")]
    public Monster monster; // 몬스터의 체력 정보를 가져오기 위한 참조

    [Header("Reference")]
    public Camera mainCamera; // 메인 카메라 참조
    private Transform playerTransform; // 플레이어의 위치

    public Vector3 offset = new Vector3(0, 2f, 0); // HP 바 위치 보정

    void Awake()
    {
        // 부모 객체에서 Monster 컴포넌트를 찾음
        if (monster == null)
            monster = GetComponentInParent<Monster>(); 

        // 메인 카메라 찾기
        if (mainCamera == null)
            mainCamera = Camera.main;

        // 플레이어 찾기 (태그 기반)
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerTransform = player.transform;

        // HP 바 처음에는 숨김 상태
        if (hpBarCanvasGroup != null)
            hpBarCanvasGroup.alpha = 0;
    }

    void Update()
    {
        UpdateHPBarPosition(); // HP 바 위치 갱신
        CheckVisibility(); // HP 바 표시 여부 확인

        if (monster != null)
        {
            UpdateHPBarUI(); // HP 바 UI 업데이트
        }
    }

    /// <summary>
    /// 몬스터 체력에 따라 HP 바 UI를 업데이트하는 함수
    /// </summary>
    public void UpdateHPBarUI()
    {
        if (monster == null || hpSlider == null)
            return;

        float hpRatio = monster.currentHP / monster.maxHP; // 현재 체력 비율 계산

        StopAllCoroutines(); // 기존 코루틴 정지 후 새로운 코루틴 시작
        StartCoroutine(SmoothHPBarChange(hpRatio));

        // 체력이 최대치라면 HP 바 숨김
        if (hpBarCanvasGroup != null)
        {
            hpBarCanvasGroup.alpha = (monster.currentHP < monster.maxHP) ? 1 : 0;
        }
    }

    /// <summary>
    /// HP 바의 값이 부드럽게 변하도록 하는 코루틴
    /// </summary>
    private IEnumerator SmoothHPBarChange(float targetValue)
    {
        float currentValue = hpSlider.value;
        while (Mathf.Abs(currentValue - targetValue) > 0.01f)
        {
            currentValue = Mathf.Lerp(currentValue, targetValue, smoothSpeed);
            hpSlider.value = currentValue;
            yield return null;
        }

        hpSlider.value = targetValue;
    }

    /// <summary>
    /// HP 바 위치를 몬스터의 위치에 맞춰 업데이트하는 함수
    /// </summary>
    private void UpdateHPBarPosition()
    {
        if (enemyTransform == null || hpSlider == null) return;

        // HP 바를 몬스터의 월드 좌표 기준으로 이동
        transform.position = enemyTransform.position + offset;

        // HP 바가 항상 카메라를 바라보도록 설정
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, 
                         mainCamera.transform.rotation * Vector3.up);
    }

    /// <summary>
    /// 플레이어와의 거리에 따라 HP 바의 투명도를 조절하는 함수
    /// </summary>
    private void CheckVisibility()
    {
        if (playerTransform == null || hpBarCanvasGroup == null || enemyTransform == null)
            return;

        float distance = Vector3.Distance(playerTransform.position, enemyTransform.position);
        hpBarCanvasGroup.alpha = (distance <= hideDistance) ? 1 : 0; // 일정 거리 이내면 HP 바 표시
    }
}
