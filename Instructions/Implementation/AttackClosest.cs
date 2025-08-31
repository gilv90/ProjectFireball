using System.Collections.Generic;
using System.Linq;
using Godot;
using ProjectFireball.Abilities;
using ProjectFireball.Characters;

namespace ProjectFireball.Instructions.Implementation;

[GlobalClass]
public partial class AttackClosest : Instruction
{
    public AttackClosest()
    {
        Name = "Attack Closest";
    }
    public override List<Character> DetermineTargets(Character user, Battle battle)
    {
        return battle.GetEnemies(user).Where(e => !e.Stats.IsDead).ToList();
    }

    public override void PlayInstructionAnimation(Character user, List<Character> targets)
    {
        user.Lunge();
    }

    public override bool CanExecute(Character character, Battle battle)
    {
        var hasValidTarget = DetermineTargets(character, battle).Count != 0;
        var hasCooldown = Ability.RemainingCooldown == 0;
        return hasValidTarget && hasCooldown;
    }
}