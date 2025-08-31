using System.Collections.Generic;
using System.Linq;
using Godot;
using ProjectFireball.Characters;

namespace ProjectFireball.Instructions.Implementation;

[GlobalClass]
public partial class ResurrectAlly : Instruction
{
    public ResurrectAlly()
    {
        Name = "Resurrect Ally";
    }

    public override List<Character> DetermineTargets(Character user, Battle battle)
    {
        return battle.GetAllies(user)
            .Where(a => a.Stats.IsDead).ToList();
    }

    public override void PlayInstructionAnimation(Character user, List<Character> targets)
    {
        user.Jump();
    }

    public override bool CanExecute(Character character, Battle battle)
    {
        var hasValidTarget = DetermineTargets(character, battle).Count != 0;
        var hasCooldown = Ability.RemainingCooldown == 0;
        return hasValidTarget && hasCooldown;
    }
}