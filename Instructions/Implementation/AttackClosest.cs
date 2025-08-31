using System.Linq;
using Godot;
using ProjectFireball.Abilities;
using ProjectFireball.Characters;

namespace ProjectFireball.Instructions.Implementation;

public class AttackClosest(IAttack ability) : Instruction(ability)
{
    public override bool CanExecute(Character character, Battle battle)
    {
        return Ability.CanExecute(DetermineTarget(character, battle));
    }

    public override void Execute(Character character, Battle battle)
    {
        character.Lunge();
        var target = DetermineTarget(character, battle);
        target.FlashRed();
        GD.Print($"{character.Name} uses {Ability.GetType().Name} on {target.Name}.");
        Ability.Execute(character, target);
        GD.Print($"{target.Name} has {target.CurrentHealth} health left.");
    }

    private static Character DetermineTarget(Character character, Battle battle)
    {
        return battle.GetEnemies(character).First(e => e.CurrentHealth > 0);
    }
}