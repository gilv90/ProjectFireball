using System.Collections.Generic;
using System.Linq;
using Godot;
using ProjectFireball.Characters;
using ProjectFireball.Effects.Implementation;

namespace ProjectFireball.Abilities.Implementation;

[GlobalClass]
public partial class Resurrect : Ability
{
    public Resurrect()
    {
        AbilityName = "Resurrect";
        Cooldown = 8;
        MaxTargets = 1;

        Effects =
        [
            new ResurrectEffect()
            {
                Name = "Resurrect Resurrect",
                Multiplier = 2
            }
        ];
    }

    public override void PlayAbilityAnimation(Character user, List<Character> targets)
    {
        var validTargets = targets.Take(MaxTargets);
        foreach (var target in validTargets)
        {
            target.FlashGreen();
        }
    }
}