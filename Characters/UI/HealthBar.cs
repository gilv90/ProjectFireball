using Godot;
using System;

public partial class HealthBar : ProgressBar
{
    [Export] private Stats _stats;
    
    public override void _Ready()
    {
        MaxValue = _stats.MaxHealth.Value;
        Value = Mathf.Clamp(_stats.CurrentHealth, 0, MaxValue);
    }

    public override void _Process(double delta)
    {
        MaxValue = _stats.MaxHealth.Value;
        Value = Mathf.Clamp(_stats.CurrentHealth, 0, MaxValue);
    }
}
