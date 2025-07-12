using System;
using UnityEngine;
using UnityEngine.UI;

public class ResourceBar : MonoBehaviour
{
    [SerializeField] private Character _character;
    [SerializeField] private Slider _hpSlider;
    [SerializeField] private Slider _mpSlider;
    [SerializeField] private Image _hpBackGround;


    private int _maxHPValue;
    private int _maxMPValue;

    private void OnEnable()
    {
        BarInitialized();
        _character.Stats.HealthChanged += ChangeHPBar;
        _character.Stats.ManaChanged += ChangeMPBar;
    }

    private void OnDisable()
    {
        _character.Stats.HealthChanged -= ChangeHPBar;
        _character.Stats.HealthChanged -= ChangeMPBar;
    }

    private void Start()
    {
        _hpBackGround.sprite = null;
    }

    private void ChangeHPBar(float value)
    {
        _hpSlider.value = value;
    }

    private void ChangeMPBar(float value)
    {
        _mpSlider.value = value;
    }

    private void BarInitialized()
    {
        _maxHPValue = Convert.ToInt32(_character.Stats.GetStat(StatType.MaxHealth));
        _maxMPValue = Convert.ToInt32(_character.Stats.GetStat(StatType.MaxMana));

        _hpSlider.maxValue = _maxHPValue;
        _hpSlider.value = _maxHPValue;

        _mpSlider.maxValue = _maxMPValue;
        _mpSlider.value = _maxMPValue;
    }
}
