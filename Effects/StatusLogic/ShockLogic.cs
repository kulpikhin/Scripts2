using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Shock Logic")]

public class ShockLogic : EffectLogic
{
    public override void OnApply(IDamageable target, int power, EffectData data)
    {
        Debug.Log("Shock = " + power);
        target.Stats.AddStat(StatType.DamageTakenIncreas, power);

        Debug.Log("DamageTakenIncreas = " + target.Stats.GetStat(StatType.DamageTakenIncreas));
    }

    public override void OnExpired(IDamageable target, int power)
    {
        target.Stats.AddStat(StatType.DamageTakenIncreas, -power);
        Debug.Log("Shock Expire, DamageTakenIncreas = " + target.Stats.GetStat(StatType.DamageTakenIncreas));
    }
}
