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
    private float finalSpeed;
    private bool isRun;
    private float xAxis;
    private float zAxis;
    private float moveAnimPercent;

    protected Camera mainCamera;
    protected Animator animator;
    protected bool isMove;
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
    public LayerMask targetLayer;
    
    protected virtual void Awake() {
        animator = GetComponentInChildren<Animator>();
        Debug.Assert(animator);
        rigid = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }
    protected virtual void Update()
    {
        DirCheck();
        SpeedCheck();
        Rotation();
        MoveAnim();
        MoveInput();
        Dodge();
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
        if(Input.GetKeyDown(KeyCode.Space))
        {
            isDodge = true;
        }
        moveInput = new Vector3(xAxis,0,zAxis);
        isMove = (moveInput.magnitude!=0)? true:false; 
    }

    void Move()
    {
        
        rigid.MovePosition(rigid.position+moveDir*finalSpeed*Time.deltaTime);
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

        if(isDodge)
        {
            finalSpeed = 0f;
        }
        else if(isRun){
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

    void Dodge()
    {
        if(Input.GetButtonDown("Jump")&&!!isDodge)
        {
            Vector3 dodgePower = transform.forward*dodgeForce;
            rigid.AddForce(dodgePower,ForceMode.VelocityChange);
            isDodge = true;
        }

        Invoke("DodgeOut",1.0f);
    }

    void DodgeOut()
    {
        //rigid.velocity = Vector3.zero;
        isDodge = false;
    }

}

