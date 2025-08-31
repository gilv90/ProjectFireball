using System.Collections.Generic;
using System.Linq;
using Godot;
using ProjectFireball.Characters;
using ProjectFireball.Effects.Implementation;

namespace ProjectFireball.Abilities.Implementation;

[GlobalClass]
public partial class Strike : Ability
{
    public Strike()
    {
        AbilityName = "Strike";
        Cooldown = 0;
        MaxTargets = 1;

        Effects =
        [
            new DamageEffect()
            {
                Name = "Strike Damage",
                Multiplier = 2
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