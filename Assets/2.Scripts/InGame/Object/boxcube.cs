using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCube : MonoBehaviour
{    
    public bool isCompleted = false;  // ✅ 큐브 완료 상태
    private bool isPlayerInside = false;  // ✅ 플레이어가 안에 있는지 여부

    public GameObject miniGameUI; // 미니게임 UI 패널

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            isCompleted = true;
            Debug.Log("Player entered the box. Starting MiniGame...");
            StartMiniGame();  // ✅ 미니게임 시작 함수 호출
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            Debug.Log("Player exited the box.");
        }
    }

    void StartMiniGame()
    {
        miniGameUI.SetActive(true);  // ✅ 미니게임 UI 활성화
        MiniGameManager miniGame = FindObjectOfType<MiniGameManager>();
        miniGame.StartMiniGame();
    }
}
