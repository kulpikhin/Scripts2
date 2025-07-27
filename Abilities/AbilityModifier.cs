using UnityEngine;

public abstract class AbilityModifier : ScriptableObject
{
    public abstract void Apply(AbilityContext context);
}
