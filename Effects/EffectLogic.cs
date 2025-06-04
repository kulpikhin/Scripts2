using UnityEngine;

public class EffectLogic : ScriptableObject
{
    public virtual void OnApply(IDamageable target, int power)
    {

    }

    public virtual void OnTick(IDamageable target, int power)
    {

    }

    public virtual void OnExpired(IDamageable target, int power)
    {
        
    }
}
