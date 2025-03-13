using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEngine;

public class MoveState : PlayerState
{
    private float finalSpeed;
    private float moveAnimPercent;
    public MoveState(PlayerCtrl player) : base(player) {}
    public override void EnterState()
    {
        player.animator.SetBool("Move", true);
        player.curState = STATE.MOVE;
    }

    public override void UpdateState()
    {
        if(player.RunInput())
        {
            finalSpeed = player.runSpeed;
            player.animator.SetFloat("Speed", 1, 0.1f, Time.deltaTime);
        }
        else if(player.isAttack){
            finalSpeed = player.attackWalkSpeed;
        }
        else{
            finalSpeed = player.walkSpeed;
        }
        MoveAnim();
        player.Rotation();

        if (!player.isMove)
        {
            player.ChangeState(new IdleState(player));
        }
        if (player.DodgeInput())
        {
            player.ChangeState(new DodgeState(player));
        }
    }
    public override void FixedUpdateState()
    {
        //rigid.MovePosition(transform.position+moveDir*finalSpeed*Time.deltaTime);

        if(player.isMove)
        {
            player.rigid.AddForce(player.moveDir*player.moveForce,ForceMode.Force);
        }
        else
        {
            player.rigid.velocity = Vector3.zero;
        }
        if (player.rigid.velocity.magnitude > finalSpeed)
        {
            player.rigid.velocity = player.rigid.velocity.normalized * finalSpeed;
        }
    }

    public override void ExitState()
    {
        player.animator.SetBool("Move", false);
        player.animator.SetFloat("MoveX", 0);
        player.animator.SetFloat("MoveZ", 0);
        player.rigid.velocity = Vector3.zero;
    }

    void MoveAnim()
    {
        moveAnimPercent = player.RunInput() ? 1f : 0f;
        player.animator.SetFloat("Speed", moveAnimPercent, 0.1f, Time.deltaTime);
        // 좌우 이동 값
        player.animator.SetFloat("MoveX", Input.GetAxis("Horizontal"));
        // 전후 이동 값
        player.animator.SetFloat("MoveZ", Input.GetAxis("Vertical"));
    }
}
