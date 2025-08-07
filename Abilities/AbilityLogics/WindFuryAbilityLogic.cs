using UnityEngine;

[CreateAssetMenu(menuName = "Logic/WindFuryAbilityLogic")]

public class WindFuryAbilityLogic : AbilityLogic
{
    [SerializeField] private AbilityData _windProcData;

    private Ability abilityWindProc;

    protected override void OnEffectApplied(EffectInstance instance)
    {
        base.OnEffectApplied(instance);

        ability.Owner.castManager.CastEnd += OnAttackCast;

        abilityWindProc = ability.gameObject.AddComponent<Ability>();
        abilityWindProc.Initialize(ability.Owner, ability.Owner.StartSpellPosition, _windProcData);
    }

    private void OnAttackCast(Ability abil)
    {
        if (abil.ListTags.Contains(AbilityTag.Attack))
        {
            ability.Owner.castManager.CastProcAbility(abilityWindProc);
        }
    }

    protected override void OnEffectExpired(EffectInstance instance)
    {
        base.OnEffectExpired(instance);
        Destroy(abilityWindProc);
        ability.Owner.castManager.CastEnd -= OnAttackCast;
    }
}
