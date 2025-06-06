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
    private float nextStepTime = 0f;
    private float stepDelay;
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
        if(Time.time >= nextStepTime)
        {
            SoundManager.Instance.PlaySFX(SFXCategory.GUNNER, PLAYER.STEP,player.transform.position);
            nextStepTime = Time.time + stepDelay;
        }
        if (!player.isMove){player.ChangeState(new IdleState(player));}
        if (player.DodgeInput()){player.ChangeState(new DodgeState(player));}
        if(player.isAttack){player.ChangeState(new PlayerAttackState(player));}
        // Q 스킬 입력 처리
        if (player.QSkillInput() && !player.activeSkills[0].isOnCooldown)
        {
            player.ChangeState(new SkillState(player, player.activeSkills[0]));
        }

        // E 스킬 입력 처리
        if (player.ESkillInput() && !player.activeSkills[1].isOnCooldown)
        {
            player.ChangeState(new SkillState(player, player.activeSkills[1]));
        }

        // R 스킬 입력 처리
        if (player.RSkillInput() && !player.activeSkills[2].isOnCooldown)
        {
            player.ChangeState(new SkillState(player, player.activeSkills[2]));
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
        if(player.RunInput())
        {
            curSpeed = player.characterStat.RunSpeed;
            stepDelay = 0.2f;
            player.animator.SetFloat("Speed", 1, 0f, Time.deltaTime);
        }
        else{
            curSpeed = player.characterStat.WalkSpeed;
            stepDelay = 0.5f;
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
