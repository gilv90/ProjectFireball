using Godot;
using System.Collections.Generic;
using ProjectFireball.Characters;
using ProjectFireball.Characters.Implementation;

namespace ProjectFireball
{
    public partial class MenuUi : Control
    {
        private async void OnStartButtonPressed()
        {
            var allies = new List<Character>
            {
                new Warrior(){Name = "Warrior1"},
                new Priest(){Name = "Priest"}
            };
            var enemies = new List<Character>
            {
                new Warrior(){Name = "Warrior2"},
                new Mage(){Name = "Mage"}
            };

            var characters = new List<Character>();
            characters.AddRange(allies);
            characters.AddRange(enemies);

            var battle = new Battle()
            {
                Allies = allies,
                Enemies = enemies,
                Characters = characters
            };

            // Load the battle scene
            var battleScene = GD.Load<PackedScene>("res://battle.tscn");
            var battleRoot = battleScene.Instantiate();

            // Pass the battle to the controller
            var controller = battleRoot.GetNode<BattleController>("BattleController");
            controller.Init(null);

            GetTree().Root.AddChild(battleRoot);
            Hide();
            
            await controller.StartBattle();
            
            battleRoot.QueueFree();
            Show();
        }

        private void OnQuitButtonPressed()
        {
            GetTree().Quit();
        }
    }
}