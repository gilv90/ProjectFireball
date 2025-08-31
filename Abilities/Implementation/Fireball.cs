using ProjectFireball.Characters;

namespace ProjectFireball.Abilities.Implementation;

public class Fireball() : Ability(5), IAttack
{
    protected override void ExecuteImplementation(Character character, Character target)
    {
        target.TakeDamage(character.Damage.Value * 3);
    }

    public override bool CanExecute(Character target)
    {
        return RemainingCooldown == 0;
    }
}