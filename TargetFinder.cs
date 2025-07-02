using System.Collections.Generic;
using UnityEngine;

public class TargetFinder : MonoBehaviour
{
    [SerializeField] BattleManager battleManager;

    private IDamageable _character;

    private void OnEnable()
    {
        _character = GetComponent<IDamageable>();
    }

    public List<IDamageable> GetTarget(IDamageable character, Ability ability)
    {
        TeamSide side;

        if (ability.AbilityTypes.HasFlag(AbilityType.Damage) || ability.AbilityTypes.HasFlag(AbilityType.Debuff))
        {
            side = character.GetSide();
        }
        else
        {
            side = (TeamSide)((byte)character.GetSide() ^ 1);
        }

        List<IDamageable> targets = battleManager.GetRandomTargets(side, ability.CountTargets);

        return targets;
    }
}
