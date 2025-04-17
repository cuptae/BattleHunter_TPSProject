using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class csIntroManager : MonoBehaviour
{
    public string lobbySceneName = "scLobby";

    public float introDuration = 3f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadLobbyScene());
    }


    IEnumerator LoadLobbyScene()
    {
        yield return new WaitForSeconds(introDuration);

        SceneManager.LoadScene(lobbySceneName);
    }
    // Update is called once per frame
  
}
