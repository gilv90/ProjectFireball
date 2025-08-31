using Godot;
using ProjectFireball.Characters;

namespace ProjectFireball.Effects;

[GlobalClass]
public abstract partial class Effect: Resource
{
    [Export] public string Name { get; set; } = "Effect";

    // Returns true if the effect can be applied in this context
    public abstract bool CanApply(Character user, Character target);

    // Apply the effect
    public abstract void Apply(Character user, Character target);
}