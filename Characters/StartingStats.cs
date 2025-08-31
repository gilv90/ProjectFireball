using Godot;

namespace ProjectFireball.Characters;

[GlobalClass]
public partial class StartingStats : Resource
{
    [Export]public int Speed { get; set; }
    [Export]public int MaxHealth { get; set; }
    [Export]public int Damage { get; set; }
    [Export]public int Armor { get; set; }
}
