using Godot;
using System;
using ProjectFireball.Characters;
public partial class Stats : Node
{
    public int Priority { get; set; }
    public int CurrentHealth { get; set; }
    public Stat Speed { get; private set; } = new();
    public Stat MaxHealth { get; private set; } = new();
    public Stat Power { get; private set; } = new();
    public Stat Armor { get; private set; } = new();
    public bool IsDead { get; set; }

    public void Initialize(StartingStats startingStats)
    {
        Priority = 0;
        CurrentHealth = startingStats.MaxHealth;
        Speed.BaseValue = startingStats.Speed;
        MaxHealth.BaseValue = startingStats.MaxHealth;
        Power.BaseValue = startingStats.Damage;
        Armor.BaseValue = startingStats.Armor;
    }
}
