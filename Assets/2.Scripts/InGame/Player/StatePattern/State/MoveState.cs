using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public enum Movetype
{
    AddForce,
    MovePosition,
}
public class MoveState : PlayerState
{
    private float curSpeed;
    private float moveAnimPercent;
    private float moveForce = 30f;
    public MoveState(PlayerCtrl player) : base(player) {}
    public override void EnterState()
    {
        player.animator.SetBool("Move", true);
        player.curState = STATE.MOVE;
    }

    public override void UpdateState()
    {
        player.MoveDir();
        player.Rotation();
        SpeedCheck();

        if (!player.isMove){player.ChangeState(new IdleState(player));}
        if (player.DodgeInput()){player.ChangeState(new DodgeState(player));}

        if(player.QSkillInput())
        {
            player.ChangeState(new SkillState(player,player.activeSkills[0]));
        }
        if(player.ESkillInput())
        {
            player.ChangeState(new SkillState(player,player.activeSkills[1]));
        }
        if(player.RSkillInput())
        {
            player.ChangeState(new SkillState(player,player.activeSkills[2]));
        }
    }
    public override void FixedUpdateState()
    {
        Move();
    }

    public override void ExitState()
    {
        player.animator.SetBool("Move", false);
        player.rigid.velocity = Vector3.zero;
    }

    void Move()
    {
        if(player.movetype == Movetype.AddForce)
        {
            if(player.isMove)
            {
                if(!player.IsSlope())
                    player.rigid.AddForce(player.MoveDir()*moveForce,ForceMode.Force);
                else
                {
                    //player.rigid.AddForce(player.MoveDir()*moveForce,ForceMode.Force);
                    player.rigid.AddForce(Vector3.ProjectOnPlane(player.MoveDir(),player.groundNormal).normalized*moveForce,ForceMode.Force);
                }
            }
            else
            {
                player.rigid.velocity = Vector3.zero;
            }
            if (player.rigid.velocity.magnitude > curSpeed)
            {
                player.rigid.velocity = player.rigid.velocity.normalized * curSpeed;
            }
        }
        else
        {
            player.rigid.MovePosition(player.transform.position+player.MoveDir()*curSpeed*Time.deltaTime);
        }
    }
    void SpeedCheck()
    {
        if(player.isAttack){
            curSpeed = player.characterStat.AttackWalkSpeed;
        }
        else if(player.RunInput())
        {
            curSpeed = player.characterStat.RunSpeed;
            player.animator.SetFloat("Speed", 1, 0f, Time.deltaTime);
        }
        else{
            curSpeed = player.characterStat.WalkSpeed;
        }
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
