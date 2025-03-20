using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject prefab; // 생성할 프리팹
    public Transform spawnPoint; // 스폰할 위치
    public float spawnInterval = 1f; // 생성 주기 (초 단위)

    private int spawnCount = 0; // 생성된 개수 카운트

    void Start()
    {
        if (prefab == null || spawnPoint == null)
        {
            Debug.LogWarning("프리팹 또는 스폰 포인트가 설정되지 않았습니다!");
            return;
        }

        // 반복 실행
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval); // 1초 대기 후 실행

            // 프리팹 생성
            Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);

            // 카운트 증가
            spawnCount++;

            // 콘솔 출력 (디버깅용)
            Debug.Log($"현재 생성된 수: {spawnCount}");
        }
    }
}
