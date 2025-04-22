using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameStartTrigger : MonoBehaviour
{
    private PhotonView pv;

    public Image image;
    public Sprite[] imageSprite;
    public bool ready;

    public GameStartTrigger gameStartTrigger;
    public Spawner spawner;

    private Coroutine checkKeyCoroutine;

    private GameObject currentPlayer;

    void Awake()
    {
        pv = GetComponent<PhotonView>();
        pv.ObservedComponents[0] = this;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player") && !ready)
        {
            currentPlayer = other.gameObject;

            pv.RPC("ImageActive", PhotonTargets.All, true);

            if (checkKeyCoroutine == null)
                checkKeyCoroutine = StartCoroutine(CheckFKeyCoroutine());
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.CompareTag("Player") && other.gameObject == currentPlayer)
        {
            pv.RPC("ImageActive", PhotonTargets.All, false);
            ready = false;
            pv.RPC("ChangeSprite", PhotonTargets.All, 0);

            if (checkKeyCoroutine != null)
            {
                StopCoroutine(checkKeyCoroutine);
                checkKeyCoroutine = null;
            }

            currentPlayer = null;
        }
    }

    [PunRPC]
    public void RPC_StartGame()
    {
        spawner.StartSpawn();
    }

    [PunRPC]
    public void ChangeSprite(int spriteIndex)
    {
        if (spriteIndex >= 0 && spriteIndex < imageSprite.Length)
        {
            image.sprite = imageSprite[spriteIndex];
        }
    }

    [PunRPC]
    public void ImageActive(bool active)
    {
        image.gameObject.SetActive(active);
    }

    public void StartGame()
    {
        pv.RPC("RPC_StartGame", PhotonTargets.All);
        pv.RPC("ChangeSprite", PhotonTargets.All, 2);
        gameStartTrigger.image.sprite = imageSprite[2]; 
    }

    private IEnumerator CheckFKeyCoroutine()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (currentPlayer != null && currentPlayer.GetComponent<PhotonView>().isMine)
                {
                    if (gameStartTrigger.ready && spawner != null)
                    {
                        pv.RPC("ChangeSprite", PhotonTargets.All, 1);
                        Invoke("StartGame", 2f);
                    }
                    else
                    {
                        ready = true;
                        pv.RPC("ChangeSprite", PhotonTargets.All, 1);
                    }
                    yield return new WaitForSeconds(2.5f);
                    break;
                }
            }

            yield return null;
        }
    }

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.isWriting)
        {
            stream.SendNext(ready);
        }
        else
        {
            ready = (bool)stream.ReceiveNext();
        }
    }
}
