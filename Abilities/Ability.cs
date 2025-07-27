using System;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    private AbilityData abilityData;
    public AbilityCooldown AbilityCooldawner { get; private set;}
    public Character Owner { get; private set; }
    public AbilityData AbilityDatas => abilityData;
    public Transform StartPosition { get; private set; }
    public List<AbilityType> Types { get; private set; }
    public List<AbilityTag> ListTags;
    public List<AbilityType> ListTypes;

    public void Initialize(Character owner, Transform startPosisition, AbilityData data)
    {
        Owner = owner;
        Transform StartPosition = startPosisition;
        AbilityCooldawner = gameObject.AddComponent<AbilityCooldown>();
        AbilityCooldawner.SetCooldawnDuration(data.CooldawnDuration);
        abilityData = data;
        FeelLists();
        data.StartPosition = StartPosition;
        abilityData.Logic.Initialize(this);
    }

    public void Cast()
    {
        abilityData.Logic.Activate(abilityData.Targets, abilityData);
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
