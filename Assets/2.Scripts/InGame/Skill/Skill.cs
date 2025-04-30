using System.Collections;

public interface ISkill
{
    IEnumerator Activation();
    IEnumerator LevelUp();
}
