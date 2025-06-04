using UnityEngine;

[CreateAssetMenu(fileName = "NewSubClass", menuName = "Characters/SubClass")]
public class SubClass : ScriptableObject
{
    public CharacterClass baseClass;  // Основной класс (например, Маг, Воин)
    public string subClassName;
    public string description;
}
