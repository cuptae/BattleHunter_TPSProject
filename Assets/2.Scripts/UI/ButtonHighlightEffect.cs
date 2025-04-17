using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHighlightEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image buttonImage;
    public Sprite normalImage;
    public Sprite highlightImage;
    // Start is called before the first frame update
    void Start()
    {
        buttonImage.sprite = normalImage;
    }

    public void OnPointerEnter(PointerEventData eventDate)
    {
        buttonImage.sprite = highlightImage;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonImage.sprite = normalImage;
    }
}
