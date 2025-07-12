using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenRoutine : MonoBehaviour
{
    private IDamageable _character;
    private StatContainer _statContainer;

    private void Start()
    {
        _statContainer = _character.Stats;
        TimeEvent.SpentSecond += Regen;
    }

    private void OnDisable()
    {
        TimeEvent.SpentSecond -= Regen;
    }

    public void SetCharecter(IDamageable character)
    {
        _character = character;
    }

    private void Regen()
    {
        RegenHp();
        RegenMP();
    }

    private void RegenHp()
    {
        _statContainer.Heal(CalculateRegen(_statContainer.GetStat(StatType.MaxHealth), _statContainer.GetStat(StatType.HPRegen)));
    }

    private void RegenMP()
    {
        _statContainer.ManaRecover(CalculateRegen(_statContainer.GetStat(StatType.MaxMana), _statContainer.GetStat(StatType.ManaRegen)));
    }

    private int CalculateRegen(float maxValue, float percentValue)
    {
        return Convert.ToInt32((maxValue/ 100) * percentValue);    
    }    
}
