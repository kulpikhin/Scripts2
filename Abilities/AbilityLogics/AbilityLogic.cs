using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(menuName = "Abilities/Logic")]
public class AbilityLogic : ScriptableObject
{
    protected Ability ability;

    private ParticleSystem VFXInstance;

    public void Initialize(Ability ability)
    {
        this.ability = ability;
    }

    public void TrackEffectInstance(EffectInstance instance)
    {
        instance.OnAply += OnEffectApplied;
        instance.OnExpired += OnEffectExpired;
    }

    protected virtual void OnEffectApplied(EffectInstance instance)
    {
        Debug.Log("on apply");

        if (ability.DurationVFXPrefab != null)
        {
            foreach (IDamageable target in ability.Targets)
            {
                VFXInstance = Instantiate(ability.DurationVFXPrefab, target._transform);
                VFXInstance.Play();
            }
        }
    }

    protected virtual void OnEffectExpired(EffectInstance instance)
    {
        if (VFXInstance != null)
        {
            VFXInstance.Stop();
            Destroy(VFXInstance.gameObject);
            VFXInstance = null;
        }

        instance.OnAply -= OnEffectApplied;
        instance.OnExpired -= OnEffectExpired;
    }

    public virtual void Activate(List<IDamageable> targets)
    {
        if (targets.Count > 0)
        {
            if (ability.ListTags.Contains(AbilityTag.Projectile)) // åñëè åñòü ñíàðÿä, òî íåò VFX
            {
                AñtivateProjectile(targets);
            }
            else
            {
                if (ability.SelfVFXPrefab != null)
                {
                    InstantiateVFX(ability.Owner, ability.SelfVFXPrefab);
                }

                foreach (IDamageable target in new List<IDamageable>(targets))
                {
                    if (ability.TargetVFXPrefab != null)
                    {
                        InstantiateVFX(target, ability.TargetVFXPrefab);
                    }

                    CalculateEffect(target);
                }

            }

            if (ability.CooldawnDuration > 0)
            {
                ability.AbilityCooldawner.StartCooldawn(ability);
            }
        }
    }

    protected virtual void InstantiateVFX(IDamageable target, ParticleSystem VFXPrefab)
    {
        Debug.Log("No VFX");

        Vector3 place = new Vector3 (target._transform.position.x, target._transform.position.y, target._transform.position.z);

        ParticleSystem vfx = Instantiate(VFXPrefab, place, Quaternion.Euler(0, 0, 0));
        vfx.Play();
    }

    public virtual void AñtivateProjectile(List<IDamageable> Targets)
    {
        List<Projectile> Projectiles = new List<Projectile>();

        foreach (IDamageable target in Targets)
        {
            Projectile projectile = Instantiate(ability.Proj, ability.Owner._transform);

            projectile.transform.position = new Vector2(ability.StartPosition.position.x, ability.StartPosition.position.y);
            Vector2 direction = target._transform.position - projectile.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.Euler(0, 0, angle);
            Projectiles.Add(projectile);
            projectile.EndFlying += CalculatePrjectileEffect;
            projectile.Launch(ability.StartPosition.position, target);
        }
    }

    private void CalculateEffect(IDamageable target)
    {
        AbilityEffectCalculator.CalculateEffect(ability, target);

        if (ability.AbilityDatas.Tags.HasFlag(AbilityTag.Melee))
        {
            target.TakeSwing();
        }
    }

    private void CalculatePrjectileEffect(IDamageable target, Projectile proj)
    {
        if (!target.IsDead)
        {
            AbilityEffectCalculator.CalculateEffect(ability, target);
            proj.EndFlying -= CalculatePrjectileEffect;
        }
    }
}