using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainOption : MonoBehaviour
{
    public Toggle Op_Graphics;
    public Toggle Op_Sounds;

    // Start is called before the first frame update
    void Start()
    {
        Op_Graphics.onValueChanged.AddListener(delegate{SetCurOption();});
        Op_Sounds.onValueChanged.AddListener(delegate{SetCurOption();});
    }

    void SetCurOption(){
        
    }
}
