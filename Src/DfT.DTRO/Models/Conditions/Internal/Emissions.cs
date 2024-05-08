using System;
using System.Collections.Generic;
using System.Linq;

namespace DfT.DTRO.Models.Conditions.Internal;

/// <summary>
/// Represents the emission characteristics of a vehicle.
/// </summary>
public class Emissions : IEquatable<Emissions>
{
    /// <summary>
    /// specifies the minimum Euro emission classification
    /// the vehicle(s) have to comply with
    /// according to the 1970 Directive 70/220/EEC and its several amendments.
    /// </summary>
    public string EmissionClassificationEuro { get; init; }

    /// <summary>
    /// specifies optionally multiple free-text descriptions of
    /// classification types for vehicle emissions, distinct from the Euro classifications.
    /// </summary>
    public List<string> EmissionClasifficationOther { get; init; }

    /// <inheritdoc/>
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