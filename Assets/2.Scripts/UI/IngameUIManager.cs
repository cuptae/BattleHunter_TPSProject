using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class IngameUIManager : MonoBehaviour
{
    public Animator anim;
    public string InvenOn;
    public string InvenOff;
    public GameObject OptionPanel;

    public bool isOnPlaying = false;
    public bool isOptionPanel = false;

    private AudioClip invenOpenSound;
    private AudioClip invenCloseSound;

    private void Awake()
    {
        invenOpenSound = Resources.Load("invenOpenSound") as AudioClip;
        invenCloseSound = Resources.Load("invenCloseSound") as AudioClip;
    }
    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(!isOnPlaying && !isOptionPanel)
            {
                anim.SetTrigger(InvenOn);
                isOnPlaying = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
            } 
            
            else if(isOptionPanel)
            {
                return;
            }

            else if(isOnPlaying)
            {
                anim.SetTrigger(InvenOff);
                isOnPlaying = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }


        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isOptionPanel && !isOnPlaying)
            {
                OptionPanel.SetActive(true);
                isOptionPanel = true;
            }
            else if(isOnPlaying)
            {
                return;
            }
            else if(isOptionPanel)
            {
                OptionPanel.SetActive(false);
                isOptionPanel = false;
            }
        }
    }
    public IEnumerator WorkBenchInteraction()
    {
        if (!isOnPlaying && !isOptionPanel)
        {
            anim.SetTrigger(InvenOn);
            isOnPlaying = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else if (isOptionPanel)
        {
            yield return null;
        }
        else if (isOnPlaying)
        {
            anim.SetTrigger(InvenOff);
            isOnPlaying = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            yield return new WaitForSeconds(0.3f);
        }
    }

    public void OpBackButtonO()
    {
        OptionPanel.SetActive(false);
        isOptionPanel = false;
    }
}
