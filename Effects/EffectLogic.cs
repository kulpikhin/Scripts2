using UnityEngine;

public class EffectLogic : ScriptableObject
{
    private ParticleSystem VFXPrefab;

    public virtual void OnApply(IDamageable target, int power, EffectData data)
    {
        if (data.VFXPrefab != null)
        {
            VFXPrefab = Instantiate(data.VFXPrefab, target._transform);
            VFXPrefab.Play();
        }
    }

    public virtual void OnTick(IDamageable target, int power)
    {

    }

    public virtual void OnExpired(IDamageable target, int power)
    {
        if (VFXPrefab != null)
        {
            VFXPrefab.Stop();
            VFXPrefab = null;
        }
    }
}
