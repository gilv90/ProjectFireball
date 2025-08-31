using Godot;
using System;

public partial class HealthBar : ProgressBar
{
    [Export] private Stats _stats;
    private StyleBoxFlat _fillStyle;
    
    public override void _Ready()
    {
        MaxValue = _stats.MaxHealth.Value;
        Value = Mathf.Clamp(_stats.CurrentHealth, 0, MaxValue);
        
        _fillStyle = new StyleBoxFlat();
        AddThemeStyleboxOverride("fill", _fillStyle);

        UpdateColor();
    }

    public override void _Process(double delta)
    {
        MaxValue = _stats.MaxHealth.Value;
        Value = Mathf.Clamp(_stats.CurrentHealth, 0, MaxValue);
        
        UpdateColor();
    }
    
    private void UpdateColor()
    {
        float healthPercent = (float)_stats.CurrentHealth / (float)_stats.MaxHealth.Value;

        // Lerp between red (low HP) and green (full HP)
        _fillStyle.BgColor = new Color(1 - healthPercent, healthPercent, 0);
    }
}
