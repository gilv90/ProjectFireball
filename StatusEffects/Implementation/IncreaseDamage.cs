namespace ProjectFireball.StatusEffects.Implementation;

public class IncreaseDamage : StatusEffect, IBuff
{
    public override void Execute()
    {
        ReduceDuration();
    }
}