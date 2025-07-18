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
        float armorResist = target.Stats.GetStat(StatType.Armor) / 100;
        float finalDamage = ability.Damage + ((float)ability.Damage / 100f * target.Stats.GetStat(StatType.DamageIncreas));
        int trueDamage = Convert.ToInt32(finalDamage * (1f - armorResist));
        target.TakeDamage(trueDamage);
        ApplyAilment(ability, target, trueDamage);
    }

    private static void HandleHeal(Ability ability, IDamageable target)
    {
        target.Stats.Heal(ability.HealPower);
    }

    private static void HandleBuff(Ability ability, IDamageable target)
    {
        Debug.Log("add buff");
        target.Container.ApplyEffect(new EffectInstance(ability.TypeEffect, ability.HealPower, EffectDatas.GetEffectData(ability.TypeEffect).RefreshMode, ability.AilmentDuration));
    }
    
    private static void HandleDebuff(Ability ability, IDamageable target)
    {
        Debug.Log("Debuff");
    }

    private static int CalculateEffectChance(float abilityEffect, float characterEffect)
    {
        return Convert.ToInt32((abilityEffect + ((abilityEffect / 100 * characterEffect))));
    }

    private static float CalculatePower(EffectType type, Ability ability, float trueDamage)
    {
        float totalPower = (float)EffectDatas.GetEffectData(type).power / 100f * (ability.AilmentPower + ability._character.Stats.GetStat(StatType.AilmentPower)) + EffectDatas.GetEffectData(type).power;
        return trueDamage / 100f * totalPower;
    }

    private static float CalculateDuration(EffectType type, Ability ability)
    {
        float increaseDuration = ability.AilmentDuration + ability._character.Stats.GetStat(StatType.AilmentDuration);
        return EffectDatas.GetEffectData(type).Duration / 100 * increaseDuration + EffectDatas.GetEffectData(type).Duration;
    }

    private static void CalculateAilment(Ability ability, float trueDamage, IDamageable target, EffectType effectType)
    {
        float roll = UnityEngine.Random.Range(0, 100);

        if (roll <= CalculateEffectChance(ability.AilmentChance, ability._character.Stats.GetStat(StatType.AilmentChance)))
        {
            int power = Convert.ToInt32(CalculatePower(effectType, ability, trueDamage));
            float duration = CalculateDuration(effectType, ability);

            if (power > 0)
            {
                target.Container.ApplyEffect(new EffectInstance(effectType, power, EffectDatas.GetEffectData(effectType).RefreshMode, duration));
            }
        }
    }

    private static void ApplyAilment(Ability ability, IDamageable target, float trueDamage)
    {
        if (ability.Tags.HasFlag(AbilityTag.Fire))
        {
            CalculateAilment(ability, trueDamage, target, EffectType.Ignite);
        }
        if (ability.Tags.HasFlag(AbilityTag.Cold))
        {
            CalculateAilment(ability, trueDamage, target, EffectType.Chill);
        }
        if (ability.Tags.HasFlag(AbilityTag.Lightning))
        {
            CalculateAilment(ability, trueDamage, target, EffectType.Shock);
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

    public static void SetDataBase(EffectDataBase data)
    {
        EffectDatas = data;
    }
}
