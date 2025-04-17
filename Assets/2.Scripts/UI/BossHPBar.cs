using UnityEngine;
using UnityEngine.UI;

public class BossHPBar : MonoBehaviour
{
    public Text bossNameText;
    public Slider hpSlider;
    public CanvasGroup canvasGroup;

    private float smoothSpeed = 5f;

    private float targetValue = 1f;

    void Start()
    {
        SetVisible(false);
    }

    void Update()
    {
        if (hpSlider != null)
        {
            hpSlider.value = Mathf.Lerp(hpSlider.value, targetValue, Time.deltaTime * smoothSpeed);
        }
    }

    public void SetBossName(string name)
    {
        bossNameText.text = name;
    }

    public void SetHP(float ratio)
    {
        targetValue = ratio;
    }

    public void SetVisible(bool visible)
    {
        canvasGroup.alpha = visible ? 1 : 0;
    }
}
