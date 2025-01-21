using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerCtrl : MonoBehaviour
{
    private Vector3 moveInput;
    private float finalSpeed;
    private bool isRun;

    protected Camera mainCamera;
    protected Animator animator;
    protected CharacterController charCtrl;
    protected bool isMove;

    [HideInInspector]
    public bool isFire;
    public float firingWalkSpeed = 3.0f;
    public float walkSpeed = 5.0f;
    public float runSpeed = 10.0f;
    public float jumpForce = 6.0f;
    public float rotationSpeed = 4.0f;
    public float gravity = 20.0f;
    public GameObject Weapon;
    


    protected virtual void Awake() {
        animator = GetComponentInChildren<Animator>();
        Debug.Assert(animator);
        charCtrl = GetComponent<CharacterController>();
        Debug.Assert(charCtrl);
        mainCamera = Camera.main;
    }
    protected virtual void Update()
    {
        Move();
        //Shooting();
    }
    void Move()
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

        float xAxis = Input.GetAxis("Horizontal");
		float zAxis = Input.GetAxis("Vertical");

        moveInput = new Vector3(xAxis,0,zAxis);

        isMove = (moveInput.magnitude!=0)? true:false;

        Vector3 lookForward = new Vector3(mainCamera.transform.forward.x,0,mainCamera.transform.forward.z).normalized;
        Vector3 lookSide = new Vector3(mainCamera.transform.right.x,0,mainCamera.transform.right.z).normalized;
        Vector3 moveDir = (lookForward * moveInput.z) + (lookSide*moveInput.x);

        // //percent는 애니메이션 블랜드 수치 run이 ture이면 1이고 false라면 0.5로 뛰는 애니메이션과 걷는 애니메이션 구분
        float percent = ((isRun) ? 1f : 0f) * moveInput.magnitude;
        Quaternion rotation = Quaternion.LookRotation(lookForward);
        transform.localRotation = Quaternion.Lerp(transform.localRotation,rotation,rotationSpeed*Time.deltaTime);


        PlayerMoveAnim(isMove, xAxis, zAxis, percent);

        //if(isMove){
        //    animator.SetBool("Move",true);
        //}
        //else{
        //    animator.SetBool("Move",false);
        //}
        moveDir.y -= gravity * Time.deltaTime;
        charCtrl.Move(moveDir*finalSpeed*Time.deltaTime);
    }


    void PlayerMoveAnim(bool isMove,float xAxis,float zAxis,float percent)
    {
        animator.SetFloat("Speed", percent, 0.1f, Time.deltaTime);
        // 좌우 이동 값
        animator.SetFloat("MoveX", xAxis);
        // 전후 이동 값
        animator.SetFloat("MoveZ", zAxis);
        animator.SetBool("Move", isMove);
    }



}

