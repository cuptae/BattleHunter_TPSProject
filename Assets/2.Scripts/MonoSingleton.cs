using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T: MonoBehaviour
{

    private static T _instance = null;
    public static T Instance{

        get{
            if(_instance == null)
            {
                _instance = GameObject.FindObjectOfType<T>();
                if(_instance == null)
                {
                    _instance = new GameObject("Singleton_" + typeof(T).ToString(), typeof(T)).GetComponent<T>();
                    DontDestroyOnLoad(_instance);
                }
            }
            return _instance;
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
