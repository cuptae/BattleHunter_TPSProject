using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnLobby : MonoBehaviour
{
    public void ReturnToLobby()
    {
        // 로비로 돌아갈 때 BGM 변경
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayBGM(BGMType.MainMenu);
        }

        // LOBBY 씬으로 이동
        SceneManager.LoadScene("Lobby");
    }
}
