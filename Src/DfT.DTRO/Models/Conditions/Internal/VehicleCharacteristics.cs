using DfT.DTRO.Models.Conditions.ValueRules;
using System.Collections.Generic;
using System.Linq;

namespace DfT.DTRO.Models.Conditions.Internal;

/// <summary>
/// Specifies the vehicle characteristics.
/// </summary>
public class VehicleCharacteristics
{
    /// <summary>
    /// specifies optionally multiple fuel types used by vehicles.
    /// Permissible fuel types include ‘petrol’, ‘diesel’, ‘hydrogen’, ‘lpg’, ‘battery’, etc.
    /// </summary>
    public List<string> FuelType { get; init; } = new ();

    /// <summary>
    /// specifies optionally one type of load carried by the vehicle.
    /// Permissible load types include ‘empty’, ‘dangerousGoods’, ‘explosiveMaterials’.
    /// </summary>
    public string LoadType { get; init; }

    /// <summary>
    /// specifies optionally one type of vehicle equipment.
    /// Permissible equipment types include ‘snowChainsInUse’,
    /// ‘dippedHeadlightsInUse’, ‘electronicTollEquipment’, etc.
    /// </summary>
    public string VehicleEquipment { get; init; }

    /// <summary>
    /// specifies optionally multiple types of vehicle.
    /// Permissible vehicle types include ‘taxi’, ‘pedalCycle’,
    /// ‘car’, ‘goodsVehicle’, ‘bus’, etc.
    /// </summary>
    public List<string> VehicleType { get; init; } = new ();

    /// <summary>
    /// specifies optionally one purpose the vehicle is being used for.
    /// Permissible vehicle usage types include ‘accessToOffStreetPremises’,
    /// ‘localBuses’, ‘privateHireVehicle’, ‘guidedBus’, etc.
    /// </summary>
    public string VehicleUsage { get; init; }

    /// <summary>
    /// specifies optionally the year of registration.
    /// </summary>
    public int? YearOfFirstRegistration { get; init; }

    /// <summary>
    /// Specifies optionally the emission characteristics of the vehicle.
    /// </summary>
    public Emissions Emissions { get; init; }

    /// <summary>
    /// Specifies optionally the allowed weight of the vehicle.
    /// </summary>
    [ValueRulePropertyNames("comparisonOperator", "grossVehicleWeight")]
    public IValueRule<double> GrossWeightCharacteristic { get; init; }

    /// <summary>
    /// Specifies optionally the allowed height of the vehicle.
    /// </summary>
    [ValueRulePropertyNames("comparisonOperator", "vehicleHeight")]
    public IValueRule<double> HeightCharacteristic { get; init; }

    /// <summary>
    /// Specifies optionally the allowed length of the vehicle.
    /// </summary>
    [ValueRulePropertyNames("comparisonOperator", "vehicleLength")]
    public IValueRule<double> LengthCharacteristic { get; init; }

    /// <summary>
    /// Specifies optionally the allowed width of the vehicle.
    /// </summary>
    [ValueRulePropertyNames("comparisonOperator", "vehicleWidth")]
    public IValueRule<double> WidthCharacteristic { get; init; }

    /// <summary>
    /// Specifies optionally the heavies allowed axle of the vehicle.
    /// </summary>
    [ValueRulePropertyNames("comparisonOperator", "heaviestAxleWeight")]
    public IValueRule<double> HeaviestAxleWeightCharacteristic { get; init; }

    /// <summary>
    /// Specifies optionally the number of axles allowed in the vehicle.
    /// </summary>
    [ValueRulePropertyNames("comparisonOperator", "numberOfAxles")]
    public IValueRule<int> NumberOfAxlesCharacteristic { get; init; }

    /// <summary>
    /// Checks if the current <see cref="VehicleCharacteristics"/>
    /// contradict the <see cref="VehicleCharacteristics"/>
    /// specified in <paramref name="other"/>.
    /// </summary>
    /// <param name="other">The characteristics to compare against.</param>
    /// <param name="invertThis">Should these characteristics be inverted.</param>
    /// <param name="invertOther">Should the other characteristics be inverted.</param>
    /// <returns>
    /// <see langword="true"/> if these characteristics contradict <paramref name="other"/>;
    /// otherwise <see langword="false"/>.
    /// </returns>
    public bool Contradicts(VehicleCharacteristics other, bool invertThis = false, bool invertOther = false)
    {
        if (FuelType.Except(other.FuelType).Any() || other.FuelType.Except(FuelType).Any())
        {
            return false;
        }

        if (VehicleType.Except(other.VehicleType).Any() || other.VehicleType.Except(VehicleType).Any())
        {
            return false;
        }

        if (LoadType != other.LoadType
            || YearOfFirstRegistration != other.YearOfFirstRegistration
            || !(Emissions?.Equals(other?.Emissions) ?? true)
            || VehicleEquipment != other.VehicleEquipment
            || VehicleUsage != other.VehicleUsage)
        {
            return false;
        }

        var grossWeightCharacteristic = GrossWeightCharacteristic?.MaybeInverted(invertThis);
        var heightCharacteristic = HeightCharacteristic?.MaybeInverted(invertThis);
        var lengthCharacteristic = LengthCharacteristic?.MaybeInverted(invertThis);
        var widthCharacteristic = WidthCharacteristic?.MaybeInverted(invertThis);
        var heaviestAxleWeightCharacteristic = HeaviestAxleWeightCharacteristic?.MaybeInverted(invertThis);
        var numberOfAxlesCharacteristic = NumberOfAxlesCharacteristic?.MaybeInverted(invertThis);

        var otherGrossWeightCharacteristic = other.GrossWeightCharacteristic?.MaybeInverted(invertOther);
        var otherHeightCharacteristic = other.HeightCharacteristic?.MaybeInverted(invertOther);
        var otherLengthCharacteristic = other.LengthCharacteristic?.MaybeInverted(invertOther);
        var otherWidthCharacteristic = other.WidthCharacteristic?.MaybeInverted(invertOther);
        var otherHeaviestAxleWeightCharacteristic = other.HeaviestAxleWeightCharacteristic?.MaybeInverted(invertOther);
        var otherNumberOfAxlesCharacteristic = other.NumberOfAxlesCharacteristic?.MaybeInverted(invertOther);

        return
            (grossWeightCharacteristic?.Contradicts(otherGrossWeightCharacteristic) ?? false)
            || (heightCharacteristic?.Contradicts(otherHeightCharacteristic) ?? false)
            || (lengthCharacteristic?.Contradicts(otherLengthCharacteristic) ?? false)
            || (widthCharacteristic?.Contradicts(otherWidthCharacteristic) ?? false)
            || (heaviestAxleWeightCharacteristic?.Contradicts(otherHeaviestAxleWeightCharacteristic) ?? false)
            || (numberOfAxlesCharacteristic?.Contradicts(otherNumberOfAxlesCharacteristic) ?? false);
    }
}