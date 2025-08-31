using Godot;
using ProjectFireball.Characters;

public partial class PriorityBar : ProgressBar
{
    [Export] private Stats _stats;

    public override void _Ready()
    {
        Value = Mathf.Clamp(_stats.Priority, 0, (int)MaxValue);
        
    }

    public override void _Process(double delta)
    {
        Value = _stats.Priority > 100 ? 100 : _stats.Priority;
    }
}
