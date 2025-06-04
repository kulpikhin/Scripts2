using System.Collections.Generic;
using UnityEngine;

public class SpeedManager : MonoBehaviour
{
    private int BaseSpeed = 0;

    public float CurrentSpeed { get; private set; }

    // Добавление временного модификатора скорости (например, от ChillEffect)
    public void AddSpeedModifier(int modifier)
    {
        BaseSpeed += modifier;
    }

    // Удаление временного модификатора скорости
    public void RemoveSpeedModifier(int modifier)
    {
        BaseSpeed -= modifier;
    }
}
