using System.Collections.Generic;
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
    private RandomNumberGenerator _rng = new();

    [Export] public Node2D AlliesContainer { get; set; }
    [Export] public Node2D EnemiesContainer { get; set; }
    [Export] public RichTextLabel LogLabel { get; set; }
    [Export] public PackedScene WarriorScene;
    [Export] public PackedScene MageScene;
    [Export] public PackedScene PriestScene;
    [Export] public TileMapLayer TileMapLayer;

    public void Init(Battle battle)
    {
        _battle = battle ?? GenerateDefaultBattle();
        
        var usedCells = TileMapLayer.GetUsedCells();
        var allyCells = usedCells.Where(c => c.Y == 2).ToList();
        var enemyCells = usedCells.Where(c => c.Y == 8).ToList();
        if (usedCells.Count == 0)
        {
            GD.PrintErr("No used cells found on TileMap; cannot spawn characters.");
            return;
        }
        var index = 0;
        // Spawn allies
        foreach (var ally in _battle.Allies)
        {
            PlaceCharacter(allyCells[index], ally, AlliesContainer);
            index++;
            // AlliesContainer.AddChild(ally);
            
        }

        index = 0;
        // Spawn enemies
        foreach (var enemy in _battle.Enemies)
        {
            // Node2D node2DInstance = enemy.Scene.Instantiate<Node2D>();
            // node2DInstance.Name = enemy.Name;
            //
            // Control wrapper = new Control();
            // wrapper.AddChild(node2DInstance);
            // wrapper.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            // wrapper.SizeFlagsVertical = Control.SizeFlags.Fill;
            //
            // enemy.Visual = node2DInstance;
            PlaceCharacter(enemyCells[index], enemy, AlliesContainer);
            index++;
            // EnemiesContainer.AddChild(enemy);
        }
    }
    
    private Battle GenerateDefaultBattle()
    {
        var warrior1 = WarriorScene.Instantiate<Character>();
        var priest = PriestScene.Instantiate<Character>();
        var warrior2 = WarriorScene.Instantiate<Character>();
        var mage = MageScene.Instantiate<Character>();

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
                .OrderByDescending(c => c.Stats.Priority)
                .FirstOrDefault(c => c.Stats.Priority >= 100);

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
                var result = _battle.Allies.Any(a => a.Stats.CurrentHealth > 0)
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
    
    private void PlaceCharacter(Vector2I cell, Character character, Node2D container)
    {
        // Convert grid cell -> world
        Vector2 worldPos = TileMapLayer.ToGlobal(TileMapLayer.MapToLocal(cell));
        character.Position = worldPos;
        character.GridPosition = cell;

        container.AddChild(character);
    }
}