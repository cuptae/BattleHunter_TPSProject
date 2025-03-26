
using System;
using System.Collections.Generic;
using UnityEngine;
public class ActiveData
{
    public int skillId{get; private set;}
    public string skillName{get; private set;}
    public string skillDesc{get; private set;}
    public string skillIcon{get; private set;}
    public float cooltime{get; private set;}
    public float attackRange{get; private set;}
    public bool isCharge{get; private set;}
    public int projectileCount{get; private set;}
    public bool isPenetrate{get; private set;}
    public List<string> skillEffectParam{get; private set;}
    public List<string> skillType{get; private set;}
}
