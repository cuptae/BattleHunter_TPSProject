using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameExit : MonoBehaviour
{
    public void ExitGame()
    {
        Debug.Log("게임 종료");
        Application.Quit(); // 어플리케이션에서 실행 중인 경우 종료

        // 유니티 에디터에서 실행 중이라면 강제 종료
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
