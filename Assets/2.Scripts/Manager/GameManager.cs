using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{

    public Character curCharacter = Character.NONSELECTED;
    public int enemyLayerMask;
    public int groundLayer;
    public bool gameEnd = false;


    protected override void Awake()
    {
        base.Awake();
        enemyLayerMask = 1<<LayerMask.NameToLayer("ENEMY");
        groundLayer = 1<<LayerMask.NameToLayer("GROUND");
    }
}
