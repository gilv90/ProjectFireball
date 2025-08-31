using System.Collections.Generic;
using System.Linq;
using Godot;
using ProjectFireball.Characters;
using ProjectFireball.Effects.Implementation;

namespace ProjectFireball.Abilities.Implementation;

[GlobalClass]
public partial class MinorHeal : Ability
{
    public MinorHeal()
    {
        AbilityName = "Minor Heal";
        Cooldown = 3;
        MaxTargets = 1;

        Effects =
        [
            new HealEffect()
            {
                Name = "Minor Heal Healing",
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