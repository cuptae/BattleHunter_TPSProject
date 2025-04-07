using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHPBar : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider hpSlider;
    public Text enemyNameText;
    public Transform enemyTransform;
    public CanvasGroup hpBarCanvasGroup;

    [Header("Visibility Settings")]
    public float hideDistance = 15f;
    public float smoothSpeed = 0.1f;

    [Header("Monster HP Reference")]
    public EnemyCtrl enemyCtrl;

    [Header("Reference")]
    public Camera mainCamera;
    private Transform playerTransform;

    public Vector3 offset = new Vector3(0, 2f, 0); // HP 바 위치 보정

    void Awake()
{
    if (enemyCtrl == null)
        enemyCtrl = GetComponentInParent<EnemyCtrl>();

    if (enemyTransform == null && enemyCtrl != null)
        enemyTransform = enemyCtrl.transform; // ✅ 자동으로 몬스터 Transform 할당

    if (mainCamera == null)
        mainCamera = Camera.main;

    GameObject player = GameObject.FindGameObjectWithTag("Player");
    if (player != null)
        playerTransform = player.transform;

    if (hpBarCanvasGroup != null)
        hpBarCanvasGroup.alpha = 0;
}

    void Update()
    {
        if (enemyCtrl == null || enemyCtrl.isDead)
        {
            if (hpBarCanvasGroup != null)
                hpBarCanvasGroup.alpha = 0;
            return;
        }
        
        UpdateHPBarPosition();
        CheckVisibility();
        UpdateHPBarUI();
    }

    public void UpdateHPBarUI()
{
    if (enemyCtrl == null || hpSlider == null)
    {
        Debug.LogWarning("❌ 체력바 참조가 잘못되었음!");
        return;
    }

    Debug.Log("✅ 체력바 UI 업데이트 중");

    float hpRatio = (float)enemyCtrl.curHp / enemyCtrl.maxHp;
    StopAllCoroutines();
    StartCoroutine(SmoothHPBarChange(hpRatio));

    if (hpBarCanvasGroup != null)
    {
        hpBarCanvasGroup.alpha = (enemyCtrl.curHp < enemyCtrl.maxHp) ? 1 : 0;
    }
}



    private IEnumerator SmoothHPBarChange(float targetValue)
    {
        float currentValue = hpSlider.value;
        while (Mathf.Abs(currentValue - targetValue) > 0.001f)
        {
            currentValue = Mathf.Lerp(currentValue, targetValue, smoothSpeed);
            hpSlider.value = currentValue;
            yield return null;
        }

        hpSlider.value = targetValue;
    }

    private void UpdateHPBarPosition()
{
    if (enemyTransform == null && enemyCtrl != null)
        enemyTransform = enemyCtrl.transform;

    if (enemyTransform == null || hpSlider == null) 
    {
        Debug.LogWarning("❌ HP 바 위치 업데이트 실패: enemyTransform이 없음!");
        return;
    }

    transform.position = enemyTransform.position + offset;
    transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
        mainCamera.transform.rotation * Vector3.up);

    Debug.Log($"📍 HP 바 위치 업데이트: {transform.position}");
}



    private void CheckVisibility()
    {
        if (playerTransform == null || hpBarCanvasGroup == null || enemyTransform == null)
            return;

        float distance = Vector3.Distance(playerTransform.position, enemyTransform.position);
        hpBarCanvasGroup.alpha = (distance <= hideDistance) ? 1 : 0;
    }

    public void HideHPBar()
{
    if (hpBarCanvasGroup != null)
    {
        hpBarCanvasGroup.alpha = 0;
    }
}

}
