using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Text buttonText; // 버튼 텍스트
    public Image leftImage; // 왼쪽 이미지
    public Image rightImage; // 오른쪽 이미지

    public float duration = 1f;

    private Color originalTextColor; // 원래 버튼 텍스트의 색상
    private Vector2 originalLeftImagePos; // 원래 왼쪽 이미지의 위치
    private Vector2 originalRightImagePos; // 원래 오른쪽 이미지의 위치
    

    private Coroutine animateButton;
    void Start()
    {
        originalTextColor = buttonText.color;
        originalLeftImagePos = leftImage.rectTransform.anchoredPosition;
        originalRightImagePos = rightImage.rectTransform.anchoredPosition;

        leftImage.enabled = false;
        rightImage.enabled = false;
    }

   
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (animateButton != null)
        {
            StopCoroutine(animateButton);
        }
        
        
        // 마우스가 버튼에 올라갔을 때의 동작
        buttonText.fontSize += 5; // 폰트 크기 증가
        buttonText.color = new Color(255f / 255f, 139f / 255f, 0f / 255f, 255); // 폰트 색상 변경
        //leftImage.rectTransform.anchoredPosition -= new Vector2(20, 0); // 왼쪽 이미지 이동
        //rightImage.rectTransform.anchoredPosition += new Vector2(20, 0); // 오른쪽 이미지 이동
        //leftImage.enabled = true;
        //rightImage.enabled = true;
        animateButton = StartCoroutine(AnimateButton(true));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (animateButton != null)
        {
            StopCoroutine(animateButton);
        }
        
        // 마우스가 버튼을 떠났을 때의 동작
        buttonText.fontSize -= 5; // 폰트 크기 감소
        buttonText.color = originalTextColor; // 폰트 색상 원래대로 복구
        leftImage.rectTransform.anchoredPosition = originalLeftImagePos; // 왼쪽 이미지 위치 복구
        rightImage.rectTransform.anchoredPosition = originalRightImagePos; // 오른쪽 이미지 위치 복구
        

        animateButton = StartCoroutine(AnimateButton(false));
    }

    private IEnumerator AnimateButton(bool isHovering)
    {
        leftImage.enabled = true;
        rightImage.enabled = true;
        
        if (!isHovering)
        {
            leftImage.enabled = false;
            rightImage.enabled = false;
        }
        float startTime = Time.time;
        float alpha = 0;
        float startScale = 1f;
        float endScale = isHovering ? 1.2f : 1f;
        while (Time.time - startTime < 0.3f)
        {
            float t = (Time.time - startTime) / 0.3f;
            float scale = Mathf.Lerp(startScale, endScale, t);
            transform.localScale = new Vector3(scale, scale, 1);

            
            alpha += Time.time / duration;

            leftImage.color = new Color(255f / 255f, 139f / 255f, 0f / 255f, alpha);
            rightImage.color = new Color(255f / 255f, 139f / 255f, 0f / 255f, alpha);

            yield return null;
        }


        //while (alpha < 1)
        //{
        //    alpha += Time.deltaTime / duration;

        //    leftImage.color = new Color(255, 139, 0, alpha);
        //    rightImage.color = new Color(255, 139, 0, alpha);

        //    yield return null;
        //}
    }
}