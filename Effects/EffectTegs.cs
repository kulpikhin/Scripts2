[System.Flags]
public enum EffectTegs
{
    None = 0,   
    DamageOnEnd    = 1 << 0,
    Control        = 1 << 1,
    NoDamage       = 1 << 2,
    Buff           = 1 << 4,
    Debuff         = 1 << 6,
    DamageOverTime = 1 << 8,
}
