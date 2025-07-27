using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Logic")]
public class AbilityLogic : ScriptableObject
{
    private Ability ability;

    public void Initialize(Ability ability)
    {
        this.ability = ability;
    }

    public virtual void Activate(List<IDamageable> targets, AbilityData data)
    {
        if (targets.Count > 0)
        {
            if (data.Tags.HasFlag(AbilityTag.Projectile))
            {
                AvtivateProjectile(targets, data);
            }
            else
            {
                foreach (IDamageable target in targets)
                {
                    if (data.VFXPrefab != null)
                    {
                        Vector3 place;

                        if (data.SelfAnimation)
                        {
                            place = ability.Owner.transform.position;
                        }
                        else
                        {
                            place = target._transform.position;
                        }

                        ParticleSystem vfx = Instantiate(data.VFXPrefab, place - new Vector3(0, 2, 0), Quaternion.Euler(-100, 0, 0));
                        vfx.Play();
                    }

                    CalculateEffect(target);
                }
            }

            ability.AbilityCooldawner.StartCooldawn(ability);
        }
    }

    public virtual void AvtivateProjectile(List<IDamageable> Targets, AbilityData data)
    {
        List<Projectile> Projectiles = new List<Projectile>();

        foreach (IDamageable target in Targets)
        {
            Projectile projectile = Instantiate(data.Proj, ability.Owner._transform);

            projectile.transform.position = new Vector2(data.StartPosition.position.x, data.StartPosition.position.y);
            Vector2 direction = target._transform.position - projectile.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.Euler(0, 0, angle);
            Projectiles.Add(projectile);
            projectile.EndFlying += CalculatePrjectileEffect;
            projectile.Launch(data.StartPosition.position, target);
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