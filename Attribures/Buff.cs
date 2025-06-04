public class Buff
{
    public string Id;
    public StatType Type;
    public float Value;
    public bool IsAdditive;
    public float Duration;

    public Buff(string id, StatType type, float value, bool isAdditive, float duration)
    {
        Id = id;
        Type = type;
        Value = value;
        IsAdditive = isAdditive;
        Duration = duration;
    }

    public Modifier ToModifier()
    {
        return new Modifier(Type, Value, IsAdditive, 0, Id);
    }
}
