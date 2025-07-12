using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

//[RequireComponent(typeof(AbilityCooldown))]
public class Ability : MonoBehaviour
{
    public AbilityType AbilityTypes;
    public AbilityTag Tags;
    public List<AbilityTag> ListTags;
    public List<AbilityType> ListTypes;

    public List<IDamageable> Targets;

    public Projectile Proj;
    public Transform StartPosition;

    public int CountTargets;
    public IconAbility icon;
    public Sprite sprite;
    public string AbilityName;
    public string Description;
    public int HealPower;
    public int Damage;
    public int ManaCost;
    public float CritChance;
    public float CritDamage;
    public float CastTime;
    public float AilmentChance;
    public float AilmentPower;
    public float AilmentDuration;
    //public ParticleSystem VFXPrefab;

    public Character _character;

    public AbilityCooldown CooldownAbility { get; private set; }

    public void Init()
    {
        CooldownAbility = GetComponent<AbilityCooldown>();
        Targets = new List<IDamageable>();
        FeelLists();
    }

    public virtual void Activate()
    {
        if (Targets.Count > 0)
        {
            if (Tags.HasFlag(AbilityTag.Projectile))
            {
                AvtivateProjectile();
            }
            else
            {
                foreach (IDamageable target in Targets)
                {
/*                    if (VFXPrefab != null)
                    {
                        Debug.Log("vfx");
                        ParticleSystem vfx = Instantiate(VFXPrefab, target._transform);
                        VFXPrefab.Play();
                    }*/

                    CalculateEffect(target);
                }
            }

            CooldownAbility.StartCooldawn(this);
        }
    }

    public virtual void AvtivateProjectile()
    {
        List<Projectile> Projectiles = new List<Projectile>();

        foreach (IDamageable target in Targets)
        {
            Projectile projectile = Instantiate(Proj, _character._transform);
/*
            if (_character.Side == TeamSide.Right)
            {
                projectile.transform.localScale = new Vector2(projectile.transform.localScale.x * (-1), projectile.transform.localScale.y);
            }
*/
            projectile.transform.position = new Vector2(StartPosition.position.x, StartPosition.position.y);
            Vector2 direction = target._transform.position - projectile.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.Euler(0, 0, angle);
            Projectiles.Add(projectile);
            projectile.EndFlying += CalculatePrjectileEffect;
            projectile.Launch(StartPosition.position, target);
        }
    }

    private void CalculateEffect(IDamageable target)
    {
        AbilityEffectCalculator.CalculateEffect(this, target);

        if (Tags.HasFlag(AbilityTag.Melee))
        {
            target.TakeSwing();
        }
    }

    private void CalculatePrjectileEffect(IDamageable target, Projectile proj)
    {
        if (!target.IsDead)
        {
            AbilityEffectCalculator.CalculateEffect(this, target);
            proj.EndFlying -= CalculatePrjectileEffect;
        }
    }

    private void FeelLists()
    {
        foreach (AbilityTag tag in Enum.GetValues(typeof(AbilityTag)))
        {
            if (tag == AbilityTag.None)
                continue;

            if (Tags.HasFlag(tag))
                ListTags.Add(tag);
        }

        foreach (AbilityType type in Enum.GetValues(typeof(AbilityType)))
        {
            if (type == AbilityType.None)
                continue;

            if (AbilityTypes.HasFlag(type))
                ListTypes.Add(type);
        }
    }
}
