using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Poison Logic")]

public class PoisonLogic : EffectLogic
{
    public override void OnTick(IDamageable target, int power)
    {
        target.TakeDamage(power);
    }
}
