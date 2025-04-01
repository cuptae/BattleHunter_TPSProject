using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossHP : MonoBehaviour
{
    public Slider hpSlider; // HP 바 UI
    public GameObject bossHPUI; // 보스 HP 바 패널
    private static BossHP instance; // 싱글톤으로 관리

    private void Awake()
    {
        instance = this;
        bossHPUI.SetActive(false); // 처음에는 비활성화
    }

    // 보스 체력 업데이트
    public void UpdateBossHP(float currentHP, float maxHP)
    {
        if (!bossHPUI.activeSelf)
        {
            bossHPUI.SetActive(true); // 보스 등장 시 활성화
        }

        hpSlider.value = currentHP / maxHP;

        if (currentHP <= 0)
        {
            StartCoroutine(HideBossHP());
        }
    }

    // 일정 시간 후 보스 체력바 숨기기
    private IEnumerator HideBossHP()
    {
        yield return new WaitForSeconds(2f);
        bossHPUI.SetActive(false);
    }

    // 싱글턴 인스턴스 가져오기
    public static BossHP Instance
    {
        get { return instance; }
    }
}
