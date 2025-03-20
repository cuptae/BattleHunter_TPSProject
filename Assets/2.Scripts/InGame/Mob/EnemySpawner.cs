using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject prefab; // ������ ������
    public Transform spawnPoint; // ������ ��ġ
    public float spawnInterval = 1f; // ���� �ֱ� (�� ����)

    private int spawnCount = 0; // ������ ���� ī��Ʈ

    void Start()
    {
        if (prefab == null || spawnPoint == null)
        {
            Debug.LogWarning("������ �Ǵ� ���� ����Ʈ�� �������� �ʾҽ��ϴ�!");
            return;
        }

        // �ݺ� ����
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval); // 1�� ��� �� ����

            // ������ ����
            Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);

            // ī��Ʈ ����
            spawnCount++;

            // �ܼ� ��� (������)
            Debug.Log($"���� ������ ��: {spawnCount}");
        }
    }
}
