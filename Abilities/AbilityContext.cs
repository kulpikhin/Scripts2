using System;
using UnityEngine;

public class AbilityContext
{
    public Transform Caster;
    public Transform Target;
    public AbilityData Data;

    // �������, �� ������� ����� ������������� ������������
    public Action<Transform> OnHit;
}
