using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlotManager : MonoBehaviour
{
    public Image[] slotImages;

    [SerializeField] private List<int> selectedSkillIds = new List<int>();
    [SerializeField] private List<Sprite> selectedSkillIcons = new List<Sprite>();

    public void SetSkill(int skillId, Sprite icon)
    {
        if (selectedSkillIds.Count >= 2) return;

        selectedSkillIds.Add(skillId);
        selectedSkillIcons.Add(icon);

        int index = selectedSkillIds.Count - 1;
        if (index < slotImages.Length)
        {
            slotImages[index].sprite = icon;
        }
    }

    public void ResetSlots()
    {
        selectedSkillIds.Clear();
        selectedSkillIcons.Clear();
        foreach (var img in slotImages)
        {
            img.sprite = null;
        }
    }

    public List<int> GetSelectedSkillIds()
    {
        return selectedSkillIds;
    }

    public void SetInitialSkillSlots(int q, int e)
    {
        selectedSkillIds.Clear();
        if (q > 0) selectedSkillIds.Add(q);
        if (e > 0) selectedSkillIds.Add(e);
    }
}
