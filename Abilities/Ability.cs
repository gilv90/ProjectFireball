using System.Text.Json.Serialization;
using ProjectFireball.Abilities.Implementation;
using ProjectFireball.Characters;

namespace ProjectFireball.Abilities;

[JsonDerivedType(typeof(Fireball), nameof(Fireball))]
[JsonDerivedType(typeof(Strike), nameof(Strike))]
[JsonDerivedType(typeof(HeavyStrike), nameof(HeavyStrike))]
public abstract class Ability(int cooldown) : IAbility
{
    public void Execute(Character character, Character target)
    {
        ExecuteImplementation(character, target);
        RemainingCooldown = Cooldown;
    }
    
    protected abstract void ExecuteImplementation(Character character, Character target);
    public abstract bool CanExecute(Character target);

    public void ReduceCooldown(int turns = 1)
    {
        if (RemainingCooldown <= turns)
            RemainingCooldown = 0;
        else
            RemainingCooldown -= turns;
    }

    public void IncreaseCooldown(int turns)
    {
        RemainingCooldown += turns;
    }

    public int RemainingCooldown { get; private set; }
    public int Cooldown { get; } = cooldown;
}

public interface IAbility
{
    public void Execute(Character character, Character target);
    public bool CanExecute(Character target);
}