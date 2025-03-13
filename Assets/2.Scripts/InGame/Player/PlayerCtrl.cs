using System.Collections;
using System.IO;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public enum STATE
{
    IDLE,
    MOVE,
    ATTACK,
    WALKATTACK,
    DODGE,
    DEAD,
}
[RequireComponent(typeof(Rigidbody))]
public abstract class PlayerCtrl : MonoBehaviour
{
    public Animator animator{get; protected set;}
    public Rigidbody rigid{get; protected set;}
    public Vector3 moveDir{get; protected set;}

    private Vector3 moveInput;
    private Vector3 lookForward;
    private Vector3 lookSide;
    private Vector3 dodgeDir;
    private Collider col;

    protected int enemyLayerMask;

    public float finalSpeed{get; private set;}
    private float xAxis;
    private float zAxis;
    private float moveAnimPercent;
    public float dodgeTime = 0.7f;

    protected Camera mainCamera;
    public bool isMove;
    public bool isDodge = false;

    public bool isAttack;
    public float attackRange;
    public float attackWalkSpeed = 3.0f;
    public float walkSpeed = 5.0f;
    public float runSpeed = 10.0f;
    public float dodgeForce;
    public float rotationSpeed = 4.0f;
    public float moveForce;

    
    public GameObject weapon;
    public STATE curState; 
    public PlayerStateMachine stateMachine; 

    //Photon
    protected PhotonView pv = null;
    protected Vector3 curPos = Vector3.zero;
    protected Quaternion curRot = Quaternion.identity;

    protected Transform tr;
    [HideInInspector]
    public Transform camFollow;
    
    protected virtual void Awake() {
        animator = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        col = GetComponent<CapsuleCollider>(); 
        pv = GetComponent<PhotonView>();
        camFollow = transform.GetChild(0).Find("CameraFollow");
        mainCamera = Camera.main;
        enemyLayerMask = 1<<LayerMask.NameToLayer("ENEMY");
        stateMachine =new PlayerStateMachine();
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
    protected virtual void Start() {
        if(stateMachine != null)
        {
            stateMachine.Initialize(new IdleState(this));
            Debug.Log("Input stateMachine");
        }
        else
            Debug.Log("stateMachine is null");
    }
    protected virtual void Update()
    {
        if(pv.isMine)
        {
            DirCheck();
            MoveInput();
            Rotation();
            RunInput();
            DodgeInput();
            stateMachine.Update();
        }
    }

    private void FixedUpdate() {
        if(pv.isMine)
        {
            if (isDodge)
            {
                rigid.MovePosition(transform.position + dodgeDir.normalized * dodgeForce * Time.fixedDeltaTime);
                //rigid.AddForce(dodgeDir * dodgeForce,ForceMode.Impulse);
            }
            stateMachine.FixedUpdate();
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

    public void Rotation()
    {
        if(DodgeInput())
            return;

        curRot = Quaternion.LookRotation(lookForward);
        transform.localRotation = Quaternion.Lerp(transform.localRotation,curRot,rotationSpeed*Time.deltaTime);
    }
    
    public bool RunInput(){return Input.GetKey(KeyCode.LeftShift);}
    public bool DodgeInput(){return Input.GetKeyDown(KeyCode.Space);}


    IEnumerator Dodge()
    {
        if(DodgeInput())
            yield break;

        float elapseTime = 0f;
        dodgeDir = isMove?moveDir:transform.forward;
        if (dodgeDir == Vector3.zero)
        {
            dodgeDir = transform.forward;  // 기본 방향 설정
        }
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

    public void ChangeState(PlayerState newState)
    {
        stateMachine.ChangeState(newState);
    }

    protected abstract void Attack();

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


#region Move()
    // public void Move()
    // {
    //     if(isDodge)
    //         return;

    //     //rigid.MovePosition(transform.position+moveDir*finalSpeed*Time.deltaTime);

    //     if(isMove)
    //     {
    //         rigid.AddForce(moveDir*moveForce,ForceMode.Force);
    //     }
    //     else
    //     {
    //         rigid.velocity = Vector3.zero;
    //     }
    //     if (rigid.velocity.magnitude > finalSpeed)
    //     {
    //         rigid.velocity = rigid.velocity.normalized * finalSpeed;
    //     }
    // }
#endregion
#region  SpeedCheck()
    // void SpeedCheck()
    // {
    //     if(Input.GetKey(KeyCode.LeftShift)&&!isAttack){
    //         isRun = true;
    //     }
    //     else{
    //         isRun = false;
    //     }

    //     // if(isRun){
    //     //     finalSpeed = runSpeed;
    //     // }
    //     // else if(isAttack){
    //     //     finalSpeed = attackWalkSpeed;
    //     // }
    //     // else{
    //     //     finalSpeed = walkSpeed;
    //     // }
    // }
    #endregion