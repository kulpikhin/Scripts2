using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/WindFury Logic")]

public class WindFuryEffectLogic : EffectLogic
{
    public override void OnApply(IDamageable target, int power, EffectData data)
    {
        if (target != null)
        {
            target.castManager.CastEnd += TriggerWindFury;
        }
    }

    private void TriggerWindFury(Ability ability)
    {
/*        float roll = UnityEngine.Random.Range(0, 100);

        if (ability.AbilityDatas.Tags.HasFlag(AbilityTag.Attack))
        {

            if (_effectData.VFXPrefab != null)
            {
                var vfxInstance = Instantiate(_effectData.VFXPrefab, _owner._transform);
                vfxInstance.Play();
            }

            List<IDamageable> targets = _owner.castManager._targetFinder.GetTarget(_owner.GetSide(), _effectData.CountTargets);

            foreach (IDamageable target in new List<IDamageable>(targets))
            {
                AbilityEffectCalculator.CalculateEffect(_ability, target);
            }

        }*/
    }

    public override void OnExpired(IDamageable target, int power)
    {
        base.OnExpired(target, power);

        if (target != null)
        {
            target.castManager.CastEnd -= TriggerWindFury;
        }
    }
}
