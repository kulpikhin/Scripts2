using System;
using UnityEngine;

public class ArmorManager : MonoBehaviour
{
    public float Value { get; private set; }

    private void OnEnable()
    {
        Value = 2;
    }

    public void AddArmor(int value)
    {
        if (value >= 0)
        {
            Value += value;
        }
        else
        {
            Debug.Log("negative armor value");
        }
    }
    
    public void ReducedArmor(int value)
    {
        if (value >= 0)
        {
            Value -= value;
        }
        else
        {
            Debug.Log("negative armor value");
        }
    }

    public int GetFinalDamage(int damage)
    {
        float finalDamage = damage / 100f * (100f - Value);

        return Convert.ToInt32(finalDamage);
    }
}
