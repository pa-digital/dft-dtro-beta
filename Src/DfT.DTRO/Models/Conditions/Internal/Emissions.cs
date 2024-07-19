namespace DfT.DTRO.Models.Conditions.Internal;

public class Emissions : IEquatable<Emissions>
{
    public string EmissionClassificationEuro { get; init; }

    public List<string> EmissionClasifficationOther { get; init; }

    public bool Equals(Emissions other)
    {
        if (other is null)
        {
            return false;
        }

        return EmissionClassificationEuro == other.EmissionClassificationEuro
            && !EmissionClasifficationOther.Except(other.EmissionClasifficationOther).Any()
            && !other.EmissionClasifficationOther.Except(EmissionClasifficationOther).Any();
    }
}