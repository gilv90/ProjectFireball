using ProjectFireball.Abilities;
using ProjectFireball.Characters;

namespace ProjectFireball.Events;

public record AbilityUsed(Ability Ability);
public record DamageTaken(Character Character, int Damage);
public record CharacterDefeated(Character Character);