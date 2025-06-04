using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using System.Collections;

public class EffectContainer : MonoBehaviour
{
    internal Dictionary<EffectType, List<EffectInstance>> Instances { get; } = new Dictionary<EffectType, List<EffectInstance>>();
    internal Dictionary<EffectType, Coroutine> TypeCoroutines { get; } = new Dictionary<EffectType, Coroutine>();
    private Dictionary<RefreshEffectMode, IEffectStrategy> _strategies;
    private Dictionary<EffectType, EffectIconUI> _activeIcons;

    [Header("UI Prefabs & Parents")]
    public GameObject EffectIconPrefab;
    public Transform BuffsContainer;
    public Transform DebuffsContainer;

    public EffectDataBase EffectDatas;

    public IDamageable _owner;

    private void Awake()
    {
        _strategies = new Dictionary<RefreshEffectMode, IEffectStrategy>
        {
            { RefreshEffectMode.StrongestMode, new StrongestStrategy() },
            { RefreshEffectMode.RefreshMode, new RefreshStrategy() },
            { RefreshEffectMode.StackMode, new StackStrategy() }
        };
        _activeIcons = new Dictionary<EffectType, EffectIconUI>();
    }

    private void OnEnable()
    {
        if (!TryGetComponent<IDamageable>(out _owner))
            Debug.LogWarning("EffectContainer: IDamageable owner not found.");
    }

    public void ApplyEffect(EffectInstance newInstance)
    {
        newInstance.Init(this);

        _strategies[newInstance.RefreshMode].Apply(this, newInstance);

        if (newInstance.RefreshMode == RefreshEffectMode.StrongestMode &&
            !TypeCoroutines.ContainsKey(newInstance.Type))
        {
            TypeCoroutines[newInstance.Type] = StartCoroutine(StrongestCoroutine(newInstance.Type));
        }

        CreateOrUpdateIcon(newInstance.Type);
    }

    private void CreateOrUpdateIcon(EffectType type)
    {
        if (!_activeIcons.TryGetValue(type, out var icon))
        {
            var isDebuff = EffectDatas.GetEffectData(type).effectTegs.HasFlag(EffectTegs.Debuff);
            var parent = isDebuff ? DebuffsContainer : BuffsContainer;
            var go = Instantiate(EffectIconPrefab, parent);
            icon = go.GetComponent<EffectIconUI>();
            icon.Initialize(this, type);
            icon.iconImage.sprite = EffectDatas.GetEffectData(type).Icon;   
            _activeIcons[type] = icon;
        }
        icon.RefreshUI();
    }

    internal void HandleTick(EffectInstance inst)
    {
        // 1) Старая логика: наносим урон
        var data = EffectDatas.GetEffectData(inst.Type);
        data.Logic.OnTick(_owner, inst.Power);

        //Debug.Log("tick " + inst.Power);

        // 2) Новая строка: обновляем UI-иконку этого эффекта
        if (_activeIcons.TryGetValue(inst.Type, out var icon))
            icon.RefreshUI();
    }

    public void OnAply(EffectInstance inst)
    {
        var data = EffectDatas.GetEffectData(inst.Type);
        data.Logic.OnApply(_owner, inst.Power);
    }

    internal void OnExpire(EffectInstance inst)
    {
        var data = EffectDatas.GetEffectData(inst.Type);

        _strategies[inst.RefreshMode].HandleExpiration(this, inst);
        var type = inst.Type;
        if (_activeIcons.TryGetValue(type, out var icon))
        {
            if (Instances.TryGetValue(type, out var list) && list.Count > 0)
                icon.RefreshUI();
            else
                RemoveIcon(type);
        }
    }

    internal IEnumerator StrongestCoroutine(EffectType type)
    {
        float nextTick = Time.time + 1f;
        while (Instances.TryGetValue(type, out var list) && list.Count > 0)
        {
            float now = Time.time;
            if (now >= nextTick)
            {
                var strongest = list.OrderByDescending(e => e.Power).First();
                HandleTick(strongest);

                // сразу же обновляем иконку этого эффекта
                if (_activeIcons.TryGetValue(type, out var icon))
                    icon.RefreshUI();

                nextTick += 1f;
            }
            var expired = list.Where(e => e.ExpirationTime <= now).ToList();
            foreach (var e in expired) OnExpire(e);
            yield return null;
        }
        TypeCoroutines.Remove(type);
    }

    private void RemoveIcon(EffectType type)
    {
        if (_activeIcons.TryGetValue(type, out var icon))
        {
            if (icon != null) Destroy(icon.gameObject);
            _activeIcons.Remove(type);
        }
    }
}