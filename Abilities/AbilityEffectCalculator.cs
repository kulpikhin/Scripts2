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
        float finalDamage = ability.AbilityDatas.Damage + ((float)ability.AbilityDatas.Damage / 100f * ability.Owner.Stats.GetStat(StatType.DamageIncreas));
        float toArmoreDamage = finalDamage * (1f - armorResist);
        int trueDamage = Convert.ToInt32(toArmoreDamage / 100 * target.Stats.GetStat(StatType.DamageTakenIncreas) + toArmoreDamage);

        if(ability.AbilityDatas.Name == "WindProc")
        {
            Debug.Log("WindFury Deal " + trueDamage + " Damage");
        }

        target.TakeDamage(trueDamage);
        ApplyAilment(ability, target, trueDamage);
    }

    private static void HandleHeal(Ability ability, IDamageable target)
    {
        target.Stats.Heal(ability.AbilityDatas.HealPower);
    }

    private static void HandleBuff(Ability ability, IDamageable target)
    {
        Debug.Log("add buff");
        target.Container.ApplyEffect(new EffectInstance(ability.AbilityDatas.TypeEffect, ability.AbilityDatas.BuffPower, EffectDatas.GetEffectData(ability.AbilityDatas.TypeEffect).RefreshMode, ability.AbilityDatas.BuffDuration));
    }

    private static void HandleDebuff(Ability ability, IDamageable target)
    {
        Debug.Log("Debuff");
        target.Container.ApplyEffect(new EffectInstance(ability.AbilityDatas.TypeEffect, ability.AbilityDatas.BuffPower, EffectDatas.GetEffectData(ability.AbilityDatas.TypeEffect).RefreshMode, ability.AbilityDatas.BuffDuration));
    }

    private static int CalculateEffectChance(float abilityEffect, float characterEffect)
    {
        return Convert.ToInt32((abilityEffect + ((abilityEffect / 100 * characterEffect))));
    }

    private static float CalculatePower(EffectType type, Ability ability, float trueDamage)
    {
        float totalPower = (float)EffectDatas.GetEffectData(type).power / 100f * (ability.AbilityDatas.AilmentPower + ability.Owner.Stats.GetStat(StatType.AilmentPower)) + EffectDatas.GetEffectData(type).power;
        return trueDamage / 100f * totalPower;
    }

    private static float CalculatePower(EffectType type, Ability ability)
    {
        return (float)EffectDatas.GetEffectData(type).power / 100f * (ability.AbilityDatas.AilmentPower + ability.Owner.Stats.GetStat(StatType.AilmentPower)) + EffectDatas.GetEffectData(type).power;
    }

    private static float CalculatePoisonPower(Ability ability)
    {
        return ability.AbilityDatas.AilmentPower / 100 * ability.Owner.Stats.GetStat(StatType.AilmentPower) + ability.AbilityDatas.AilmentPower;
    }

    private static float CalculateDuration(EffectType type, Ability ability)
    {
        float increaseDuration = ability.AbilityDatas.AilmentDuration + ability.Owner.Stats.GetStat(StatType.AilmentDuration);
        return EffectDatas.GetEffectData(type).Duration / 100 * increaseDuration + EffectDatas.GetEffectData(type).Duration;
    }

    private static void CalculateAilment(Ability ability, float trueDamage, IDamageable target, EffectType effectType)
    {
        float roll = UnityEngine.Random.Range(0, 100);

        if (roll <= CalculateEffectChance(ability.AbilityDatas.AilmentChance, ability.Owner.Stats.GetStat(StatType.AilmentChance)))
        {
            int power = Convert.ToInt32(CalculatePower(effectType, ability, trueDamage));
            float duration = CalculateDuration(effectType, ability);

            if (power > 0)
            {
                target.Container.ApplyEffect(new EffectInstance(effectType, power, EffectDatas.GetEffectData(effectType).RefreshMode, duration));
            }
        }
    }

    private static void CalculateAilment(Ability ability, IDamageable target, EffectType effectType)
    {
        float roll = UnityEngine.Random.Range(0, 100);

        if (roll <= CalculateEffectChance(ability.AbilityDatas.AilmentChance, ability.Owner.Stats.GetStat(StatType.AilmentChance)))
        {
            int power;

            if (effectType == EffectType.Poison)
            {
                power = Convert.ToInt32(CalculatePoisonPower(ability));
            }
            else
            {
                power = Convert.ToInt32(CalculatePower(effectType, ability));
            }

            float duration = CalculateDuration(effectType, ability);

            if (power > 0)
            {
                target.Container.ApplyEffect(new EffectInstance(effectType, power, EffectDatas.GetEffectData(effectType).RefreshMode, duration));
            }
        }
    }

    private static void ApplyAilment(Ability ability, IDamageable target, float trueDamage)
    {
        if (ability.AbilityDatas.Tags.HasFlag(AbilityTag.Fire))
        {
            CalculateAilment(ability, trueDamage, target, EffectType.Ignite);
        }
        if (ability.AbilityDatas.Tags.HasFlag(AbilityTag.Cold))
        {
            CalculateAilment(ability, trueDamage, target, EffectType.Chill);
        }
        if (ability.AbilityDatas.Tags.HasFlag(AbilityTag.Lightning))
        {
            CalculateAilment(ability, trueDamage, target, EffectType.Shock);
        }
        if (ability.AbilityDatas.Tags.HasFlag(AbilityTag.Poison))
        {
            CalculateAilment(ability, target, EffectType.Poison);
            //target.Container.ApplyEffect(new EffectInstance(EffectType.Poison, Convert.ToInt32(ability.AbilityDatas.AilmentPower), EffectDatas.GetEffectData(EffectType.Poison).RefreshMode, EffectDatas.GetEffectData(ability.AbilityDatas.TypeEffect).Duration));
        }
        if (ability.AbilityDatas.Tags.HasFlag(AbilityTag.Bleed))
        {
            CalculateAilment(ability, target, EffectType.Bleed);
            //target.Container.ApplyEffect(new EffectInstance(EffectType.Bleed, Convert.ToInt32(ability.AbilityDatas.AilmentPower), EffectDatas.GetEffectData(EffectType.Bleed).RefreshMode, EffectDatas.GetEffectData(ability.AbilityDatas.TypeEffect).Duration));
        }
    }

    public static void SetDataBase(EffectDataBase data)
    {
        EffectDatas = data;
    }
}
