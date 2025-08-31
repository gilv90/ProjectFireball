using Godot;
using ProjectFireball.Characters;

namespace ProjectFireball.Effects.Implementation;

[GlobalClass]
public partial class DamageEffect : Effect
{
    [Export] public int Multiplier { get; set; } = 1;

    public override bool CanApply(Character user, Character target)
    {
        return target.Stats.CurrentHealth > 0;
    }

    public override void Apply(Character user, Character target)
    {
        var damage = user.Stats.Power.Value * Multiplier;
        GD.Print($"{user.Name} deals {damage} damage to {target.Name}.");
        target.TakeDamage(damage);
    }
}