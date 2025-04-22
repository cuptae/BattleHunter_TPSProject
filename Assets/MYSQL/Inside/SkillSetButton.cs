using UnityEngine;
using UnityEngine.UI;

public class SkillSetButton : MonoBehaviour
{
    [Header("이 버튼이 참조하는 스킬 슬롯 인덱스")]
    public int skillIndex; // SkillLoader의 skillImageSlots, loadedSkillIds 인덱스와 동일하게 맞춰야 함

    [Header("참조 연결")]
    public SkillLoader skillLoader;       // Skill 정보를 가져올 대상
    public SkillSlotManager slotManager;  // Q/E 슬롯 저장 대상

    public void OnClickSet()
    {
        // 인덱스가 유효한지 확인
        if (skillIndex < 0 || skillIndex >= skillLoader.loadedSkillIds.Length)
        {
            Debug.LogWarning($"[SkillSetButton] 유효하지 않은 인덱스: {skillIndex}");
            return;
        }

        int skillId = skillLoader.loadedSkillIds[skillIndex];
        Sprite icon = skillLoader.loadedSkillSprites[skillIndex];

        if (skillId < 0 || icon == null)
        {
            Debug.LogWarning($"[SkillSetButton] 스킬 정보가 없습니다. (Index: {skillIndex})");
            return;
        }

        // Q/E 슬롯에 스킬 저장
        slotManager.SetSkill(skillId, icon);
    }
}
