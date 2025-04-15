
using System;
using System.Collections.Generic;
using SKILLCONSTANT;
using UnityEngine;
public class ActiveData
{
    public int skillId{get; private set;}
    public string skillName{get; private set;}
    public string skillDesc{get; private set;}
    public string skillIcon{get; private set;}
    public int damage{get; private set;}
    public float cooltime{get; private set;}
    public float attackRange{get; private set;}
    public float attackDistance{get; private set;}
    public bool isCharge{get; private set;}
    public int chargeCount{get; private set;}
    public int projectileCount{get; private set;}
    public float duration{get; private set;}
    public SkillEffect skillEffectParam{get; private set;}
    public SkillType skillType{get; private set;}
    public PlayerCtrl caster{get; private set;}

public void SetSkillId(int skillId) { this.skillId = skillId; }
public void SetSkillName(string skillName) { this.skillName = skillName; }
public void SetSkillDesc(string skillDesc) { this.skillDesc = skillDesc; }
public void SetSkillIcon(string skillIcon) { this.skillIcon = skillIcon; }
public void SetSkillDamage(int damage){this.damage = damage;}
public void SetCooltime(float cooltime) { this.cooltime = cooltime; }
public void SetAttackRange(float attackRange) { this.attackRange = attackRange; }
public void SetAttackDistance(float attackDistance){this.attackDistance = attackDistance;}
public void SetIsCharge(bool isCharge) { this.isCharge = isCharge; }
public void SetChargeCount(int chargeCount){this.chargeCount = chargeCount;}
public void SetProjectileCount(int projectileCount) { this.projectileCount = projectileCount; }
public void SetDuration(float duration) { this.duration = duration; }
public void SetSkillEffectParam(SkillEffect skillEffectParam) { this.skillEffectParam = skillEffectParam; }
public void SetSkillType(SkillType skillType) { this.skillType = skillType; }
public void SetCaster(PlayerCtrl caster){this.caster = caster;}
}
