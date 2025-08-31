using System.Collections.Generic;
using System.Linq;
using ProjectFireball.Characters;

namespace ProjectFireball;

public class Battle
{
    public required List<Character> Allies;
    public required List<Character> Enemies;
    public required List<Character> Characters;

    public List<Character> GetAllies(Character character)
    {
        return Allies.Contains(character) ? Allies : Enemies;
    }
    
    public List<Character> GetEnemies(Character character)
    {
        return Allies.Contains(character) ? Enemies : Allies;
    }

    public bool IsBattleOver()
    {
        return Allies.All(c => c.Stats.CurrentHealth <= 0) || Enemies.All(c => c.Stats.CurrentHealth <= 0);
    }
}