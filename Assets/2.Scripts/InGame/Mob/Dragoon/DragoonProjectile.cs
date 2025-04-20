using UnityEngine;

public class DragoonProjectile : MonoBehaviour
{
    public float speed = 10f;
    private Vector3 direction;

    private Vector3 startPosition;
    public int damage = 10;

    private Vector3 curPos;
    private Quaternion curRot;
    private Transform tr;
    private PhotonView pv;

    void OnEnable()
    {
        //Invoke("ResetProjectile",5.0f);
    }


    void Awake()
    {
        tr = GetComponent<Transform>();
        pv = GetComponent<PhotonView>();
        pv.ObservedComponents[0] = this;
        pv.synchronization = ViewSynchronization.UnreliableOnChange;
    }

    void Start()
    {
        startPosition = transform.position;
    }

    public void Launch(Vector3 shootDirection)
    {
        direction = shootDirection.normalized;
    }


    void Update()
    {        
        if(pv.isMine)
        {
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        if (!pv.isMine)
        {
            tr.position = Vector3.Lerp(tr.position, curPos, Time.fixedDeltaTime * speed);
            tr.rotation = Quaternion.Slerp(tr.rotation, curRot, Time.fixedDeltaTime*10f);
        }
    }

    void ResetProjectile()
    {
        PoolManager.Instance.PvReturnObject("DragoonProjectile",this.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCtrl playerdamege = other.GetComponent<PlayerCtrl>();

            if (playerdamege != null)
            {
                playerdamege.GetDamage(damage);
                CancelInvoke("ResetProjectile");
                PoolManager.Instance.PvReturnObject("DragoonProjectile",this.gameObject);
            }
        }
    }

    [PunRPC]
    public void EnableObject(Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;

        // Lerp 기준값도 업데이트
        curPos = pos;
        curRot = rot;
        gameObject.SetActive(true);
    }

    [PunRPC]
    public void DisableObject()
    {
        gameObject.SetActive(false);
    }


     public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.isWriting)
        {
            stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);
        }
        else
        {
            curPos = (Vector3)stream.ReceiveNext();
            curRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
