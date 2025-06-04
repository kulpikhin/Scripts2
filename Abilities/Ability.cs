using System;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

//[RequireComponent(typeof(AbilityCooldown))]
public class Ability : MonoBehaviour
{
    public AbilityType AbilityTypes;
    public AbilityTag Tags;
    public List<AbilityTag> ListTags;
    public List<AbilityType> ListTypes;

    public List<IDamageable> Targets;

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

    public Character _character;

    public AbilityCooldown CooldownAbility {get; private set;}

    public void Init()
    {
        CooldownAbility = GetComponent<AbilityCooldown>();
        Targets = new List<IDamageable>();
        FeelLists();
    }

    public virtual void Activate() 
    {
        if(Targets.Count > 0)

        {
            foreach (IDamageable target in Targets)
            {
                Debug.Log(_character.Name + " использует способность на " + target.Name);
                AbilityEffectCalculator.CalculateEffect(this, target);
            }

            CooldownAbility.StartCooldawn(this);
        }    
    }

    private void FeelLists()
    {
        foreach (AbilityTag tag in Enum.GetValues(typeof(AbilityTag)))
        {
            if (tag == AbilityTag.None)
                continue;

            if (Tags.HasFlag(tag))
                ListTags.Add(tag);
        }

        foreach (AbilityType type in Enum.GetValues(typeof(AbilityType)))
        {
            if (type == AbilityType.None)
                continue;

            if (AbilityTypes.HasFlag(type))
                ListTypes.Add(type);
        }
    }
}
