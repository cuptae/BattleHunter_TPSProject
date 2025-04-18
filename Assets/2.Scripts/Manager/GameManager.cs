using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{

    public Character curCharacter = Character.NONSELECTED;
    public string userId;
    public int enemyLayerMask;
    public int groundLayer;
    public bool gameEnd = false;
    public bool startGame{get; private set;}


    protected override void Awake()
    {
        base.Awake();
        enemyLayerMask = 1<<LayerMask.NameToLayer("ENEMY");
        groundLayer = 1<<LayerMask.NameToLayer("GROUND");
    }

    public void SetStartGame(bool startGame){this.startGame = startGame;}
}
