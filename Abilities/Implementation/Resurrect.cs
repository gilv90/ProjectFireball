using ProjectFireball.Characters;

namespace ProjectFireball.Abilities.Implementation;

public class Resurrect() : Ability(8), IHeal
{
    protected override void ExecuteImplementation(Character character, Character target)
    {
        target.Heal(character.Damage.Value * 1);
    }

    public override bool CanExecute(Character target)
    {
        if (target.CurrentHealth > 0)
            return false;
        return RemainingCooldown == 0;
    }
}