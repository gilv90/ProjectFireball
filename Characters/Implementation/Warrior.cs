using ProjectFireball.Abilities.Implementation;
using ProjectFireball.Instructions.Implementation;
using ProjectFireball.Passives.Implementation;

namespace ProjectFireball.Characters.Implementation;

public partial class Warrior : Character
{
    public Warrior()
    {
        Name = nameof(Warrior);
        // Speed = new Stat() { BaseValue = 10 };
        // MaxHealth = new Stat() { BaseValue = 100 };
        // Damage = new Stat() { BaseValue = 10 };
        // Armor = new Stat() { BaseValue = 10 };

        var strike = new Strike();
        var heavyStrike = new HeavyStrike();

        // Abilities = [strike, heavyStrike];
        Passives = [new ReactiveArmor(this)];

        // Instructions = [new AttackClosest(heavyStrike), new AttackClosest(strike)];
    }
}