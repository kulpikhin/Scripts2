using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour, IDamageable
{
    [SerializeField] private string _name;
    [SerializeField] public TeamSide Side;
    [SerializeField] public CharacterAbilities abilities;

    private CharacterAnimator _animator;
    private RegenRoutine _regenRoutine;

    public event UnityAction<IDamageable> Died;

    public UnityEngine.Transform StartSpellPosition;
    public CharacterClass Class;
    public CastManager castManager;
    public SubClass characterSubClass;

    public UnityEngine.Transform _transform { get; set; }
    public bool IsDead { get; set; }
    public CharacterAbilities Abilities { get; set; }

    private Dictionary<StatType, float> BaseStats;

    public string Name { get; set; }
    public EffectContainer Container { get; set; }
    public StatContainer Stats { get; set; }
    public ConditionManager Condition { get; private set; }

    private void OnEnable()
    {
        _regenRoutine = GetComponent<RegenRoutine>();
        _regenRoutine.SetCharecter(this);
        _transform = transform;
        Abilities = abilities;
        FillBaseStats();
        _animator = GetComponent<CharacterAnimator>();
        Stats = GetComponent<StatContainer>();
        Stats.SetStats(BaseStats);
        Stats.SetCurrentStats();
        Stats.OnDead += OnDeath;
        Name = _name;
        Container = GetComponent<EffectContainer>();
        Condition = GetComponent<ConditionManager>();
    }

    private void FillBaseStats()
    {
        BaseStats = new Dictionary<StatType, float>()
        {
            [StatType.DamageTakenIncreas] = Class.DamageIncreas,
            [StatType.Armor] = Class.Armor,
            [StatType.DamageIncreas] = Class.DamageIncreas,
            [StatType.DamageMore] = Class.DamageMore,
            [StatType.MaxHealth] = Class.MaxHealth,
            [StatType.CurrentHealth] = Class.CurrentHealth,
            [StatType.Speed] = Class.Speed,
            [StatType.CritChance] = Class.CritChance,
            [StatType.CritDamage] = Class.CritDamage,
            [StatType.LifeSteal] = Class.LifeSteal,
            [StatType.DodgeRaiting] = Class.DodgeRaiting,
            [StatType.MaxMana] = Class.MaxMana,
            [StatType.CurrentMana] = Class.CurrentMana,
            [StatType.CCResistance] = Class.CCResistance,
            [StatType.CDSpeed] = Class.CDSpeed,
            [StatType.HPRegen] = Class.HPRegen,
            [StatType.ManaRegen] = Class.ManaRegen,
            [StatType.HealPower] = Class.HealPower,
            [StatType.PetHealth] = Class.PetHealth,
            [StatType.PetDamage] = Class.PetDamage,
            [StatType.AilmentChance] = Class.AilmentChance,
            [StatType.AilmentPower] = Class.AilmentPower,
            [StatType.AilmentDuration] = Class.AilmentDuration,
        };
    }

    public TeamSide GetSide()
    {
        TeamSide side = new TeamSide();
        side = Side;
        return side;
    }

    private void OnDisable()
    {
        Stats.OnDead -= OnDeath;
    }

    public void TakeSwing()
    {
        _animator.TakeSwing();
    }

    public void Heal(int power)
    {
        Stats.Heal(power);
    }

    public void ApplyEffect(EffectInstance effect)
    {
        Container.ApplyEffect(effect);
    }

    public void StopActions()
    {
        castManager.StopCasting((IDamageable)this);
    }

    public void TakeDamage(int damage)
    {
        //Debug.Log("Damage = " + damage);
        Stats.TakeDamage(damage);        
    }

    private void OnDeath()
    {
        Debug.Log(Name + " dead");
        Died?.Invoke(this);
    }
}
