using ProjectFireball.Characters;

namespace ProjectFireball.StatusEffects.Implementation;

public class Burning(Character character) :  StatusEffect, IDebuff
{
    public override void Execute()
    {
        character.TakePureDamage((int)(character.MaxHealth.Value * 0.05));
        ReduceDuration();
    }
}