namespace ProjectFireball.Characters;

public enum ModifierType { Flat, Percent }

public class StatModifier(int value, ModifierType type, object source)
{
    public ModifierType Type { get; } = type;
    public int Value { get; } = value;
    public object Source { get; } = source; // e.g., item, buff, passive (helps with removal)
}