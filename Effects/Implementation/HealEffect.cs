using Godot;
using ProjectFireball.Characters;

namespace ProjectFireball.Effects.Implementation;

[GlobalClass]
public partial class HealEffect : Effect
{
    [Export] public int Multiplier { get; set; } = 1;

    public override bool CanApply(Character user, Character target)
    {
        return target.Stats.CurrentHealth < target.Stats.MaxHealth.Value 
               && !target.Stats.IsDead;
    }

    public override void Apply(Character user, Character target)
    {
        target.Heal(user.Stats.Power.Value * Multiplier);
        GD.Print($"{user.Name} heals {target.Name} for {user.Stats.Power.Value * Multiplier}.");
    }
}