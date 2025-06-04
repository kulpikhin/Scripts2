[System.Flags]
public enum AbilityType
{
    None = 0,
    Damage = 1 << 0,
    Heal = 1 << 1,
    Buff = 1 << 2,
    Debuff = 1 << 4
}
