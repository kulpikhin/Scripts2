using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterClass", menuName = "Characters/CharacterClass")]
public class CharacterClass : ScriptableObject
{
    public float DamageTakenIncreas;
    public float Armor;
    public float DamageIncreas;
    public float DamageMore;
    public float MaxHealth;
    public float CurrentHealth;
    public float Speed;
    public float CritChance;
    public float CritDamage;
    public float LifeSteal;
    public float DodgeRaiting;
    public float MaxMana;
    public float CurrentMana;
    public float CCResistance;
    public float CDSpeed;
    public float HPRegen;
    public float ManaRegen;
    public float HealPower;
    public float PetHealth;
    public float PetDamage;
    public float AilmentChance;
    public float AilmentPower;
    public float AilmentDuration;
}
