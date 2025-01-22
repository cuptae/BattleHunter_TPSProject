using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerCtrl : MonoBehaviour
{
    private Vector3 moveInput;
    private Vector3 lookForward;
    private Vector3 lookSide;
    private Vector3 moveDir;
    private float finalSpeed;
    private bool isRun;
    private float xAxis;
    private float zAxis;

    protected Camera mainCamera;
    protected Animator animator;
    protected CharacterController charCtrl;
    protected bool isMove;
    protected bool isDodge;
    protected bool isInvincible;

    [HideInInspector]
    public bool isFire;
    public float firingWalkSpeed = 3.0f;
    public float walkSpeed = 5.0f;
    public float runSpeed = 10.0f;
    public float dodgeForce = 10.0f;
    public float rotationSpeed = 4.0f;
    public float gravity = 20.0f;
    public GameObject Weapon;
    public LayerMask targetLayer;
    


    protected virtual void Awake() {
        animator = GetComponentInChildren<Animator>();
        Debug.Assert(animator);
        charCtrl = GetComponent<CharacterController>();
        Debug.Assert(charCtrl);
        mainCamera = Camera.main;
    }
    protected virtual void Update()
    {
        DirCheck();
        SpeedCheck();
        Rotation();
        MoveAnim();
        Move();
        Dodge();
    }

    void DirCheck()
    {
        lookForward = new Vector3(mainCamera.transform.forward.x,0,mainCamera.transform.forward.z).normalized;
        lookSide = new Vector3(mainCamera.transform.right.x,0,mainCamera.transform.right.z).normalized;
        moveDir = (lookForward * moveInput.z) + (lookSide*moveInput.x);
    }

    void Move()
    {
 
        xAxis = Input.GetAxis("Horizontal");
		zAxis = Input.GetAxis("Vertical");

        moveInput = new Vector3(xAxis,0,zAxis);

        isMove = (moveInput.magnitude!=0)? true:false;

        moveDir.y -= gravity * Time.deltaTime;

        if(isDodge)
            return;
        charCtrl.Move(moveDir*finalSpeed*Time.deltaTime);
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
        float percent = ((isRun) ? 1f : 0f) * moveInput.magnitude;
        animator.SetFloat("Speed", percent, 0.1f, Time.deltaTime);
        // 좌우 이동 값
        animator.SetFloat("MoveX", xAxis);
        // 전후 이동 값
        animator.SetFloat("MoveZ", zAxis);
        animator.SetBool("Move", isMove);
    }

    IEnumerator DodgeStart()
    {

        if(isDodge)
        {
            isInvincible = true;
            Vector3 dodgeDir = transform.forward;
            charCtrl.Move(dodgeDir*dodgeForce*Time.deltaTime);
        }

        yield return new WaitForSeconds(1.0f);
        isDodge = false;
        isInvincible = false;
    }

    void Dodge()
    {
        if(isDodge)
            return;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            isDodge = true;
            animator.SetTrigger("Dodge");
            StartCoroutine(DodgeStart());
        }
    }
}

