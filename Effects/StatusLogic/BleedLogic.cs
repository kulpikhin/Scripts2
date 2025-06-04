using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Bleed Logic")]
public class BleedLogic : EffectLogic
{
    public override void OnTick(IDamageable target, int power)
    {
        target.TakeDamage(power);
    }
}
