using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatContainer : MonoBehaviour
{
    private Dictionary<StatType, float> _values;

    public bool IsDead = false;

    public event Action OnDead;
    public event Action<float> HealthChanged;
    public event Action<float> ManaChanged;

    public void SetCurrentStats()
    {
        float currentHealth = _values[StatType.MaxHealth];
        _values[StatType.CurrentHealth] = currentHealth;

        float currentMana = _values[StatType.MaxMana];
        _values[StatType.CurrentMana] = currentMana;
    }

    public void SetStats(Dictionary<StatType, float> baseStats)
    {
        //  опируем исходные значени€
        _values = new Dictionary<StatType, float>(baseStats);
    }

    public void TakeDamage(int damage)
    {
        if (!IsDead && BattleCondition.GameContinues)
        {
            if (damage >= _values[StatType.CurrentHealth])
            {
                _values[StatType.CurrentHealth] = 0;

                OnDead?.Invoke();
                IsDead = true;
            }
            else
            {               
                _values[StatType.CurrentHealth] -= damage;
            }

            HealthChanged?.Invoke(Convert.ToInt32(_values[StatType.CurrentHealth]));
        }
    }

    public void Heal(int power)
    {
        if (!IsDead && BattleCondition.GameContinues)
        {
            _values[StatType.CurrentHealth] = Mathf.Clamp(_values[StatType.CurrentHealth] + power, 0, _values[StatType.MaxHealth]);
            HealthChanged?.Invoke(_values[StatType.CurrentHealth]);
        }
    }

    public void SpentMana(int value)
    {
        _values[StatType.CurrentMana] -= value; // должна быть проверка
        ManaChanged?.Invoke(Convert.ToInt32(_values[StatType.CurrentMana]));
    }

    public float GetStat(StatType type)
    {
        return _values.TryGetValue(type, out var val) ? val : 0f;
    }

    public void AddStat(StatType type, float value)
    {
        if (_values.ContainsKey(type)) _values[type] += value;
        else _values[type] = value;
    }

    public void RemoveStat(StatType type, float value)
    {
        if (!_values.ContainsKey(type)) return;
        _values[type] -= value;
    }
}
