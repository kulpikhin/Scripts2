using UnityEngine;

public class EffectLogic : ScriptableObject
{
    private ParticleSystem VFXPrefab;

    public virtual void OnApply(IDamageable target, int power, EffectData data)
    {
        Debug.Log("Apply");
        
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
        Debug.Log("Expire");
        if (VFXPrefab != null)
        {
            VFXPrefab.Stop();
            VFXPrefab = null;
        }
    }
}
