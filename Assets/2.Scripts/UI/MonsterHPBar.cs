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
    public float hideDistance = 25f;

    [Header("Monster HP Reference")]
    public EnemyCtrl enemyCtrl;

    [Header("Reference")]
    public Camera mainCamera;
    private Transform playerTransform;

    public Vector3 offset = new Vector3(0, 2f, 0);
    private Coroutine currentLerpCoroutine;

    void Awake()
    {
        if (enemyCtrl == null)
            enemyCtrl = GetComponentInParent<EnemyCtrl>();

        if (enemyTransform == null && enemyCtrl != null)
            enemyTransform = enemyCtrl.transform;

        if (mainCamera == null)
            mainCamera = Camera.main;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerTransform = player.transform;

        if (hpBarCanvasGroup == null)
        hpBarCanvasGroup = GetComponent<CanvasGroup>(); // 혹시 누락돼 있다면 자동 할당

        if (hpBarCanvasGroup != null)
            hpBarCanvasGroup.alpha = 0;  // 시작할 땐 안 보이게

    }

    void Update()
    {
        if (enemyCtrl == null || enemyCtrl.isDead)
        {
            HideHPBar();
            return;
        }

        UpdateHPBarPosition();
    }

    public void UpdateHPBarUI()
    {
        if (enemyCtrl == null || hpSlider == null)
            return;

        float hpRatio = (float)enemyCtrl.curHp / enemyCtrl.maxHp;

        if (currentLerpCoroutine != null)
            StopCoroutine(currentLerpCoroutine);
        currentLerpCoroutine = StartCoroutine(SmoothHPBarChange(hpRatio));

        // ✅ 체력이 줄어들면 보이고, 가득 차면 숨김
        if (hpBarCanvasGroup != null)
            hpBarCanvasGroup.alpha = (hpRatio < 1f) ? 1f : 0f;
    }

    private IEnumerator SmoothHPBarChange(float targetValue)
    {
        float currentValue = hpSlider.value;
        while (Mathf.Abs(currentValue - targetValue) > 0.001f)
        {
            currentValue = Mathf.Lerp(currentValue, targetValue, Time.deltaTime * 10f); // 속도 조절
            hpSlider.value = currentValue;
            yield return null;
        }

        hpSlider.value = targetValue;
    }

    private void UpdateHPBarPosition()
    {
        if (enemyTransform == null) return;

        transform.position = enemyTransform.position + offset;
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
            mainCamera.transform.rotation * Vector3.up);
    }

    private void CheckVisibility()
{
    if (playerTransform == null || hpBarCanvasGroup == null || enemyTransform == null)
        return;

    float distance = Vector3.Distance(playerTransform.position, enemyTransform.position);
    bool visible = (distance <= hideDistance);
    hpBarCanvasGroup.alpha = visible ? 1 : 0;

    Debug.Log($"[HPBar] 거리: {distance}, 보임: {visible}, 알파: {hpBarCanvasGroup.alpha}");
}



    public void HideHPBar()
    {
        if (hpBarCanvasGroup != null)
            hpBarCanvasGroup.alpha = 0f;
    }
}
