using System;
using UnityEngine;

public class AbilityContext
{
    public Transform Caster;
    public Transform Target;
    public AbilityData Data;

    // События, на которые могут подписываться модификаторы
    public Action<Transform> OnHit;
}
