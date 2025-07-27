using UnityEngine;
using System.Collections.Generic;

public class CharacterAbilities : MonoBehaviour
{
    [SerializeField] private Character character;
    [SerializeField] private AbilitiesIcon _ablitiesIcons;

    public List<AbilityData> abilityList;

    public List<Ability> abilities = new List<Ability>();

    private void OnEnable()
    {
        if (!TestMode.IsTest)
        {
            SetAbilityList();
            SetAbilityIcons();
            StartAllCoolDawns();
            SetAllIcon();
            character.Died += StopAllCooldawn;
        }
    }

    private void OnDisable()
    {
        character.Died -= StopAllCooldawn;
    }

    public void UseAbility(Ability ability)
    {
        ability.Cast();
    }

    private void SetAbilityList()
    {
        foreach (var abilityData in abilityList)
        {
            Ability ability = gameObject.AddComponent<Ability>();
            ability.Initialize(character, character.StartSpellPosition, abilityData);

            abilities.Add(ability);
        }
    }

    private void SetAbilityIcons()
    {
        for (int i = 0; i < abilities.Count; i++)
        {
            abilities[i].AbilityDatas.icon = _ablitiesIcons.GetAbility(i);
        }
    }

    private void SetAllIcon()
    {
        for (int i = 0; i < abilities.Count; i++)
        {
            _ablitiesIcons.SetIcon(i, abilities[i].AbilityDatas.sprite);
        }

        _ablitiesIcons.FillEmptySlots();
    }

    private void StartAllCoolDawns()
    {
        foreach (Ability ability in abilities)
        {            
            ability.AbilityCooldawner.StartCooldawn(ability);
        }
    }

    private void StopAllCooldawn(IDamageable owner)
    {
        foreach (Ability ability in abilities)
        {
            ability.AbilityCooldawner.StopCooldown();
        }
    }
}