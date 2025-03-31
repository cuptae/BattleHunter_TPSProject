using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoSingleton<GameManager>
{
    public Character curCharacter = Character.NONSELECTED;
    public int enemyLayerMask;
    public int groundLayer;


    protected override void Awake()
    {
        base.Awake();
        enemyLayerMask = 1<<LayerMask.NameToLayer("ENEMY");
        groundLayer = 1<<LayerMask.NameToLayer("GROUND");
    }
}
