using System.Collections;
using System.IO;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine.UI;
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
    public Vector3 groundNormal{get; private set;}


    private PlayerStateMachine stateMachine;
    public STATE curState; 
    public Movetype movetype;
    private Vector3 moveInput;
    private Vector3 lookForward;
    private Vector3 lookSide;

    
    private RaycastHit slopeHit;
    public bool isMove;
    public bool isDodge = false;
    public bool isAttack;


    public CharacterStat characterStat;
    protected Camera mainCamera;

    //Photon
    protected PhotonView pv = null;
    protected Vector3 curPos = Vector3.zero;
    protected Quaternion curRot = Quaternion.identity;
    protected Transform tr;
    [HideInInspector]
    private Transform camFollow;
    public GameObject weapon;
    public int curHp{get; private set;}

    public event System.Action OnHpChanged; // HP 변경 이벤트


    [SerializeField] 
    Transform groundCheck;

    public List<ActiveSkill> activeSkills;


    public bool rSkillTrigger;
    public bool qSkillTrigger;
    public bool eSkillTrigger;
    public bool dodgeTrigger;


    public GameObject TestBtn;
    protected virtual void Awake() {
        TestBtn = GameObject.FindWithTag("Test");
        animator = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        //characterData = new CharacterData();
        characterStat = new CharacterStat();
        camFollow = transform.Find("CameraFollow");
        mainCamera = Camera.main;

        stateMachine =new PlayerStateMachine();

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

        TestBtn.GetComponent<Button>().onClick.AddListener(TestSkillLevelUP);
    }
    protected virtual void Update()
    {
        if(pv.isMine)
        {
            qSkillTrigger = QSkillInput();
            eSkillTrigger = ESkillInput();
            rSkillTrigger = RSkillInput();
            dodgeTrigger = DodgeInput();
            MoveInput();
            RunInput();
            if(IsSlope()&&IsGrounded())
            {
                rigid.useGravity = false;
            }
            else
            {
                rigid.useGravity = true;
            }
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
            tr.position = Vector3.Lerp(tr.position,curPos,Time.fixedDeltaTime * characterStat.RunSpeed);
            tr.rotation = Quaternion.Slerp(tr.rotation, curRot, Time.fixedDeltaTime * characterStat.RotationSpeed);
        }
    }
    void MoveInput()
    {
        float xAxis = Input.GetAxisRaw("Horizontal");
		float zAxis = Input.GetAxisRaw("Vertical");
        moveInput = new Vector3(xAxis,0,zAxis).normalized;
        isMove = moveInput.magnitude > 0;
    }
    public bool RunInput(){return Input.GetKey(KeyCode.LeftShift);}
    public bool DodgeInput(){return Input.GetKeyDown(KeyCode.Space);}
    
    public Vector3 MoveDir()
    {
        lookSide = new Vector3(mainCamera.transform.right.x,0,mainCamera.transform.right.z).normalized;
        return (lookForward * moveInput.z) + (lookSide*moveInput.x);
    }

    public void Rotation()
    {
        lookForward = new Vector3(mainCamera.transform.forward.x,0,mainCamera.transform.forward.z).normalized;
        curRot = Quaternion.LookRotation(lookForward);
        transform.localRotation = Quaternion.Lerp(transform.localRotation,curRot,characterStat.RotationSpeed*Time.deltaTime);
    }
    
    public bool IsSlope()
    {
        Ray ray = new Ray(transform.position,Vector3.down);
        if(Physics.Raycast(ray,out slopeHit,2.0f,GameManager.Instance.groundLayer))
        {
            groundNormal = slopeHit.normal;
            float groundAngle = Vector3.Angle(Vector3.up,groundNormal);
            return groundAngle != 0f;
        }
        return false;
    }



    public bool IsGrounded()
    {
        Vector3 boxSize = new Vector3(transform.lossyScale.x-0.5f, 0.05f, transform.lossyScale.z-0.5f);
        return Physics.CheckBox(groundCheck.position, boxSize, Quaternion.identity, GameManager.Instance.groundLayer);
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

    public abstract void Attack();

    public bool QSkillInput(){return Input.GetKeyDown(KeyCode.Q);}
    public bool ESkillInput(){return Input.GetKeyDown(KeyCode.E);}
    public bool RSkillInput(){return Input.GetKeyDown(KeyCode.R);}

    public void GetDamage(int damage)
    {
        if(PhotonNetwork.isMasterClient)
        {
            pv.RPC("TakeDamage",PhotonTargets.AllBuffered,damage);
        }
    }

    [PunRPC]
    public void TakeDamage(int damage,PhotonMessageInfo info)
    {
        curHp -= damage;
        if(curHp<=0)
        {
            ChangeState(new PlayerDieState(this));
        }
        OnHpChanged?.Invoke(); // HP 변경 이벤트 호출
    }
    protected virtual void  OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);
            stream.SendNext(curHp);
            stream.SendNext(isMove);
            stream.SendNext(animator.GetFloat("Speed"));
            stream.SendNext(animator.GetFloat("MoveX"));
            stream.SendNext(animator.GetFloat("MoveZ"));
            stream.SendNext(animator.GetBool("Move"));
            stream.SendNext(rSkillTrigger);
            stream.SendNext(qSkillTrigger);
            stream.SendNext(eSkillTrigger);
            stream.SendNext(dodgeTrigger);
        }
        else
        {
            curPos = (Vector3)stream.ReceiveNext();
            curRot = (Quaternion)stream.ReceiveNext();
            curHp = (int)stream.ReceiveNext();
            isMove = (bool)stream.ReceiveNext();
            animator.SetFloat("Speed",(float)stream.ReceiveNext());
            animator.SetFloat("MoveX",(float)stream.ReceiveNext());
            animator.SetFloat("MoveZ",(float)stream.ReceiveNext());
            animator.SetBool("Move",(bool)stream.ReceiveNext());
            rSkillTrigger = (bool)stream.ReceiveNext();
            qSkillTrigger = (bool)stream.ReceiveNext();
            eSkillTrigger = (bool)stream.ReceiveNext();
            dodgeTrigger = (bool)stream.ReceiveNext();

            if (eSkillTrigger)
            {
                animator.SetTrigger("ESkill");
                eSkillTrigger = false;
            }
            if (rSkillTrigger)
            {
                animator.SetTrigger("RSkill");
                rSkillTrigger = false; // 한 번만 발동하게 초기화
            }
            if (qSkillTrigger)
            {
                animator.SetTrigger("QSkill");
                qSkillTrigger = false;
            }
            if (dodgeTrigger)
            {
                animator.SetTrigger("Dodge");
                dodgeTrigger = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 boxSize = new Vector3(transform.lossyScale.x, 0.05f, transform.lossyScale.z);
        Gizmos.DrawWireCube(groundCheck.position, boxSize);
    }


    public void TestSkillLevelUP()
    {
        if(SkillManager.Instance.modify <3)
        {
            SkillManager.Instance.modify += 1;
            Debug.Log($"현재 레벨 : {SkillManager.Instance.modify}");
        }
        else
        {
            Debug.Log("이미 최대 레벨 입니다");
            return;
        }

        activeSkills.Clear();
        activeSkills = SkillManager.Instance.SkillAdd();


    }

    protected void SetHPInit(int hp)
    {
        curHp = hp;;
    }
}