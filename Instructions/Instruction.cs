using System.Text.Json.Serialization;
using ProjectFireball.Abilities;
using ProjectFireball.Characters;
using ProjectFireball.Instructions.Implementation;

namespace ProjectFireball.Instructions;

[JsonDerivedType(typeof(AttackClosest), nameof(AttackClosest))]
[JsonDerivedType(typeof(HealWoundedAlly), nameof(HealWoundedAlly))]
public abstract class Instruction(IAbility ability)
{
    public IAbility Ability { get; set; } = ability;
    public abstract bool CanExecute(Character character, Battle battle);
    public abstract void Execute(Character character, Battle battle);
}