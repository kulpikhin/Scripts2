using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityData : ScriptableObject
{
    public List<AbilityType> AbilityTypes;
    public List<AbilityTag> Tags;

    public AbilityLogic abilityLogic;
    public AbilityName abilityName;
    public int CountTargets;
    public Sprite sprite;
    public string Description;
    public int HealPower;
    public int Damage;
    public int ManaCost;
    public int CritChance;
    public int CritDamage;
    public float CastTime;
    public float AilmentChance;
    public float AilmentPower;
    public float AilmentDuration;
}
