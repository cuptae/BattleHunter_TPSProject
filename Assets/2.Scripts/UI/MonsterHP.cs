using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MonsterHP : MonoBehaviour
{
    public Transform monster; // 몬스터 Transform
    public Vector3 offset = new Vector3(0, 2f, 0); // HP 바 위치 조정
    public float disappearTime = 3f; // HP 바가 사라지는 시간

    private Slider hpSlider;
    private Camera myCam; // 로컬 플레이어의 카메라
    private Coroutine hideCoroutine;

    private void Start()
    {
        hpSlider = GetComponentInChildren<Slider>();
        myCam = Camera.main; // 로컬 플레이어의 카메라를 가져옴
        gameObject.SetActive(false); // 처음에는 비활성화
    }

    private void Update()
    {
        if (monster == null || myCam == null)
            return;

        // 월드 좌표를 UI 좌표로 변환
        Vector3 screenPos = myCam.WorldToScreenPoint(monster.position + offset);

        // 몬스터가 화면 앞에 있을 때만 HP 바 표시
        if (screenPos.z > 0)
        {
            transform.position = screenPos;
            transform.rotation = Quaternion.LookRotation(transform.position - myCam.transform.position);
        }
    }

    // HP 업데이트 및 HP 바 활성화
    public void ShowHP(float currentHP, float maxHP)
    {
        hpSlider.value = currentHP / maxHP;
        gameObject.SetActive(true);

        // 기존 코루틴이 실행 중이면 중지
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }
        // 일정 시간이 지나면 HP 바 숨김
        hideCoroutine = StartCoroutine(HideAfterDelay());
    }

    // 일정 시간 후 HP 바 숨기기
    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(disappearTime);
        gameObject.SetActive(false);
    }
}
