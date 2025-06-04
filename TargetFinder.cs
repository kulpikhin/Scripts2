using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

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
            side = (TeamSide)((byte)character.GetSide() ^ 1);
        }
        else
        {
            side = character.GetSide();
        }

        List<IDamageable> targets = battleManager.GetRandomTargets(side, ability.CountTargets);

        return targets;
    }
}
