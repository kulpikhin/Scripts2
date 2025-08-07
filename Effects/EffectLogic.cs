using UnityEngine;

public class EffectLogic : ScriptableObject
{
    private ParticleSystem VFXPrefab;

    public virtual void OnApply(IDamageable target, int power, EffectData data)
    {

    }

    public virtual void OnTick(IDamageable target, int power)
    {

    }

    public virtual void OnExpired(IDamageable target, int power)
    {

    }
}
