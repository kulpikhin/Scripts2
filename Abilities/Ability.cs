using System;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    private AbilityData abilityData;
    public AbilityCooldown AbilityCooldawner { get; private set;}
    public IDamageable Owner { get; private set; }
    public AbilityData AbilityDatas => abilityData;
    public Transform StartPosition { get; private set; }
    public List<AbilityType> Types { get; private set; }
    public List<AbilityTag> ListTags;
    public List<AbilityType> ListTypes;

    public List<IDamageable> Targets;

    public IconAbility icon;
    public Projectile Proj;
    public Sprite sprite;
    public string AbilityName;
    public string Description;
    public ParticleSystem SelfVFXPrefab;
    public ParticleSystem TargetVFXPrefab;
    public ParticleSystem DurationVFXPrefab;
    public EffectType TypeEffect;

    public AbilityLogic Logic;
    public int CountTargets;
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
    public bool SelfTarget;
    public float BuffDuration;
    public int BuffPower;
    public int AbilityChance;

    public void Initialize(IDamageable owner, Transform startPosisition, AbilityData data)
    {
        Owner = owner;
        StartPosition = startPosisition;
        AbilityCooldawner = gameObject.AddComponent<AbilityCooldown>();
        abilityData = Instantiate(data);
        InitializeData();
        FeelLists();
        AbilityCooldawner.SetCooldawnDuration(CooldawnDuration);
    }

    private void InitializeData()
    {
        Logic = Instantiate(abilityData.Logic);
        Logic.Initialize(this);
        CountTargets = abilityData.CountTargets;
        HealPower = abilityData.HealPower;
        Damage = abilityData.Damage;
        ManaCost = abilityData.ManaCost;
        CritChance = abilityData.CritChance;
        CritDamage = abilityData.CritDamage;
        CastTime = abilityData.CastTime;
        AilmentChance = abilityData.AilmentChance;
        AilmentPower = abilityData.AilmentPower;
        CooldawnDuration = abilityData.CooldawnDuration;
        SelfTarget = abilityData.SelfTarget;
        BuffDuration = abilityData.BuffDuration;
        BuffPower = abilityData.BuffPower;
        AbilityChance = abilityData.AbilityChance;
        Description = abilityData.Description;
        AbilityName = abilityData.AbilityName;
        sprite = abilityData.sprite;
        Proj = abilityData.Proj;
        icon = abilityData.icon;
        TypeEffect = abilityData.TypeEffect;
        AbilityName = abilityData.AbilityName;
        SelfVFXPrefab = abilityData?.SelfVFXPrefab;
        TargetVFXPrefab = abilityData?.TargetVFXPrefab;
        DurationVFXPrefab = abilityData?.DurationVFXPrefab;
    }


    public void Cast()
    {
        Logic.Activate(Targets);
    }

    private void FeelLists()
    {
        ListTags = new List<AbilityTag>();
        ListTypes = new List<AbilityType>();

        foreach (AbilityTag tag in Enum.GetValues(typeof(AbilityTag)))
        {
            if (tag == AbilityTag.None)
                continue;

            if (abilityData.Tags.HasFlag(tag))
                ListTags.Add(tag);
        }

        foreach (AbilityType type in Enum.GetValues(typeof(AbilityType)))
        {
            if (type == AbilityType.None)
                continue;

            if (abilityData.AbilityTypes.HasFlag(type))
                ListTypes.Add(type);
        }
    }
}
