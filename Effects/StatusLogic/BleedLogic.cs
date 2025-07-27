using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Bleed Logic")]
public class BleedLogic : EffectLogic
{
    public override void OnTick(IDamageable target, int power)
    {
        Debug.Log((float)target.Stats.GetStat(StatType.MaxHealth) + " / 100 * " + (float)power);
        float totalDamage = (float)target.Stats.GetStat(StatType.MaxHealth) / 100f * (float)power;
        target.TakeDamage(power);
        Debug.Log(target.Name + " получил " +  totalDamage);
    }
}
