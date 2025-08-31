using ProjectFireball.Characters;
using ProjectFireball.Events;

namespace ProjectFireball.Passives.Implementation;

public class ReactiveArmor(Character character) : Passive
{
    public override void Activate()
    {
        EventBroker.Subscribe<DamageTaken>(OnCharacterDamageTaken);
    }

    public override void Deactivate()
    {
        EventBroker.Unsubscribe<DamageTaken>(OnCharacterDamageTaken);
    }

    private void OnCharacterDamageTaken(DamageTaken damageTaken)
    {
        if (damageTaken.Character == character)
        {
            character.Stats.Armor.AddModifier(new StatModifier(1, ModifierType.Flat, this));
            // Console.WriteLine($"Reactive Armor increases {character.Name}'s armor to {character.Armor.Value}");
        }
    }
}