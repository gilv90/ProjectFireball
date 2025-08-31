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
public abstract partial class Character : Node2D
{
    public Node2D Visual { get; set; }
    [Export] 
    public PackedScene Scene;
    public int CurrentHealth { get; private set; }

    public List<StatusEffect> StatusEffects { get; set; } = [];
    public int Priority { get; set; }

    public Stat Speed { get; set; }

    public Stat MaxHealth { get; set; }

    public Stat Damage { get; set; }

    public Stat Armor { get; set; }

    public List<Ability> Abilities { get; set; }
    public List<Passive> Passives { get; set; }

    public List<Instruction> Instructions { get; set; }

    public void TakeTurn(Battle battle)
    {
        var instruction = Instructions.FirstOrDefault(i => i.CanExecute(this, battle));
        if (instruction != null)
        {
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

        Abilities.ForEach(a => a.ReduceCooldown());
        Priority = 0;
    }

    public void Tick()
    {
        if (CurrentHealth > 0)
            Priority += Speed.Value;
    }

    protected void Initialize()
    {
        CurrentHealth = MaxHealth.Value;
        Passives.ForEach(p => p.Activate());
    }

    public void TakeDamage(int damage)
    {
        var mitigatedDamage = damage / (1 + (Armor.Value / (decimal)10));
        CurrentHealth -= (int)mitigatedDamage;
        
        if (CurrentHealth <= 0)
            CurrentHealth = 0;
        
        EventBroker.Publish(new DamageTaken(this, damage));

        if (CurrentHealth <= 0)
        {
            EventBroker.Publish(new CharacterDefeated(this));
            GD.Print($"{Name} has been defeated.");
        }
    }

    public void TakePureDamage(int damage)
    {
        CurrentHealth -= damage;
    }

    public void Heal(int heal)
    {
        CurrentHealth += heal;
        if (CurrentHealth > MaxHealth.Value)
            CurrentHealth = MaxHealth.Value;
    }
    
    public void Jump(float height = 40f, float duration = 0.3f)
    {
        if (Visual == null) return;

        var tween = Visual.GetTree().CreateTween();
        var originalY = Visual.Position.Y;

        // Jump up
        tween.TweenProperty(Visual, "position:y", originalY - height, duration / 2)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.Out);

        // Come down
        tween.TweenProperty(Visual, "position:y", originalY, duration / 2)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.In);
    }
    
    public void Lunge(float distance = 20f, float duration = 0.2f)
    {
        if (Visual == null) return;

        var tween = Visual.GetTree().CreateTween();
        var originalX = Visual.Position.X;

        tween.TweenProperty(Visual, "position:x", originalX + distance, duration / 2)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.Out);

        tween.TweenProperty(Visual, "position:x", originalX, duration / 2)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.In);
    }
    
    public void FlashRed(float duration = 0.2f)
    {
        if (Visual == null) return;

        // Get the Sprite2D inside the Visual node
        if (Visual.GetNodeOrNull<Sprite2D>("Sprite2D") is not Sprite2D sprite) return;

        Color originalColor = sprite.Modulate;
        Color redColor = new Color(1f, 0f, 0f, 1f);

        // Create a Tween from the scene tree
        var tween = Visual.GetTree().CreateTween();

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
        if (Visual == null) return;

        // Get the Sprite2D inside the Visual node
        if (Visual.GetNodeOrNull<Sprite2D>("Sprite2D") is not Sprite2D sprite) return;

        Color originalColor = sprite.Modulate;
        Color greenColor = new Color(0f, 1f, 0f, 1f);

        // Create a Tween from the scene tree
        var tween = Visual.GetTree().CreateTween();

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