using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxcube : MonoBehaviour
{
    public bool isPlayerInside = false;
    public bool isCompleted = false;  // 큐브가 완료되었는지 여부를 추적

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            Debug.Log("Player is inside the box collider: " + isPlayerInside);
            isCompleted = true;  // 플레이어가 접근하면 완료 상태로 설정
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            Debug.Log("Player is outside the box collider: " + isPlayerInside);
            isCompleted = false;  // 플레이어가 나가면 완료 상태를 false로 설정
        }
    }
}
