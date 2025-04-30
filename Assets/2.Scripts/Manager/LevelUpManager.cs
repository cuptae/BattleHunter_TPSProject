using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpManager : MonoSingleton<LevelUpManager>
{
    public PlayerCtrl player;

    public GameObject levelUpPanel;
    public Button qLevelUpButton;
    public Button eLevelUpButton;
    public Button rLevelUpButton;

    public Image qLevelUpIcon;
    public Image eLevelUpIcon;
    public Image rLevelUpIcon;

    public Text qdesc;
    public Text edesc;
    public Text rdesc;
    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerCtrl>();

        qLevelUpIcon.sprite = SkillManager.Instance.QskillIcon.sprite;
        eLevelUpIcon.sprite = SkillManager.Instance.EskillIcon.sprite;
        rLevelUpIcon.sprite = SkillManager.Instance.RskillIcon.sprite;

        qLevelUpButton.GetComponent<Button>().onClick.AddListener(() => { player.QLevelUp(); HideLevelUpPanel(); });
        eLevelUpButton.GetComponent<Button>().onClick.AddListener(() => { player.ELevelUp(); HideLevelUpPanel(); });
        rLevelUpButton.GetComponent<Button>().onClick.AddListener(() => { player.RLevelUp(); HideLevelUpPanel(); });
    }

    public void ShowLevelUpPanel()
    {
        levelUpPanel.SetActive(true);
    }
    public void HideLevelUpPanel()
    {
        qdesc.text = player.activeSkills[0].nextSkillDesc;
        edesc.text = player.activeSkills[1].nextSkillDesc;
        rdesc.text = player.activeSkills[2].nextSkillDesc;
        levelUpPanel.SetActive(false);
    }
}
