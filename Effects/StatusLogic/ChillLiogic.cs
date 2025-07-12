using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Chill Logic")]

public class ChillLiogic : EffectLogic
{
    public override void OnApply(IDamageable target, int power, EffectData data)
    {
        Debug.Log("Chill = " + power);
        target.Stats.AddStat(StatType.Speed, -power);

        Debug.Log("speed = " + target.Stats.GetStat(StatType.Speed));
    }

    public override void OnExpired(IDamageable target, int power)
    {  
        target.Stats.AddStat(StatType.Speed, power);
        Debug.Log("Chill Expire, speed = " + target.Stats.GetStat(StatType.Speed));
    }
}
