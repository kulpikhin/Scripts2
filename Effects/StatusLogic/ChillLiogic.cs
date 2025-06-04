using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Chill Logic")]

public class ChillLiogic : EffectLogic
{
    public override void OnApply(IDamageable target, int power)
    {
        Debug.Log("Chill");
        target.Stats.AddStat(StatType.Speed, -power);
    }

    public override void OnExpired(IDamageable target, int power)
    {
        target.Stats.AddStat(StatType.Speed, power);
    }
}
