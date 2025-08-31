using ProjectFireball.Characters;

namespace ProjectFireball.Abilities.Implementation;

public class Strike() : Ability(0), IAttack
{
    protected override void ExecuteImplementation(Character character, Character target)
    {
        target.TakeDamage(character.Damage.Value);
    }

    public override bool CanExecute(Character target)
    {
        return true;
    }
}