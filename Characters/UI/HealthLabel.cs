using Godot;
using System;

public partial class HealthLabel : Label
{
    [Export] private Stats _stats;
    
    public override void _Ready()
    {
        Text = $"{_stats.CurrentHealth}/{_stats.MaxHealth.Value}";
    }

    public override void _Process(double delta)
    {
        Text = $"{_stats.CurrentHealth}/{_stats.MaxHealth.Value}";
    }
}
