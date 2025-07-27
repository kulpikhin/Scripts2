[System.Flags]
public enum AbilityTag
{
    None = 0,
    Poison = 1 << 0,
    Bleed = 1 << 1,
    Damage = 1 << 2,
    Fire = 1 << 4,
    Cold = 1 << 6,
    Lightning = 1 << 8,
    Physical = 1 << 10,
    Spell = 1 << 12,
    Attack = 1 << 14,
    Projectile = 1 << 16,
    Debuff = 1 << 18,
    Healing = 1 << 20,
    Summoning = 1 << 22,
    Buff = 1 << 24,
    Ailment = 1 << 26,
    Melee = 1 << 28,
    Range = 1 << 30,
}
