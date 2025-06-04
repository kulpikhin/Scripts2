using System.Collections.Generic;
using UnityEngine;

public class AbilityDataBase : ScriptableObject
{
    public AbilityData[] abilityData;

    public Dictionary<AbilityName, AbilityData> abilityBases = new Dictionary<AbilityName, AbilityData>();

    public void OnEnable()
    {
        FillData();
    }

    public void FillData()
    {
        foreach (AbilityData data in abilityData)
        {
            abilityBases.Add(data.abilityName, data);
        }
    }

    public AbilityData GetAbilityData(AbilityName name)
    {
        AbilityData foundedBilityData;

        if (abilityBases.TryGetValue(name, out AbilityData abilityData))
        {
            foundedBilityData = abilityData;
        }
        else
        {
            foundedBilityData = null;
            Debug.Log("effectdata = null");
        }

        return foundedBilityData;
    }
}
