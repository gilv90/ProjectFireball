using System.Linq;
using Godot;
using ProjectFireball.Abilities;
using ProjectFireball.Characters;

namespace ProjectFireball.Instructions.Implementation;

public class HealWoundedAlly(IHeal ability) : Instruction(ability)
{
    public override bool CanExecute(Character character, Battle battle)
    {
        return DetermineTarget(character, battle) is not null;
    }

    public override void Execute(Character character, Battle battle)
    {
        character.Jump();
        var target = DetermineTarget(character, battle);
        GD.Print($"{character.Name} uses {Ability.GetType().Name} on {target.Name}.");
        Ability.Execute(character, target);
        GD.Print($"{target.Name} has {target.CurrentHealth} health left.");
    }

    private Character DetermineTarget(Character character, Battle battle)
    {
        return battle.GetAllies(character)
            .Where(a => Ability.CanExecute(a))
            .OrderBy(a => a.CurrentHealth / a.MaxHealth.Value)
            .FirstOrDefault();
    }
}