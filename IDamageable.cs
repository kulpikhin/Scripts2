using UnityEngine;
using UnityEngine.Events;

public interface IDamageable
{
    public event UnityAction<IDamageable> Died;

    public string Name { get; set; }
    public StatContainer Stats { get; set; }
    public EffectContainer Container { get; set; }
    public CharacterAbilities Abilities { get; set; }
    public bool IsDead { get; set; }
    public Transform _transform { get; set; }

    public void SetTansform()
    {
    }

    public void TakeDamage(int damage)
    {
    }

    public void Heal(int power)
    {
    }

    public void TakeSwing()
    {        
    }

    public void ApplyEffect(EffectInstance effect)
    {
        Container.ApplyEffect(effect);
    }

    public TeamSide GetSide()
    {
        TeamSide side = new TeamSide();
        return side;
    }
}
