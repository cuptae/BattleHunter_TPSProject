using System.Collections;
using System.IO;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class PlayerCtrl : MonoBehaviour
{
    private Rigidbody rigid;
    private Vector3 moveInput;
    private Vector3 lookForward;
    private Vector3 lookSide;
    private Vector3 moveDir;
    private Vector3 dodgeDeltaPos;
    private Vector3 dodgeDir;
    protected Transform tr;
    private float finalSpeed;
    private bool isRun;
    private float xAxis;
    private float zAxis;
    private float moveAnimPercent;
    private float dodgeTime = 1.0f;

    protected Camera mainCamera;
    protected Animator animator;
    public bool isMove;
    public bool isDodge = false;
    protected bool isInvincible;

    public bool isFire;
    public float firingWalkSpeed = 3.0f;
    public float walkSpeed = 5.0f;
    public float runSpeed = 10.0f;
    public float dodgeForce;
    public float rotationSpeed = 4.0f;
    
    public GameObject Weapon;
    

    protected PhotonView pv = null;
    protected Vector3 curPos = Vector3.zero;
     protected Quaternion curRot = Quaternion.identity;
    public Transform camFollow;
    
    protected virtual void Awake() {
        animator = GetComponentInChildren<Animator>();
        Debug.Assert(animator);
        rigid = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        pv = GetComponent<PhotonView>();
        camFollow = transform.GetChild(0).Find("CameraFollow");
        mainCamera = Camera.main;

        pv.ObservedComponents[0] = this;
        pv.synchronization = ViewSynchronization.UnreliableOnChange;

        if(pv.isMine)
        {
            mainCamera.GetComponent<CameraCtrl>().target = camFollow;
            //mainCamera.GetComponent<CameraCtrlVer2>().target = camFollow; 
        }
        else
        {
            rigid.isKinematic = true;
            curPos = tr.position;
            curRot = tr.rotation;
        }
    }
    protected virtual void Update()
    {
        if(pv.isMine)
        {
            DirCheck();
            MoveInput();
            SpeedCheck();
            Rotation();
            if(Input.GetKeyDown(KeyCode.LeftControl))StartCoroutine(Dodge());
            MoveAnim();
        }

    }
    
    private void FixedUpdate() {
        if(pv.isMine)
        {
            if (isDodge)
            {
                rigid.MovePosition(transform.position + dodgeDir.normalized * dodgeForce * Time.fixedDeltaTime);
            }
            Move();
        }
        else
        {
            tr.position = Vector3.Lerp(tr.position,curPos,Time.deltaTime * runSpeed);
            tr.rotation = Quaternion.Slerp(tr.rotation, curRot, Time.deltaTime * rotationSpeed);
        }
    }
    
    void DirCheck()
    {
        lookForward = new Vector3(mainCamera.transform.forward.x,0,mainCamera.transform.forward.z).normalized;
        lookSide = new Vector3(mainCamera.transform.right.x,0,mainCamera.transform.right.z).normalized;
        moveDir = (lookForward * moveInput.z) + (lookSide*moveInput.x);
    }

    void MoveInput()
    {
        xAxis = Input.GetAxis("Horizontal");
		zAxis = Input.GetAxis("Vertical");
        moveInput = new Vector3(xAxis,0,zAxis).normalized;
        isMove = (moveInput.magnitude!=0)? true:false; 
    }

    void Move()
    {
        if(isDodge)
            return;
        rigid.MovePosition(transform.position+moveDir*finalSpeed*Time.deltaTime);
    }

    void Rotation()
    {
        if(isDodge)
            return;

        curRot = Quaternion.LookRotation(lookForward);
        transform.localRotation = Quaternion.Lerp(transform.localRotation,curRot,rotationSpeed*Time.deltaTime);
    }

    void SpeedCheck()
    {
        if(Input.GetKey(KeyCode.LeftShift)&&!isFire){
            isRun = true;
        }
        else{
            isRun = false;
        }

        if(isRun){
            finalSpeed = runSpeed;
        }
        else if(isFire){
            finalSpeed = firingWalkSpeed;
        }
        else{
            finalSpeed = walkSpeed;
        }
    }

    void MoveAnim()
    {
        moveAnimPercent = ((isRun) ? 1f : 0f) * moveInput.magnitude;
        animator.SetFloat("Speed", moveAnimPercent, 0.1f, Time.deltaTime);
        // 좌우 이동 값
        animator.SetFloat("MoveX", xAxis);
        // 전후 이동 값
        animator.SetFloat("MoveZ", zAxis);
        animator.SetBool("Move", isMove);
    }

    IEnumerator Dodge()
    {
        if(isDodge)
            yield break;

        float elapseTime = 0f;
        float dodgeTime = 0.7f;
        dodgeDir = isMove?moveDir:transform.forward;
        Quaternion dodgeLook = isMove?Quaternion.LookRotation(moveDir):Quaternion.LookRotation(transform.forward);
        animator.SetTrigger("Dodge");
        while(elapseTime<dodgeTime)
        {
            isDodge = true;
            transform.rotation = dodgeLook;
            elapseTime += Time.deltaTime;
            yield return null;
        }
        isDodge = false;
    }

    protected virtual void  OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);
            stream.SendNext(isMove);
            stream.SendNext(animator.GetFloat("Speed"));
            stream.SendNext(animator.GetFloat("MoveX"));
            stream.SendNext(animator.GetFloat("MoveZ"));
            stream.SendNext(animator.GetBool("Move"));
        }
        else
        {
            curPos = (Vector3)stream.ReceiveNext();
            curRot = (Quaternion)stream.ReceiveNext();
            isMove = (bool)stream.ReceiveNext();
            animator.SetFloat("Speed",(float)stream.ReceiveNext());
            animator.SetFloat("MoveX",(float)stream.ReceiveNext());
            animator.SetFloat("MoveZ",(float)stream.ReceiveNext());
            animator.SetBool("Move",(bool)stream.ReceiveNext());
             
        }
    }
}

