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
    #region Components
    [HideInInspector]
    public Animator animator { get; private set; }
    [HideInInspector]
    public Rigidbody rigid { get; private set; }
    protected Camera mainCamera;
    public PhotonView pv{get; private set;} = null;
    protected Transform tr;
    private Transform camFollow;
    #endregion

    #region State Management
    private PlayerStateMachine stateMachine;
    public STATE curState;
    public Movetype movetype;
    #endregion

    #region Movement Variables
    [SerializeField]
    private Transform groundCheck;   
    private Vector3 moveInput;
    private Vector3 lookForward;
    private Vector3 lookSide;
    private RaycastHit slopeHit;
    public Vector3 groundNormal { get; private set; }
    public bool isMove;
    public bool isDodge = false;
    public bool isAttack;
    public bool isDead = false;
    #endregion

    #region Character Stats
    public CharacterStat characterStat;
    public int curHp { get; private set; }
    public event System.Action OnHpChanged;
    public float damageReduceRate =1f;

    public bool invincible;
    public int skillQ;
    public int skillE;
    #endregion

    #region Skill Variables
    [HideInInspector]
    public List<ActiveSkill> activeSkills;
    protected Image abilitycooldownbar;
    // public bool rSkillTrigger = false;
    // public bool qSkillTrigger = false;
    // public bool eSkillTrigger = false;
    private bool dodgeTrigger;
    public bool canAbility = true;
    #endregion

    #region Weapon and Effects
    public GameObject weapon;
    public GameObject TestBtn;
    
    public InventoryManager invenMgr;
    public IngameUIManager IngameUI;
    public SlotClass[] backupItems;

    #endregion

    #region Photon Networking
    protected Vector3 curPos = Vector3.zero;
    protected Quaternion curRot = Quaternion.identity;
    #endregion

    #region Unity Callbacks
    protected virtual void Awake()
    {
        invenMgr = GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>();
        IngameUI = GameObject.FindGameObjectWithTag("UIManager").GetComponent<IngameUIManager>();
        //TestBtn = GameObject.FindWithTag("Test");
        abilitycooldownbar = GameObject.FindWithTag("AbilityCooldown").GetComponent<Image>();
        animator = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        characterStat = new CharacterStat();
        camFollow = transform.Find("CameraFollow");
        mainCamera = Camera.main;
        damageReduceRate = 1f;

        stateMachine = new PlayerStateMachine();

        pv = GetComponent<PhotonView>();
        pv.ObservedComponents[0] = this;
        pv.synchronization = ViewSynchronization.UnreliableOnChange;
        if (pv.isMine)
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

    protected virtual void Start()
    {
        if (stateMachine != null)
        {
            stateMachine.Initialize(new IdleState(this));
        }
        skillQ = GameStartData.skillIdQ;
        skillE = GameStartData.skillIdE;

        SlotExStat(skillE);
        SlotExStat(skillQ);
        backupItems = new SlotClass[32];
        for(int i = 0; i < backupItems.Length; i++)
        {
            backupItems[i] = new SlotClass();
        }
    }

    protected virtual void Update()
    {
        if (pv.isMine)
        {
            dodgeTrigger = DodgeInput();
            MoveInput();
            RunInput();
            if (IsSlope() && IsGrounded())
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

    private void FixedUpdate()
    {
        if (pv.isMine)
        {
            stateMachine.FixedUpdate();
        }
        else
        {
            tr.position = Vector3.Lerp(tr.position, curPos, Time.fixedDeltaTime * characterStat.RunSpeed);
            tr.rotation = Quaternion.Slerp(tr.rotation, curRot, Time.fixedDeltaTime * characterStat.RotationSpeed);
        }
    }
    #endregion

    #region Movement Methods
    void MoveInput()
    {
        float xAxis = Input.GetAxisRaw("Horizontal");
        float zAxis = Input.GetAxisRaw("Vertical");
        moveInput = new Vector3(xAxis, 0, zAxis).normalized;
        isMove = moveInput.magnitude > 0;
    }

    public bool RunInput() { return Input.GetKey(KeyCode.LeftShift); }
    public bool DodgeInput() { return Input.GetKeyDown(KeyCode.Space); }

    public Vector3 MoveDir()
    {
        lookSide = new Vector3(mainCamera.transform.right.x, 0, mainCamera.transform.right.z).normalized;
        return (lookForward * moveInput.z) + (lookSide * moveInput.x);
    }

    public void Rotation()
    {
        lookForward = new Vector3(mainCamera.transform.forward.x, 0, mainCamera.transform.forward.z).normalized;
        curRot = Quaternion.LookRotation(lookForward);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, curRot, characterStat.RotationSpeed * Time.deltaTime);
    }

    public bool IsSlope()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out slopeHit, 2.0f, GameManager.Instance.groundLayer))
        {
            groundNormal = slopeHit.normal;
            float groundAngle = Vector3.Angle(Vector3.up, groundNormal);
            return groundAngle != 0f;
        }
        return false;
    }

    public bool IsGrounded()
    {
        Vector3 boxSize = new Vector3(transform.lossyScale.x - 0.5f, 0.05f, transform.lossyScale.z - 0.5f);
        return Physics.CheckBox(groundCheck.position, boxSize, Quaternion.identity, GameManager.Instance.groundLayer);
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
    [PunRPC]
    public void RPC_PlaySkillAnim(int skillType)
    {
        switch (skillType)
        {
            case 1:
                animator.SetTrigger("QSkill");
                break;
            case 2:
                animator.SetTrigger("ESkill");
                break;
            case 3:
                animator.SetTrigger("RSkill");
                break;
        }
    }
    #endregion

    #region State Management Methods
    public void ChangeState(PlayerState newState)
    {
        stateMachine.ChangeState(newState);
    }
    #endregion

    #region Abstract Methods
    public abstract void Attack();
    public abstract void UniqueAbility();
    #endregion

    #region PhotonSerializeView
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
            // stream.SendNext(qSkillTrigger);
            // stream.SendNext(eSkillTrigger);
            // stream.SendNext(rSkillTrigger);
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
            // qSkillTrigger = (bool)stream.ReceiveNext();
            // eSkillTrigger = (bool)stream.ReceiveNext();
            // rSkillTrigger = (bool)stream.ReceiveNext();
            // if (qSkillTrigger)
            // {
            //     animator.SetTrigger("QSkill");
            // }
            // if (eSkillTrigger)
            // {
            //     animator.SetTrigger("ESkill");
            // }
            // if (rSkillTrigger)
            // {
            //     animator.SetTrigger("RSkill");
            // }
        }
    }

    [PunRPC]
    public void AttachEffect(int viewID)
    {
        GameObject effect = PhotonView.Find(viewID).gameObject;
        effect.transform.SetParent(this.transform);
        effect.transform.localPosition = new Vector3(0, 1f, 0);
        effect.transform.localRotation = Quaternion.identity;
    }

    [PunRPC]
    public void DetachEffect(int viewID)
    {
        GameObject effect = PhotonView.Find(viewID).gameObject;
        effect.transform.SetParent(null);
        effect.transform.localPosition = Vector3.zero;
        effect.transform.localRotation = Quaternion.identity;
    }
    #endregion

    #region Skill Input Methods
    public bool QSkillInput() { return Input.GetKeyDown(KeyCode.Q); }
    public bool ESkillInput() { return Input.GetKeyDown(KeyCode.E); }
    public bool RSkillInput() { return Input.GetKeyDown(KeyCode.R); }
    #endregion

    #region Damage and HP Methods
    public void GetDamage(int damage)
    {
        if (invincible||curState == STATE.DEAD) return;
        if (PhotonNetwork.isMasterClient)
        {
            pv.RPC("TakeDamage", PhotonTargets.AllBuffered, damage);
        }
    }
    [PunRPC]
    public void TakeDamage(int damage, PhotonMessageInfo info)
    {
        // 데미지 감소율 적용
        int reducedDamage = Mathf.CeilToInt(damage * damageReduceRate); // 감소된 데미지를 계산

        // HP 감소
        curHp -= reducedDamage;

        // HP가 0 이하라면 사망 상태로 전환
        if (curHp <= 0&&curState != STATE.DEAD)
        {
            // 상태 머신을 PlayerDieState로 변경
            stateMachine.ChangeState(new PlayerDieState(this));
            //아이템 백업
            for (int i = 0; i < invenMgr.items.Length; i++)
            {
                if(invenMgr.items[i] != null)
                {
                    backupItems[i].AddItem(invenMgr.items[i].GetItem(), invenMgr.items[i].GetCount());                
                }
            }
            
            //빈털터리됨
            for(int i = 0; i < invenMgr.CurrentItems().Length; i++)
            {
                invenMgr.CurrentItems()[i].Clear();
            }
            

        }
        else if (curHp > characterStat.MaxHp)
        {
            curHp = characterStat.MaxHp;
        }

        // HP 변경 이벤트 호출
        OnHpChanged?.Invoke();
    }

    protected void SetHPInit(int hp) { curHp = hp; }

    [PunRPC]
    public void RPC_SetInvincible(bool _invincible)
    {
        invincible = _invincible;
    }

    public void SetInvincible(bool _invincible) { 
        invincible = _invincible;
        if (pv.isMine)
        {
            pv.RPC("RPC_SetInvincible", PhotonTargets.AllBuffered, _invincible);
        }
    }
    #endregion


    #region Stat Modification Methods
    
    //데미지 변경
    public void ModifyDamage(int damage)
    {
        characterStat.modifyDamage += damage;
    }

    //최대 체력 변경
    public void ModifyMaxHp(int maxHp)
    {
        characterStat.modifyMaxHp += maxHp;
    }

    //공격 속도 변경
    public void ModifyAttackRate(float attackRate)
    {
        characterStat.modifyAttackRate += attackRate;
    }

    //이동 속도 변경
    public void ModifyMoveSpeed(float moveSpeed)
    {
        characterStat.modifyWalkSpeed += moveSpeed;
        characterStat.modifyRunSpeed += moveSpeed;
    }
    #endregion
    

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
    public void SlotExStat(int slotId)
    {
        switch (slotId)
        {
            case 1: //공격력 증가
                ModifyDamage(10); // 예시로 10 증가 
                Debug.Log("공격력 증가");
                break;

            case 2: //체력 증가
                ModifyMaxHp(50); // 예시로 50 증가
                SetHPInit(characterStat.MaxHp); // 최대 체력으로 초기화 가능   
                Debug.Log("체력 증가");
                break;

            case 3: //이동속도 증가
                ModifyMoveSpeed(5f);
                Debug.Log("이동속도 증가");
                break;

            default:
                Debug.Log("유효하지 않은 슬롯입니다.");
                break;
        }
    }
}