using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Godot;
using ProjectFireball.Abilities;
using ProjectFireball.Characters.Implementation;
using ProjectFireball.Events;
using ProjectFireball.Instructions;
using ProjectFireball.Passives;
using ProjectFireball.StatusEffects;
using IBuff = ProjectFireball.StatusEffects.IBuff;
using IDebuff = ProjectFireball.StatusEffects.IDebuff;

namespace ProjectFireball.Characters;

[JsonDerivedType(typeof(Mage), nameof(Mage))]
[JsonDerivedType(typeof(Warrior), nameof(Warrior))]
[JsonDerivedType(typeof(Priest), nameof(Priest))]
public partial class Character : Node2D
{
    [Export] public StartingStats StartingStats { get; set; }
    [Export] public InstructionList InstructionList { get; set; }

    public Vector2I GridPosition { get; set; }
    public Stats Stats { get; private set; }
    public List<StatusEffect> StatusEffects { get; set; } = [];
    public List<Passive> Passives { get; set; } = [];

    public List<Instruction> Instructions { get; set; } = [];

    public override void _Ready()
    {
        Stats = GetNode<Stats>("Stats");
        Stats.Initialize(StartingStats);
        
        Instructions = InstructionList.Instructions.ToList();
        
        Passives.ForEach(p => p.Activate());
    }

    public void TakeTurn(Battle battle)
    {
        var instruction = Instructions.FirstOrDefault(i => i.CanExecute(this, battle));
        
        if (instruction != null)
        {
            GD.Print(instruction.Name);
            instruction.Execute(this, battle);
            EventBroker.Publish(new AbilityUsed(instruction.Ability));
        }
        
        foreach (var buff in StatusEffects.OfType<IBuff>())
        {
            buff.Execute();
        }
        foreach (var debuff in StatusEffects.OfType<IDebuff>())
        {
            debuff.Execute();
        }
        StatusEffects = StatusEffects.Where(b => b.RemainingDuration > 0).ToList();

        Instructions.ForEach(i => i.Ability.ReduceCooldown());
        Stats.Priority = 0;
    }

    public void Tick()
    {
        if (Stats == null) return;
        if (Stats.CurrentHealth > 0)
            Stats.Priority += Stats.Speed.Value;
    }

    public void TakeDamage(int damage)
    {
        var mitigatedDamage = damage / (1 + (Stats.Armor.Value / (decimal)10));
        Stats.CurrentHealth = Math.Clamp(Stats.CurrentHealth - (int)mitigatedDamage, 0, Stats.MaxHealth.Value);
        
        EventBroker.Publish(new DamageTaken(this, damage));

        if (Stats.CurrentHealth <= 0)
        {
            Stats.IsDead = true;
            EventBroker.Publish(new CharacterDefeated(this));
            GD.Print($"{Name} has been defeated.");
        }
    }

    public void TakePureDamage(int damage)
    {
        Stats.CurrentHealth -= damage;
    }

    public void Heal(int heal)
    {
        Stats.CurrentHealth = Math.Clamp(Stats.CurrentHealth + heal, 0, Stats.MaxHealth.Value);
        
        if (Stats.CurrentHealth > 0)
            Stats.IsDead = false;
    }
    
    public void Jump(float height = 40f, float duration = 0.3f)
    {
        var tween = GetTree().CreateTween();
        var originalY = Position.Y;

        // Jump up
        tween.TweenProperty(this, "position:y", originalY - height, duration / 2)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.Out);

        // Come down
        tween.TweenProperty(this, "position:y", originalY, duration / 2)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.In);
    }
    
    public void Lunge(float distance = 20f, float duration = 0.2f)
    {
        var tween = GetTree().CreateTween();
        var originalX = Position.X;

        tween.TweenProperty(this, "position:x", originalX + distance, duration / 2)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.Out);

        tween.TweenProperty(this, "position:x", originalX, duration / 2)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.In);
    }
    
    public void FlashRed(float duration = 0.2f)
    {
        // Get the Sprite2D inside the Visual node
        if (GetNodeOrNull<Sprite2D>("Sprite2D") is not Sprite2D sprite) return;

        Color originalColor = sprite.Modulate;
        Color redColor = new Color(1f, 0f, 0f, 1f);

        // Create a Tween from the scene tree
        var tween = GetTree().CreateTween();

        // Tween to red
        tween.TweenProperty(sprite, "modulate", redColor, duration / 2)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.Out);

        // Tween back to original color
        tween.TweenProperty(sprite, "modulate", originalColor, duration / 2)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.In)
            .SetDelay(duration / 2);
    }
    
    public void FlashGreen(float duration = 0.2f)
    {
        // Get the Sprite2D inside the Visual node
        if (GetNodeOrNull<Sprite2D>("Sprite2D") is not Sprite2D sprite) return;

        Color originalColor = sprite.Modulate;
        Color greenColor = new Color(0f, 1f, 0f, 1f);

        // Create a Tween from the scene tree
        var tween = GetTree().CreateTween();

        // Tween to red
        tween.TweenProperty(sprite, "modulate", greenColor, duration / 2)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.Out);

        // Tween back to original color
        tween.TweenProperty(sprite, "modulate", originalColor, duration / 2)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.In)
            .SetDelay(duration / 2);
    }
}