using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/EffectDataBase")]

public class EffectDataBase : ScriptableObject
{
    public EffectData[] effectDatas;

    public Dictionary<EffectType, EffectData> effectBase = new Dictionary<EffectType, EffectData>();

    public void OnEnable()
    {
        FillData();
    }

    public void FillData()
    {
        foreach (EffectData data in effectDatas)
        {
            effectBase.Add(data.effectType, data);
        }

        AbilityEffectCalculator.SetDataBase(this);
    }

    public EffectData GetEffectData(EffectType effectType)
    {
        EffectData foundedEffectData;

        if(effectBase.TryGetValue(effectType, out EffectData effectData))
        {
            foundedEffectData = effectData;
        }
        else
        {
            foundedEffectData = null;
            Debug.Log("effectdata = null");
        }

        return foundedEffectData;
    }
}
