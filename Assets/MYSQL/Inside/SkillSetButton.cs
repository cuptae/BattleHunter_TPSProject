using UnityEngine;
using UnityEngine.UI;

public class SkillSetButton : MonoBehaviour
{
    [Header("�� ��ư�� �����ϴ� ��ų ���� �ε���")]
    public int skillIndex; // SkillLoader�� skillImageSlots, loadedSkillIds �ε����� �����ϰ� ����� ��

    [Header("���� ����")]
    public SkillLoader skillLoader;       // Skill ������ ������ ���
    public SkillSlotManager slotManager;  // Q/E ���� ���� ���

    public void OnClickSet()
    {
        // �ε����� ��ȿ���� Ȯ��
        if (skillIndex < 0 || skillIndex >= skillLoader.loadedSkillIds.Length)
        {
            Debug.LogWarning($"[SkillSetButton] ��ȿ���� ���� �ε���: {skillIndex}");
            return;
        }

        int skillId = skillLoader.loadedSkillIds[skillIndex];
        Sprite icon = skillLoader.loadedSkillSprites[skillIndex];

        if (skillId < 0 || icon == null)
        {
            Debug.LogWarning($"[SkillSetButton] ��ų ������ �����ϴ�. (Index: {skillIndex})");
            return;
        }

        // Q/E ���Կ� ��ų ����
        slotManager.SetSkill(skillId, icon);
    }
}
