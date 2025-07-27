using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;
using Slider = UnityEngine.UI.Slider;

[RequireComponent(typeof(Character))]
public class CastManager : MonoBehaviour
{
    private IDamageable character;
    [SerializeField] private Slider slider;

    private Queue<Ability> AbilitiesQueue;
    private Coroutine _castingRoutine;
    private bool _casting;
    private TargetFinder _targetFinder;
    private CharacterAnimator _characterAnimator;
    private List<IDamageable> Targets;

    public event UnityAction<Ability> CastEnd;

    public float CurrentCastTime { get; private set; }

    private void Start()
    {
        if (!TestMode.IsTest)
        {
            character = GetComponent<Character>();
            _targetFinder = GetComponent<TargetFinder>();
            AbilitiesQueue = new Queue<Ability>();
            _characterAnimator = GetComponent<CharacterAnimator>();
            character.Died += StopCasting;
            SubAllAbilites();
        }
    }

    private void OnDisable()
    {
        UnSubAllAbities();
        character.Died -= StopCasting;
    }

    private void SubAllAbilites()
    {
        foreach (Ability ability in character.Abilities.abilities)
        {
            ability.AbilityCooldawner.CooldownEnd += AddToQueue;
        }
    }

    private void UnSubAllAbities()
    {
        foreach (Ability ability in character.Abilities.abilities)
        {
            ability.AbilityCooldawner.CooldownEnd -= AddToQueue;
        }
    }

    private void AddToQueue(Ability ability)
    {
        AbilitiesQueue.Enqueue(ability);
        TryCastAbility();
    }

    private void TryCastAbility()
    {
        if (AbilitiesQueue.Count == 0)
        {
            return;
        }

        if (!character.IsDead && BattleCondition.GameContinues)
        {
            if (!_casting)
            {
                if (CheckMana(AbilitiesQueue.Peek().AbilityDatas.ManaCost))
                {
                    Targets = _targetFinder.GetTarget(character, AbilitiesQueue.Peek());

                    foreach(IDamageable target in Targets)
                    {
                        Debug.Log(AbilitiesQueue.Peek().Owner.Name + " Метится в " + target.Name);
                    }

                    AbilitiesQueue.Peek().AbilityDatas.Targets = Targets;

                    foreach (IDamageable target in AbilitiesQueue.Peek().AbilityDatas.Targets)
                    {
                        target.Died += OnTargetDead;
                    }

                    StartCasting(AbilitiesQueue.Peek());
                }
                else
                {
                    Debug.Log("not enough mana");

                    AbilitiesQueue.Enqueue(AbilitiesQueue.Peek());
                    AbilitiesQueue.Dequeue();
                }
            }
        }
        else
        {
            Debug.Log("BattleCondition.GameContinues = " + BattleCondition.GameContinues);
        }
    }

    private void OnTargetDead(IDamageable target)
    {
        target.Died -= OnTargetDead;

        if (AbilitiesQueue.Count > 0)
        {
            AbilitiesQueue.Peek().AbilityDatas.Targets.Remove(target);

            if (AbilitiesQueue.Peek().AbilityDatas.Targets.Count == 0)
            {
                StopCasting(character);
                TryCastAbility();
            }
        }
    }

    private bool CheckMana(int manaCost)
    {
        if (character.Stats.GetStat(StatType.CurrentMana) >= manaCost)
        {
            character.Stats.SpentMana(manaCost);
            return true;
        }

        return false;
    }

    private void StartCasting(Ability currentAbility)
    {
        _casting = true;

        if (_castingRoutine != null)
        {
            StopCoroutine(_castingRoutine);
        }

        _castingRoutine = StartCoroutine(Casting(currentAbility));
    }

    public void StopCasting(IDamageable owner)
    {
        StopCoroutine(_castingRoutine);
        slider.value = 0;
    }

    private IEnumerator Casting(Ability currentAbility)
    {
        CurrentCastTime = 0;
        float castTime = CalculateCastTime(currentAbility.AbilityDatas.CastTime);
        slider.maxValue = castTime;

        float startTime = Time.time;

        while (CurrentCastTime < castTime)
        {
            CurrentCastTime = Time.time - startTime;
            slider.value = CurrentCastTime;

            yield return null;
        }

        _characterAnimator.Attack();

        slider.value = 0;

        character.Abilities.UseAbility(AbilitiesQueue.Dequeue());
        CastEnd?.Invoke(currentAbility);

        _casting = false;

        TryCastAbility();
    }

    private float CalculateCastTime(float defaultCastTime)
    {
        float speedStat = character.Stats.GetStat(StatType.Speed);
        float adjustedTime;

        if (speedStat >= 0)
        {
            float speedIncreased = (defaultCastTime / 100f) * speedStat;
            adjustedTime = defaultCastTime + speedIncreased;
        }
        else
        {
            float speedIncreased = (defaultCastTime / 100f) * speedStat;
            adjustedTime = defaultCastTime - speedIncreased;
        }

        return Mathf.Max(adjustedTime, 0.1f);
    }
}
