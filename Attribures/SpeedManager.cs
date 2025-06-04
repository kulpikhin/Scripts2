using System.Collections.Generic;
using UnityEngine;

public class SpeedManager : MonoBehaviour
{
    private int BaseSpeed = 0;

    public float CurrentSpeed { get; private set; }

    // ���������� ���������� ������������ �������� (��������, �� ChillEffect)
    public void AddSpeedModifier(int modifier)
    {
        BaseSpeed += modifier;
    }

    // �������� ���������� ������������ ��������
    public void RemoveSpeedModifier(int modifier)
    {
        BaseSpeed -= modifier;
    }
}
