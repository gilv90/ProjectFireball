using System.Collections.Generic;
using System.Linq;
using Godot;
using ProjectFireball.Characters;
using ProjectFireball.Effects;
using ProjectFireball.Effects.Implementation;

namespace ProjectFireball.Abilities.Implementation;

[GlobalClass]
public partial class Fireball : Ability
{
    public Fireball()
    {
        AbilityName = "Fireball";
        Cooldown = 5;
        MaxTargets = 1;

        Effects =
        [
            new DamageEffect()
            {
                Name = "Fireball Damage",
                Multiplier = 3
            }
        ];
    }

    public override void PlayAbilityAnimation(Character user, List<Character> targets)
    {
        var validTargets = targets.Take(MaxTargets);
        foreach (var target in validTargets)
        {
            target.FlashRed();
        }
    }
}