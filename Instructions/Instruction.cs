using System.Collections.Generic;
using System.Text.Json.Serialization;
using Godot;
using ProjectFireball.Abilities;
using ProjectFireball.Characters;
using ProjectFireball.Instructions.Implementation;

namespace ProjectFireball.Instructions;

[JsonDerivedType(typeof(AttackClosest), nameof(AttackClosest))]
[JsonDerivedType(typeof(HealWoundedAlly), nameof(HealWoundedAlly))]
[GlobalClass]
public abstract partial class Instruction : Resource
{
    public string Name { get; set; }
    [Export] public Ability Ability { get; set; }
    public abstract List<Character> DetermineTargets(Character user, Battle battle);
    public abstract void PlayInstructionAnimation(Character user, List<Character> targets);
    public abstract bool CanExecute(Character character, Battle battle);

    public void Execute(Character character, Battle battle)
    {
        var targets = DetermineTargets(character, battle);
        
        // Instruction-specific animation (movement)
        PlayInstructionAnimation(character, targets);

        // Apply ability effects to each target
        Ability.ApplyEffects(character, targets);
        Ability.RemainingCooldown = Ability.Cooldown;

        // Ability-specific visual effects (optional)
        Ability.PlayAbilityAnimation(character, targets);
    }
}