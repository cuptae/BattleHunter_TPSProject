using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameClear : MonoBehaviour
{
    CanvasGroup gameClearCanvasGroup;
    public Image gameClearImage;
    int playerCount = 0;
    public float fadeDuration = 2f;

    
    void Awake()
    {
        gameClearCanvasGroup = gameClearImage.GetComponent<CanvasGroup>();
        gameClearCanvasGroup.alpha = 0f;
        gameClearImage.gameObject.SetActive(false);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerCount++;
            if (playerCount == 1)
            {
                gameClearImage.gameObject.SetActive(true);
                gameClearCanvasGroup = gameClearImage.GetComponent<CanvasGroup>();
                gameClearCanvasGroup.alpha = 0f;
                StartCoroutine(FadeIn(gameClearCanvasGroup));
            }
        }
    }

    IEnumerator FadeIn(CanvasGroup canvasGroup)
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }
}
