using System.Text.Json.Serialization;
using ProjectFireball.Characters;
using ProjectFireball.StatusEffects.Implementation;

namespace ProjectFireball.StatusEffects;

[JsonDerivedType(typeof(Burning), nameof(Burning))]
[JsonDerivedType(typeof(IncreaseDamage), nameof(IncreaseDamage))]
public abstract class StatusEffect() : IStatusEffect
{
    public int RemainingDuration { get; private set; }

    public abstract void Execute();
    
    public void ReduceDuration(int turns = 1)
    {
        if (RemainingDuration < turns)
            RemainingDuration = 0;
        RemainingDuration -= turns;
    }

    public void IncreaseDuration(int turns)
    {
        RemainingDuration += turns;
    }
}

public interface IStatusEffect
{
    public int RemainingDuration { get; }
    public void Execute();
    public void ReduceDuration(int turns);
    public void IncreaseDuration(int turns);
}

public interface IBuff : IStatusEffect;

public interface IDebuff : IStatusEffect;