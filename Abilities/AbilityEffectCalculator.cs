using UnityEngine;
using System;
using System.Collections.Generic;

public static class AbilityEffectCalculator
{
    public static EffectDataBase EffectDatas;

    private static readonly Dictionary<AbilityType, Action<Ability, IDamageable>> abilityActions
        = new Dictionary<AbilityType, Action<Ability, IDamageable>>
    {
        { AbilityType.Damage, HandleDamage },
        { AbilityType.Heal, HandleHeal },
        { AbilityType.Buff, HandleBuff },
        { AbilityType.Debuff, HandleDebuff }
    };

    public static void CalculateEffect(Ability ability, IDamageable target)
    {
        foreach (AbilityType type in ability.ListTypes)
        {
            if (abilityActions.TryGetValue(type, out Action<Ability, IDamageable> action))
            {
                action.Invoke(ability, target);
            }
            else
            {
                Debug.LogWarning($"Нет обработчика для типа: {type}");
            }
        }
    }

    private static void HandleDamage(Ability ability, IDamageable target)
    {
        int armorResist = Convert.ToInt32(target.Stats.GetStat(StatType.Armor)) / 100;
        int finalDamage = Convert.ToInt32 (((float)ability.Damage) * (1 - (float)armorResist));
        target.TakeDamage(finalDamage);
        ApplyAilment(ability, target);
    }

    private static void HandleHeal(Ability ability, IDamageable target)
    {
        target.Stats.Heal(ability.HealPower);
    }

    private static void HandleBuff(Ability ability, IDamageable target)
    {
        Debug.Log("Buff");
    }

    private static void HandleDebuff(Ability ability, IDamageable target)
    {
        Debug.Log("Debuff");
    }

    private static void ApplyAilment(Ability ability, IDamageable target)
    {
        if(ability.AilmentChance > 0)
        {
            if (ability.Tags.HasFlag(AbilityTag.Fire))
            {
                float chance = UnityEngine.Random.Range(0, 100);

                if(chance <= ability.AilmentChance)
                {
                    Debug.Log(ability.name + " накладывает поджог");

                    float totalPower = (EffectDatas.GetEffectData(EffectType.Ignite).power / 100) * ability.AilmentPower;
                    int power = Convert.ToInt32((float) ability.Damage / 100 * (totalPower + EffectDatas.GetEffectData(EffectType.Ignite).power));

                    float duration = (EffectDatas.GetEffectData(EffectType.Ignite).Duration / 100) * (ability.AilmentDuration + 100);

                    target.Container.ApplyEffect(new EffectInstance(EffectType.Ignite, power, EffectDatas.GetEffectData(EffectType.Ignite).RefreshMode, duration));
                }
            }
            if (ability.Tags.HasFlag(AbilityTag.Cold))
            {
                float chance = UnityEngine.Random.Range(0, 100);

                if (chance <= ability.AilmentChance)
                {
                    Debug.Log(ability.name + " накладывает охлаждение");

                    float totalPower = (EffectDatas.GetEffectData(EffectType.Chill).power / 100) * ability.AilmentPower;
                    int power = Convert.ToInt32((float) ability.Damage /100 * (totalPower + EffectDatas.GetEffectData(EffectType.Chill).power));

                    float duration = (EffectDatas.GetEffectData(EffectType.Chill).Duration / 100) * ability.AilmentDuration + EffectDatas.GetEffectData(EffectType.Chill).Duration;

                    target.Container.ApplyEffect(new EffectInstance(EffectType.Chill, power, EffectDatas.GetEffectData(EffectType.Chill).RefreshMode, duration));
                }
            }
            /*i if (ability.Tags.HasFlag(AbilityTag.Lightning))
            {
                float chance = UnityEngine.Random.Range(ability.AilmentChance, 100);

                if (chance >= ability.AilmentChance)
                {
                    target.Container.ApplyEffect(new EffectInstance(EffectType.Shock, ability.AilmentPower, EffectDatas, ability.AilmentDuration));
                }
            }
            if (ability.Tags.HasFlag(AbilityTag.Poison))
            {
                float chance = UnityEngine.Random.Range(ability.AilmentChance, 100);

                if (chance >= ability.AilmentChance)
                {
                    target.Container. Effect(new EffectInstance(EffectType.Poison, ability.AilmentPower, EffectDatas, ability.AilmentDuration));
                }
            }
            if (ability.Tags.HasFlag(AbilityTag.Bleed))
            {
                float chance = UnityEngine.Random.Range(ability.AilmentChance, 100);

                if (chance >= ability.AilmentChance)
                {
                    target.Container.ApplyEffect(new EffectInstance(EffectType.Bleed, ability.AilmentPower, EffectDatas, ability.AilmentDuration));

                }
            }
            */
        }
    }

    public static void SetDataBase(EffectDataBase data)
    {
        EffectDatas = data;
    }
}
