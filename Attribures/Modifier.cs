public class Modifier
{
    public StatType Type;
    public float Value;
    public bool IsAdditive;
    public int Priority;
    public string SourceId;

    public Modifier(StatType type, float value, bool isAdditive, int priority, string sourceId)
    {
        Type = type;
        Value = value;
        IsAdditive = isAdditive;
        Priority = priority;
        SourceId = sourceId;
    }
}
