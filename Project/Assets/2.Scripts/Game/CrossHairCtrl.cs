using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CrossHairCtrl : MonoBehaviour
{
    public RectTransform upCrossHairRect;
    public RectTransform downCrossHairRect;
    public RectTransform leftCrossHairRect;
    public RectTransform rightCrossHairRect;
    public GameObject player;

    [Range(50,100)]
    public float defaultPos;

    [Range(30,70)]
    public float firePos;

    private float finalPos;

    private void Start() {
        finalPos = defaultPos;
    }
    // Update is called once per frame
    void Update()
    {
        float targetXpos = player.GetComponent<PlayerCtrl>().isFire? firePos:defaultPos;

        finalPos = Mathf.Lerp(finalPos,targetXpos,0.05f);

        upCrossHairRect.anchoredPosition = new Vector2(0,finalPos);
        downCrossHairRect.anchoredPosition = new Vector2(0,-finalPos);
        leftCrossHairRect.anchoredPosition = new Vector2(-finalPos,0);
        rightCrossHairRect.anchoredPosition = new Vector2(finalPos,0);
    }
}
