using System.Diagnostics.CodeAnalysis;
using Godot;
using ProjectFireball.Abilities;
using ProjectFireball.Abilities.Implementation;
using ProjectFireball.Instructions;
using ProjectFireball.Instructions.Implementation;

namespace ProjectFireball.Characters.Implementation;

public partial class Mage : Character
{
    [SetsRequiredMembers]
    public Mage()
    {
        Name = nameof(Mage);
        Speed = new Stat() { BaseValue = 10 };
        MaxHealth = new Stat() { BaseValue = 70 };
        Damage = new Stat() { BaseValue = 20 };
        Armor = new Stat() { BaseValue = 0 };
        
        var fireball = new Fireball();
        var strike = new Strike();
        
        Abilities = [fireball, strike];
        Passives = [];

        Instructions = [new AttackClosest(fireball), new AttackClosest(strike)];
        
        Initialize();
    }
}