using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Abilities/Ability Data")]
public class AbilityData : ScriptableObject
{
    public AbilityType AbilityTypes;
    public EffectType TypeEffect;
    public AbilityTag Tags;

    public List<IDamageable> Targets;

    public Projectile Proj;
    public Transform StartPosition;
    public AbilityLogic Logic;

    public int CountTargets;
    public IconAbility icon;
    public Sprite sprite;
    public string AbilityName;
    public string Description;
    public int HealPower;
    public int Damage;
    public int ManaCost;
    public float CritChance;
    public float CritDamage;
    public float CastTime;
    public float AilmentChance;
    public float AilmentPower;
    public float AilmentDuration;
    public float CooldawnDuration;
    public ParticleSystem VFXPrefab;
    public bool SelfAnimation;
    public float SelfDuration;
}
