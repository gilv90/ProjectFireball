using System.Text.Json.Serialization;
using ProjectFireball.Characters;
using ProjectFireball.Passives.Implementation;

namespace ProjectFireball.Passives;

[JsonDerivedType(typeof(ReactiveArmor), nameof(ReactiveArmor))]
public abstract class Passive()
{
    public abstract void Activate();
    public abstract void Deactivate();
}