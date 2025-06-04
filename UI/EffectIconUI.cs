using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI-иконка для эффекта: показывает Power и оставшееся время.
/// Для Stack и Refresh стратегий отображает данные единственного эффекта.
/// Для Strongest стратегии отображает Power сильнейшего и общее время всех эффектов.
/// </summary>
public class EffectIconUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] public Image iconImage;
    [SerializeField] private TextMeshProUGUI powerText;
    [SerializeField] private TextMeshProUGUI timeText;

    private EffectContainer _container;
    private EffectType _effectType;
    private RefreshEffectMode _mode;

    public void Initialize(EffectContainer container, EffectType effectType)
    {
        _container = container;
        _effectType = effectType;
        if (_container.Instances.TryGetValue(_effectType, out var list) && list.Count > 0)
            _mode = list[0].RefreshMode;
        RefreshUI();
    }

    /// <summary>
    /// Принудительно обновить отображение Power и времени.
    /// </summary>
    public void RefreshUI()
    {
        if (!_container.Instances.TryGetValue(_effectType, out var list) || list.Count == 0)
        {
            _container.RemoveIcon(_effectType);
            return;
        }
        switch (_mode)
        {
            case RefreshEffectMode.StackMode:
            case RefreshEffectMode.RefreshMode:
                var single = list[0];
                powerText.text = single.Power.ToString();
                break;
            case RefreshEffectMode.StrongestMode:
                powerText.text = list.Max(e => e.Power).ToString();
                break;
        }
        float remaining;
        switch (_mode)
        {
            case RefreshEffectMode.StackMode:
            case RefreshEffectMode.RefreshMode:
                remaining = Mathf.Max(0f, list[0].ExpirationTime - Time.time);
                break;
            case RefreshEffectMode.StrongestMode:
                remaining = Mathf.Max(0f, list.Max(e => e.ExpirationTime) - Time.time);
                break;
            default:
                remaining = 0f;
                break;
        }
        timeText.text = Mathf.CeilToInt(remaining).ToString();
    }
}
