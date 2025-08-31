using System.Collections.Generic;
using System.Linq;
using Godot;
using ProjectFireball.Abilities;
using ProjectFireball.Characters;

namespace ProjectFireball.Instructions.Implementation;

[GlobalClass]
public partial class HealWoundedAlly : Instruction
{
    public HealWoundedAlly()
    {
        Name = "Heal Wounded Ally";
    }

    public override List<Character> DetermineTargets(Character user, Battle battle)
    {
        return battle.GetAllies(user)
            .Where(a => a.Stats.CurrentHealth < a.Stats.MaxHealth.Value && !a.Stats.IsDead)
            .OrderBy(a => a.Stats.CurrentHealth / a.Stats.MaxHealth.Value).ToList();
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