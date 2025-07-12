using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Regenerate Logic")]

public class RegenerateLogic: EffectLogic
{
    public override void OnApply(IDamageable target, int power, EffectData data)
    {
        base.OnApply(target, power, data);
    }

    public override void OnTick(IDamageable target, int power)
    {
        target.Heal(power);
    }

    public override void OnExpired(IDamageable target, int power)
    {
        base.OnExpired(target, power);
    }
}
