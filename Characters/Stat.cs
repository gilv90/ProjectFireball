using System.Collections.Generic;

namespace ProjectFireball.Characters;

public class Stat
{
    private readonly List<StatModifier> _modifiers = new();

    public int BaseValue { get; set; }

    public int Value
    {
        get
        {
            int finalValue = BaseValue;
            int percentSum = 0;

            foreach (var mod in _modifiers)
            {
                if (mod.Type == ModifierType.Flat)
                    finalValue += mod.Value;
                else if (mod.Type == ModifierType.Percent)
                    percentSum += mod.Value;
            }

            finalValue = (int)(finalValue * (1 + percentSum / 100f));
            return finalValue;
        }
    }

    public void AddModifier(StatModifier mod) => _modifiers.Add(mod);

    public void RemoveModifier(object source) =>
        _modifiers.RemoveAll(m => m.Source == source);
}