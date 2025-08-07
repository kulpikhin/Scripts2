using System;
using System.Collections;
using System.ComponentModel;
using System.Xml.Linq;
using UnityEngine;
using static UnityEngine.LightProbeProxyVolume;

public class EffectInstance
{
    public EffectType Type { get; }
    public int Power { get; set; }
    public float Duration { get; set; }
    public float ExpirationTime { get; private set; }
    public bool IsStrongest { get; set; }
    public RefreshEffectMode RefreshMode { get; private set; }
    public EffectContainer owner;

    public event Action<EffectInstance> OnTick;
    public event Action<EffectInstance> OnExpired;
    public event Action<EffectInstance> OnAply;


    private MonoBehaviour _runner;
    private Coroutine _coroutine;

    public EffectInstance(EffectType type, int power, RefreshEffectMode refreshMode, float duration)
    {
        Type = type;
        Power = power;
        Duration = duration;
        RefreshMode = refreshMode;
    }


    internal void Init(EffectContainer container)
    {
        _runner = container;
        owner = container;
        container._owner.Died += Stop;
        ExpirationTime = Time.time + Duration;
    }

    public void Start()
    {
        Debug.Log("Run " + Duration);
        Stop();

        if (RefreshMode == RefreshEffectMode.StrongestMode) return;
        _coroutine = _runner.StartCoroutine(Run());
    }

    public void Stop(IDamageable own)
    {
        owner._owner.Died -= Stop;
        OnExpired?.Invoke(this);

        if (_coroutine != null) _runner.StopCoroutine(_coroutine);
        _coroutine = null;
    }

    public void Stop()
    {
        if (_coroutine != null) _runner.StopCoroutine(_coroutine);
        _coroutine = null;
    }

    private IEnumerator Run()
    {
        float interval = 1f;
        int ticks = Mathf.FloorToInt(Duration / interval);       

        OnAply?.Invoke(this);

        for (int i = 0; i < ticks; i++)
        {
            yield return new WaitForSeconds(interval);
            OnTick?.Invoke(this);
        }
        OnExpired?.Invoke(this);
    }
}
