using System.Collections;

public interface ISkill
{
    //public abstract void Init();
    public abstract IEnumerator Activation();
    public IEnumerator SkillActivation();
    public void SkillLevelUp();
    public void SetSkillData(int skillId);
}
