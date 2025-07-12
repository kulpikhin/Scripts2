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
        float finalDamage = ability.Damage + (ability.Damage / 100 * target.Stats.GetStat(StatType.DamageIncreas));
        int trueDamage = Convert.ToInt32(target.Stats.GetStat(StatType.Armor) * (1 - (float)armorResist));
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
        target.Container.ApplyEffect(new EffectInstance(EffectType.Regenerate, ability.HealPower, EffectDatas.GetEffectData(EffectType.Ignite).RefreshMode, ability.AilmentDuration));
    }

    private static void HandleDebuff(Ability ability, IDamageable target)
    {
        Debug.Log("Debuff");
    }

    private static int CalculateEffect(float abilityEffect, float characterEffect)
    {
        return Convert.ToInt32((abilityEffect + ((abilityEffect / 100 * characterEffect)))); // добавить эту логику для просчёта силы эффекта
    }

    private static void CalculatePower(EffectType type, Ability ability)
    {

    }

    private static void ApplyAilment(Ability ability, IDamageable target, float trueDamage)
    {
        if (ability.Tags.HasFlag(AbilityTag.Fire)) // убрать - сделать без тегов, просчёт от конкретного урона( конвертации)
        {
            float roll = UnityEngine.Random.Range(0, 100);

            if (roll <= CalculateEffect(ability.AilmentChance, ability._character.Stats.GetStat(StatType.AilmentChance)))
            {
                Debug.Log(ability.name + " накладывает поджог");


                float totalPower = EffectDatas.GetEffectData(EffectType.Ignite).power / 100 * (ability.AilmentPower + ability._character.Stats.GetStat(StatType.AilmentPower)) + EffectDatas.GetEffectData(EffectType.Ignite).power));
                float power = trueDamage / 100f * totalPower;

                float duration = (EffectDatas.GetEffectData(EffectType.Ignite).Duration / 100) * (ability.AilmentDuration + 100);

                target.Container.ApplyEffect(new EffectInstance(EffectType.Ignite, Convert.ToInt32(power), EffectDatas.GetEffectData(EffectType.Ignite).RefreshMode, duration));
            }
        }
        if (ability.Tags.HasFlag(AbilityTag.Cold))
        {
            float roll = UnityEngine.Random.Range(0, 100);

            if (roll <= CalculateEffect(ability.AilmentChance, ability._character.Stats.GetStat(StatType.AilmentChance)))
            {
                Debug.Log(ability.name + " накладывает охлаждение");

                float totalPower = (EffectDatas.GetEffectData(EffectType.Chill).power / 100) * ability.AilmentPower;
                int power = Convert.ToInt32((float)ability.Damage / 100 * (totalPower + EffectDatas.GetEffectData(EffectType.Chill).power));

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

    public static void SetDataBase(EffectDataBase data)
    {
        EffectDatas = data;
    }
}
