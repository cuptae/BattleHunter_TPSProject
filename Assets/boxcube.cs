using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCube : MonoBehaviour
{
    public bool isPlayerInside = false;
    public bool isCompleted = false;
    public GameObject miniGameUI; // 미니 게임 UI 패널을 할당할 변수

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            Debug.Log("Player is inside the box collider: " + isPlayerInside);
            isCompleted = true;  // 플레이어가 접근하면 완료 상태로 설정
            StartMiniGame();  // 미니 게임 시작
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

    // 미니 게임을 시작하는 함수
    void StartMiniGame()
    {
       // miniGameUI.SetActive(true);  // 미니 게임 UI 활성화
        // 미니 게임 관련 초기화 코드가 필요하다면 여기서 추가할 수 있습니다.

    MiniGameManager miniGame = FindObjectOfType<MiniGameManager>();
    miniGame.StartMiniGame();
    }
}
