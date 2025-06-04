using System.Collections;
using UnityEngine;
using UnityEngine.Events;

//[RequireComponent(typeof(Ability))]
public class AbilityCooldown : MonoBehaviour
{
    [SerializeField] private float _cooldawnDuration;

    private Coroutine _cdCoroutine;
    private Ability _ability;
    private WaitForSeconds _waitTime;

    public event UnityAction<Ability> CooldownEnd;

    public float Progress { get; private set; }
    public float CurrentCDTime { get; private set; }
    public bool IsCooldown { get; private set; }
    public float CooldawnDuration => _cooldawnDuration;

    private void OnEnable()
    {
        _ability = GetComponent<Ability>();
        _waitTime = new WaitForSeconds(0.1f);
    }

    public void StartCooldawn(Ability ability)
    {        
        IsCooldown = true;
        CurrentCDTime = _cooldawnDuration;

        if (_cdCoroutine != null)
        {
            StopCoroutine(_cdCoroutine);
        }

        _cdCoroutine = StartCoroutine(CooldownRoutine(ability));
    }

    public void StopCooldown()
    {
        StopCoroutine(_cdCoroutine);
        CurrentCDTime = 0;
        IsCooldown = false;
        CooldownEnd.Invoke(_ability);
    }

    public void ReduceCooldown(float time)
    {
        if (time > CurrentCDTime)
        {
            CurrentCDTime = 0;
        }
        else
        {
            CurrentCDTime -= time;
        }
    }

    private IEnumerator CooldownRoutine(Ability ability)
    {
        CurrentCDTime = 0f;
        ability.icon.slider.maxValue = _cooldawnDuration;
        float startTime = Time.time;

        IsCooldown = true;

        while (CurrentCDTime < _cooldawnDuration)
        {
            CurrentCDTime = Time.time - startTime;

            ability.icon.slider.value = CurrentCDTime;

            yield return _waitTime;
        }

        IsCooldown = false;
        CooldownEnd.Invoke(ability);
    }
}
