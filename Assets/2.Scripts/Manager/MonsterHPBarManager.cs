using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MonsterHPBarManager : MonoSingleton<MonsterHPBarManager>
{
    public GameObject hpBarPrefab;
    public Transform canvasTransform;

    private List<MonsterHPBar> activeBars = new();

    public MonsterHPBar CreateHPBar(EnemyCtrl enemy)
{
    // ✅ 예외 처리 추가
    if (hpBarPrefab == null)
    {
        Debug.LogError("❌ hpBarPrefab이 연결되어 있지 않습니다!");
        return null;
    }

    if (canvasTransform == null || canvasTransform.gameObject.scene.name == null)
    {
        Debug.LogError("❌ canvasTransform이 씬 안에 존재하지 않습니다! 프리팹을 참조하고 있을 가능성이 높습니다.");
        return null;
    }

    GameObject barObj = Instantiate(hpBarPrefab, canvasTransform);
    MonsterHPBar bar = barObj.GetComponent<MonsterHPBar>();

    // ✅ MonsterHPBar가 없으면 경고 출력
    if (bar == null)
    {
        Debug.LogError("❌ MonsterHPBar 컴포넌트를 프리팹에서 찾을 수 없습니다!");
        return null;
    }

    bar.enemyCtrl = enemy;
    bar.enemyTransform = enemy.transform;
    activeBars.Add(bar);
    return bar;
}




    public void RemoveHPBar(MonsterHPBar bar)
    {
        if (activeBars.Contains(bar))
        {
            activeBars.Remove(bar);
            Destroy(bar.gameObject); // 또는 SetActive(false) 후 풀링
        }
    }
}
