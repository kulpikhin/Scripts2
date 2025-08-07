using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Status Data")]

public class EffectData : ScriptableObject
{
    public EffectLogic Logic;
    public float Duration;
    public Sprite Icon;
    public RefreshEffectMode RefreshMode;
    public EffectType effectType;
    public EffectTegs effectTegs;
    public float power;
    public IDamageable Target;
    public bool IsStrongest;
    public float Chance;
    public int CountTargets;
    public AbilityData ProcAbilityData;
}