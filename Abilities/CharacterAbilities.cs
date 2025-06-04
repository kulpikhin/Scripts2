using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class CharacterAbilities : MonoBehaviour
{
    [SerializeField] private Character character;
    [SerializeField] private AbilitiesIcon _ablitiesIcons;

    public List<Ability> abilities;

    private void OnEnable()
    {
        if (!TestMode.IsTest)
        {
            SetAbilityIcons();
            StartAllCoolDawns();
            SetCharcacterToAbilities();
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
        ability.Activate();
    }

    private void SetAbilityIcons()
    {
        for (int i = 0; i < abilities.Count; i++)
        {
            abilities[i].icon = _ablitiesIcons.GetAbility(i);
        }
    }

    private void SetAllIcon()
    {
        for (int i = 0; i < abilities.Count; i++)
        {
            _ablitiesIcons.SetIcon(i, abilities[i].sprite);
        }

        _ablitiesIcons.FillEmptySlots();
    }

    private void StartAllCoolDawns()
    {

        foreach (Ability ability in abilities)
        {
            ability.Init();

            ability.CooldownAbility.StartCooldawn(ability);
        }
    }

    private void StopAllCooldawn(IDamageable owner)
    {
        foreach (Ability ability in abilities)
        {
            ability.CooldownAbility.StopCooldown();
        }
    }

    private void SetCharcacterToAbilities()
    {
        foreach (Ability ability in abilities)
        {
            ability._character = character;
        }
    }
}