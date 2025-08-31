using Godot;
using System.Linq;
using System.Threading.Tasks;
using ProjectFireball;
using ProjectFireball.Characters;
using ProjectFireball.Characters.Implementation;

public partial class BattleController : Node
{
    private Battle _battle;
    private GameStatus _status = GameStatus.ReadyToStart;

    [Export] public HBoxContainer AlliesContainer { get; set; }
    [Export] public HBoxContainer EnemiesContainer { get; set; }
    [Export] public RichTextLabel LogLabel { get; set; }
    [Export] public PackedScene WarriorScene;
    [Export] public PackedScene MageScene;
    [Export] public PackedScene PriestScene;

    public void Init(Battle battle)
    {
        _battle = battle ?? GenerateDefaultBattle();
        
        AlliesContainer.Alignment = BoxContainer.AlignmentMode.Center;
        EnemiesContainer.Alignment = BoxContainer.AlignmentMode.Center;
        
        // Spawn allies
        foreach (var ally in _battle.Allies)
        {
            Node2D node2DInstance = ally.Scene.Instantiate<Node2D>();
            node2DInstance.Name = ally.Name;

            // Wrap in a Control so HBoxContainer can layout it
            Control wrapper = new Control();
            wrapper.AddChild(node2DInstance);

            // Set size flags so HBoxContainer spreads it evenly
            wrapper.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            wrapper.SizeFlagsVertical = Control.SizeFlags.Fill;

            ally.Visual = node2DInstance;
            AlliesContainer.AddChild(wrapper);
            
        }

        // Spawn enemies
        foreach (var enemy in _battle.Enemies)
        {
            Node2D node2DInstance = enemy.Scene.Instantiate<Node2D>();
            node2DInstance.Name = enemy.Name;

            Control wrapper = new Control();
            wrapper.AddChild(node2DInstance);
            wrapper.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            wrapper.SizeFlagsVertical = Control.SizeFlags.Fill;

            enemy.Visual = node2DInstance;
            EnemiesContainer.AddChild(wrapper);
        }
    }
    
    private Battle GenerateDefaultBattle()
    {
        var warrior1 = new Warrior() { Name = "Warrior1", Scene = WarriorScene };
        var priest = new Priest() { Name = "Priest", Scene = PriestScene };
        var warrior2 = new Warrior() { Name = "Warrior2", Scene = WarriorScene };
        var mage = new Mage() { Name = "Mage", Scene = MageScene };

        return new Battle()
        {
            Allies = [warrior1, priest],
            Enemies = [warrior2, mage],
            Characters = [warrior1, priest, warrior2, mage]
        };
    }

    public async Task StartBattle()
    {
        if (_battle == null)
        {
            GD.PrintErr("BattleController: No battle provided!");
            return;
        }

        _status = GameStatus.Running;

        while (_status == GameStatus.Running)
        {
            GD.Print("Tick");
            _battle.Characters.ForEach(c => c.Tick());

            var activeCharacter = _battle.Characters
                .OrderByDescending(c => c.Priority)
                .FirstOrDefault(c => c.Priority >= 100);

            if (activeCharacter != null)
            {
                Log($"{activeCharacter.Name}'s turn!");
                activeCharacter.TakeTurn(_battle);
                await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
            }

            if (_battle.IsBattleOver())
            {
                _status = GameStatus.GameOver;
                _battle.Characters.ForEach(c => c.Passives.ForEach(p => p.Deactivate()));
                var result = _battle.Allies.Any(a => a.CurrentHealth > 0)
                    ? "You win!"
                    : "You lose!";
                Log(result);
            }

            await ToSignal(GetTree().CreateTimer(0.1f), "timeout"); // prevent freeze
        }
    }

    private void Log(string message)
    {
        GD.Print(message);
        LogLabel?.AppendText(message + "\n");
    }
}