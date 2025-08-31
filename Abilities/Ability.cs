using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Godot;
using ProjectFireball.Abilities.Implementation;
using ProjectFireball.Characters;
using ProjectFireball.Effects;
using System.Linq;
using Godot.Collections;

namespace ProjectFireball.Abilities;

[JsonDerivedType(typeof(Fireball), nameof(Fireball))]
[JsonDerivedType(typeof(Strike), nameof(Strike))]
[JsonDerivedType(typeof(HeavyStrike), nameof(HeavyStrike))]
[GlobalClass]
public abstract partial class Ability : Resource
{
    [Export] public string AbilityName { get; set; } = "Ability";
    [Export] public int RemainingCooldown { get; set; }
    [Export] public int Cooldown { get; set; } = 1;
    [Export] public int MaxTargets { get; set; } = 1;
    [Export] public Array<Effect> Effects { get; set; } = [];
    
    // Virtual method for ability-specific visual effects
    public abstract void PlayAbilityAnimation(Character user, List<Character> targets);

    public void ApplyEffects(Character user, List<Character> targets)
    {
        var validTargets = targets.Take(MaxTargets);

        foreach (var target in validTargets)
        {
            GD.Print($"Use {AbilityName} on {target.Name}.");
            foreach (var effect in Effects)
            {
                if (effect.CanApply(user, target))
                    effect.Apply(user, target);
            }
        }
    }
    
    public void ReduceCooldown(int turns = 1)
    {
        RemainingCooldown = Math.Clamp(RemainingCooldown - turns, 0, Cooldown);
    }
}