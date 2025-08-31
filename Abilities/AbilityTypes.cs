using System.Text.Json.Serialization;
using ProjectFireball.Abilities.Implementation;

namespace ProjectFireball.Abilities;

[JsonDerivedType(typeof(Fireball), nameof(Fireball))]
[JsonDerivedType(typeof(Strike), nameof(Strike))]
[JsonDerivedType(typeof(HeavyStrike), nameof(HeavyStrike))]
public interface IAttack : IAbility;
public interface IHeal : IAbility;
public interface IBuff : IAbility;
public interface IDebuff : IAbility;