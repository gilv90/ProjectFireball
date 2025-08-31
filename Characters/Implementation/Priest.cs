using System.Diagnostics.CodeAnalysis;
using Godot;
using ProjectFireball.Abilities.Implementation;
using ProjectFireball.Instructions.Implementation;

namespace ProjectFireball.Characters.Implementation;

public partial class Priest : Character
{
    [SetsRequiredMembers]
    public Priest()
    {
        Name = nameof(Priest);
        // Speed = new Stat() { BaseValue = 10 };
        // MaxHealth = new Stat() { BaseValue = 70 };
        // Damage = new Stat() { BaseValue = 20 };
        // Armor = new Stat() { BaseValue = 0 };
        
        var resurrect = new Resurrect();
        var minorHeal = new MinorHeal();
        var strike = new Strike();
        
        // Abilities = [resurrect, minorHeal, strike];
        Passives = [];

        // Instructions = [new HealWoundedAlly(resurrect), new HealWoundedAlly(minorHeal), new AttackClosest(strike)];
    }
}