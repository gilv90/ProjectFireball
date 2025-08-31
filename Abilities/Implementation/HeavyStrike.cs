using ProjectFireball.Characters;

namespace ProjectFireball.Abilities.Implementation;

public class HeavyStrike() : Ability(3), IAttack
{
    protected override void ExecuteImplementation(Character character, Character target)
    {
        target.TakeDamage(character.Damage.Value * 2);
    }

    public override bool CanExecute(Character target)
    {
        return RemainingCooldown == 0;
    }
}