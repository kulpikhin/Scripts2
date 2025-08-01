using UnityEngine;

public class EffectLogic : ScriptableObject
{
    private ParticleSystem VFXPrefab;

    public virtual void OnApply(IDamageable target, int power, EffectData data)
    {
        if (data.VFXPrefab != null)
        {
            var vfxInstance = Instantiate(data.VFXPrefab, target._transform);
            vfxInstance.Play();
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
