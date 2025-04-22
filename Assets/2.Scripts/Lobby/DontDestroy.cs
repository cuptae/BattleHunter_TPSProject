using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        //이 오브젝트는 씬 전환시 사라지지 않음
        DontDestroyOnLoad(this.gameObject);
        //Application.LoadLevel("scLobby");
        SceneManager.LoadScene("Ingame");
    }
}