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
    [HideInInspector]
    public Animator animator{get; protected set;}
    [HideInInspector]
    public Rigidbody rigid{get; protected set;}
    private PlayerStateMachine stateMachine;
    public Movetype movetype;
    private Vector3 moveInput;
    private Vector3 lookForward;
    private Vector3 lookSide;
    private RaycastHit slopeHit;

    protected int enemyLayerMask;
    private  int groundLayer;
    private float xAxis;
    private float zAxis;

    protected Camera mainCamera;
    public bool isMove;
    public bool isDodge = false;
    public bool isAttack;
    public CharacterData characterData{get; protected set;}

    public STATE curState; 
    public Vector3 groundNormal;


    //Photon
    protected PhotonView pv = null;
    protected Vector3 curPos = Vector3.zero;
    protected Quaternion curRot = Quaternion.identity;
    protected Transform tr;
    [HideInInspector]
    private Transform camFollow;
    public GameObject weapon;
    
    protected virtual void Awake() {
        animator = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        characterData = new CharacterData();
        camFollow = transform.Find("CameraFollow");
        mainCamera = Camera.main;

        stateMachine =new PlayerStateMachine();

        enemyLayerMask = 1<<LayerMask.NameToLayer("ENEMY");
        groundLayer = 1<<LayerMask.NameToLayer("GROUND");

        pv = GetComponent<PhotonView>();
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
        }
    }
    protected virtual void Update()
    {
        if(pv.isMine)
        {
            MoveInput();
            RunInput();
            IsSlope();
            MoveAnim();
            stateMachine.Update();
        }
    }

    private void FixedUpdate() {
        if(pv.isMine)
        {
            stateMachine.FixedUpdate();
        }
        else
        {
            tr.position = Vector3.Lerp(tr.position,curPos,Time.fixedDeltaTime * characterData.runSpeed);
            tr.rotation = Quaternion.Slerp(tr.rotation, curRot, Time.fixedDeltaTime * characterData.rotationSpeed);
        }
    }
    void MoveInput()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
		zAxis = Input.GetAxisRaw("Vertical");
        moveInput = new Vector3(xAxis,0,zAxis).normalized;
        isMove = moveInput.magnitude > 0;
    }
    public bool RunInput(){return Input.GetKey(KeyCode.LeftShift);}
    public bool DodgeInput(){return Input.GetKeyDown(KeyCode.Space);}
    
    public Vector3 MoveDir()
    {
        lookForward = new Vector3(mainCamera.transform.forward.x,0,mainCamera.transform.forward.z).normalized;
        lookSide = new Vector3(mainCamera.transform.right.x,0,mainCamera.transform.right.z).normalized;
        return (lookForward * moveInput.z) + (lookSide*moveInput.x);
    }

    public void Rotation()
    {
        curRot = Quaternion.LookRotation(lookForward);
        transform.localRotation = Quaternion.Lerp(transform.localRotation,curRot,characterData.rotationSpeed*Time.deltaTime);
    }
    
    public bool IsSlope()
    {
        Ray ray = new Ray(transform.position,Vector3.down);
        if(Physics.Raycast(ray,out slopeHit,2.0f,groundLayer))
        {
            groundNormal = slopeHit.normal;
            float groundAngle = Vector3.Angle(Vector3.up,groundNormal);
            return groundAngle != 0f;
        }
        return false;
    }

    public void ChangeState(PlayerState newState)
    {
        stateMachine.ChangeState(newState);
    }

    void MoveAnim()
    {
        float moveAnimPercent = RunInput() ? 1f : 0f;
        animator.SetFloat("Speed", moveAnimPercent, 0.1f, Time.deltaTime);
        // 좌우 이동 값
        animator.SetFloat("MoveX", Input.GetAxis("Horizontal"));
        // 전후 이동 값
        animator.SetFloat("MoveZ", Input.GetAxis("Vertical"));
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