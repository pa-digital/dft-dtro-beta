using DfT.DTRO.Models.Conditions.ValueRules;

namespace DfT.DTRO.Models.Conditions.Internal;

public class VehicleCharacteristics
{
    public List<string> FuelType { get; init; } = new();

    public string LoadType { get; init; }

    public string VehicleEquipment { get; init; }

    public List<string> VehicleType { get; init; } = new();

    public string VehicleUsage { get; init; }

    public int? YearOfFirstRegistration { get; init; }

    public Emissions Emissions { get; init; }

    [ValueRulePropertyNames("comparisonOperator", "grossVehicleWeight")]
    public IValueRule<double> GrossWeightCharacteristic { get; init; }

    [ValueRulePropertyNames("comparisonOperator", "vehicleHeight")]
    public IValueRule<double> HeightCharacteristic { get; init; }

    [ValueRulePropertyNames("comparisonOperator", "vehicleLength")]
    public IValueRule<double> LengthCharacteristic { get; init; }

    [ValueRulePropertyNames("comparisonOperator", "vehicleWidth")]
    public IValueRule<double> WidthCharacteristic { get; init; }

    [ValueRulePropertyNames("comparisonOperator", "heaviestAxleWeight")]
    public IValueRule<double> HeaviestAxleWeightCharacteristic { get; init; }

    [ValueRulePropertyNames("comparisonOperator", "numberOfAxles")]
    public IValueRule<int> NumberOfAxlesCharacteristic { get; init; }

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