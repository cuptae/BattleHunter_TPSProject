using System.Collections;
using System.IO;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
[RequireComponent(typeof(Rigidbody))]
public class PlayerCtrl : MonoBehaviour
{
    private Rigidbody rigid;

    private Vector3 moveInput;
    private Vector3 lookForward;
    private Vector3 lookSide;
    private Vector3 moveDir;
    private Vector3 dodgeDir;
    private Collider col;

    protected Transform tr;
    protected int enemyLayerMask;

    private float finalSpeed;
    private bool isRun;
    private float xAxis;
    private float zAxis;
    private float moveAnimPercent;
    private float dodgeTime = 0.7f;

    protected Camera mainCamera;
    protected Animator animator;
    public bool isMove;
    public bool isDodge = false;
    protected bool isInvincible;

    
    public bool isAttack;
    public float attackRange;
    public float attackWalkSpeed = 3.0f;
    public float walkSpeed = 5.0f;
    public float runSpeed = 10.0f;
    public float dodgeForce;
    public float rotationSpeed = 4.0f;

    public float moveForce;
    
    public GameObject weapon;
    

    protected PhotonView pv = null;
    protected Vector3 curPos = Vector3.zero;
    protected Quaternion curRot = Quaternion.identity;

    [HideInInspector]
    public Transform camFollow;
    
    protected virtual void Awake() {
        animator = GetComponentInChildren<Animator>();
        Debug.Assert(animator);
        rigid = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        col = GetComponent<CapsuleCollider>(); 
        pv = GetComponent<PhotonView>();
        camFollow = transform.GetChild(0).Find("CameraFollow");
        mainCamera = Camera.main;
        enemyLayerMask = 1<<LayerMask.NameToLayer("ENEMY");

        pv.ObservedComponents[0] = this;
        pv.synchronization = ViewSynchronization.UnreliableOnChange;

        if(pv.isMine)
        {
            mainCamera.GetComponent<CameraCtrl>().target = camFollow;
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
            if(Input.GetKeyDown(KeyCode.Space))StartCoroutine(Dodge());
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
            tr.position = Vector3.Lerp(tr.position,curPos,Time.fixedDeltaTime * runSpeed);
            tr.rotation = Quaternion.Slerp(tr.rotation, curRot, Time.fixedDeltaTime * rotationSpeed);
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
        xAxis = Input.GetAxisRaw("Horizontal");
		zAxis = Input.GetAxisRaw("Vertical");
        moveInput = new Vector3(xAxis,0,zAxis).normalized;
        isMove = moveInput.magnitude > 0;
    }

    void Move()
    {
        if(isDodge)
            return;
        //rigid.MovePosition(transform.position+moveDir*finalSpeed*Time.deltaTime);
        if(isMove)
        {
            rigid.AddForce(moveDir*moveForce,ForceMode.Force);
        }
        else
        {
            rigid.velocity = Vector3.zero;
        }
        if (rigid.velocity.magnitude > finalSpeed)
        {
            rigid.velocity = rigid.velocity.normalized * finalSpeed;
        }
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
        if(Input.GetKey(KeyCode.LeftShift)&&!isAttack){
            isRun = true;
        }
        else{
            isRun = false;
        }

        if(isRun){
            finalSpeed = runSpeed;
        }
        else if(isAttack){
            finalSpeed = attackWalkSpeed;
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
        animator.SetFloat("MoveX", Input.GetAxis("Horizontal"));
        // 전후 이동 값
        animator.SetFloat("MoveZ", Input.GetAxis("Vertical"));
        animator.SetBool("Move", isMove);
    }

    IEnumerator Dodge()
    {
        if(isDodge)
            yield break;

        float elapseTime = 0f;
        dodgeDir = isMove?moveDir:transform.forward;
        Quaternion dodgeLook = isMove?Quaternion.LookRotation(moveDir):Quaternion.LookRotation(transform.forward);
        animator.SetTrigger("Dodge");

        Collider[] monCols = Physics.OverlapSphere(tr.position,7.0f,enemyLayerMask);

        foreach (Collider monsterCol in monCols)
        {
            //Physics.IgnoreCollision(col, monsterCol, true);
        }

        while(elapseTime<dodgeTime)
        {
            isDodge = true;
            transform.rotation = dodgeLook;
            elapseTime += Time.deltaTime;
            yield return null;
        }
        isDodge = false;

        foreach (Collider monsterCollider in monCols)
        {   if(monsterCollider != null)
            {

            }
                //Physics.IgnoreCollision(col, monsterCollider, false);
        }
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

