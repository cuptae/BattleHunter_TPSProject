using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    PlayerCtrl player;

    public Image hpBar;

    void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerCtrl>();
        player.OnHpChanged += UpdateHPBar; // Subscribe to the OnHpChanged event
        if (player == null)
        {
            Debug.LogError("PlayerCtrl component not found on Player object.");
        }
    }

    void UpdateHPBar()
    {
        if (player != null && hpBar != null)
        {
            float hpPercentage = (float)player.curHp / player.characterStat.MaxHp; // int를 float로 변환
            hpBar.fillAmount = hpPercentage;
        }
    }
}
