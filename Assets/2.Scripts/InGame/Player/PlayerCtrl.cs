using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Animations.Rigging;
[RequireComponent(typeof(Rigidbody))]
public class PlayerCtrl : MonoBehaviour
{
    private Rigidbody rigid;
    private Vector3 moveInput;
    private Vector3 lookForward;
    private Vector3 lookSide;
    private Vector3 moveDir;
    private Vector3 dodgeDeltaPos;
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

    [HideInInspector]
    public bool isFire;
    public float firingWalkSpeed = 3.0f;
    public float walkSpeed = 5.0f;
    public float runSpeed = 10.0f;
    public float dodgeForce = 15.0f;
    public float rotationSpeed = 4.0f;
    public GameObject Weapon;
    
    protected virtual void Awake() {
        animator = GetComponentInChildren<Animator>();
        Debug.Assert(animator);
        rigid = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }
    protected virtual void Update()
    {
        DirCheck();
        MoveInput();
        SpeedCheck();
        Rotation();
        if(Input.GetKeyDown(KeyCode.Space))StartCoroutine(Dodge());
        MoveAnim();

        Debug.DrawRay(transform.position, lookForward*10);
    }
    
    private void FixedUpdate() {
        Move();
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
        moveInput = new Vector3(xAxis,0,zAxis);
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

        Quaternion rotation = Quaternion.LookRotation(lookForward);
        transform.localRotation = Quaternion.Lerp(transform.localRotation,rotation,rotationSpeed*Time.deltaTime);
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

    // IEnumerator Dodge()
    // {
    //     isDodge = true;
    //     Vector3 pos = rigid.transform.position;
    //     Vector3 targetPos = rigid.transform.position + (moveDir*3.5f);

    //     float elapseTime = 0f;

    //     while(elapseTime<dodgeTime)
    //     {
    //         float ratio = elapseTime/dodgeTime;
    //         Vector3 curPos = Vector3.Lerp(pos,targetPos,ratio);
    //         rigid.MovePosition(targetPos);
    //         elapseTime += Time.deltaTime;
    //         yield return new WaitForEndOfFrame();
    //     }
    //     isDodge =false;
    // }

    IEnumerator Dodge()
    {
        float elapseTime = 0f;
        float dodgeTime = 0.7f;
        Vector3 dodgeDir = isMove?moveDir:transform.forward;
        Quaternion dodgeLook = isMove?Quaternion.LookRotation(moveDir):Quaternion.LookRotation(transform.forward);
        animator.SetTrigger("Dodge");
        while(elapseTime<dodgeTime)
        {
            isDodge = true;
            transform.rotation = dodgeLook;
            rigid.MovePosition(transform.position+dodgeDir.normalized*dodgeForce*Time.deltaTime);
            elapseTime += Time.deltaTime;
            yield return null;
        }
        isDodge = false;
    }
    void DodgeOut()
    {
        animator.applyRootMotion = false;
        isDodge = false;
    }
}

