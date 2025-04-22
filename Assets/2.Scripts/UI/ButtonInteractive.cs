using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInteractive : MonoBehaviour
{
    public Button button1;
    public Button button2;
    public Sprite activeImage;
    public Sprite InactiveImage;
    public Color activeTextColor;
    public Color InactiveTextColor;
    private Text button1Text;
    private Text button2Text;

    // Start is called before the first frame update
    void Start()
    {
        button1Text = button1.GetComponentInChildren<Text>();
        button2Text = button2.GetComponentInChildren<Text>();

        button1.image.sprite = activeImage;
        button1Text.color = activeTextColor;
        button2.image.sprite = InactiveImage;
        button2Text.color = InactiveTextColor;
        button1.onClick.AddListener(Button1Click);
        button2.onClick.AddListener(Button2Click);
    }

    private void Button1Click()
    {
        button1.image.sprite = activeImage;
        button1Text.color = activeTextColor;
        button2.image.sprite = InactiveImage;
        button2Text.color = InactiveTextColor;
        button1.interactable = false;
        button2.interactable = true;
    }

    private void Button2Click()
    {
        button1.image.sprite = InactiveImage;
        button1Text.color = InactiveTextColor;
        button2.image.sprite = activeImage;
        button2Text.color = activeTextColor;
        button1.interactable = true;
        button2.interactable = false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
