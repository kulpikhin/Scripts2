using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Ignie Logic")]
public class IgniteLogic : EffectLogic
{
    public override void OnTick(IDamageable target, int power)
    {
        target.TakeDamage(power) ;
    }
}
