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

        if (ability.AbilityDatas.Tags.HasFlag(AbilityTag.Damage) || ability.AbilityDatas.Tags.HasFlag(AbilityTag.Debuff))
        {
            side = character.GetSide();
        }
        else
        {
            side = (TeamSide)((byte)character.GetSide() ^ 1);
        }

        List<IDamageable> targets = battleManager.GetRandomTargets(side, ability.AbilityDatas.CountTargets);

        return targets;
    }

    public List<IDamageable> GetTarget(TeamSide sideTarget, int countTargets)
    {
        List<IDamageable> targets = battleManager.GetRandomTargets(sideTarget, countTargets);

        return targets;
    }
}
