using System.Text.Json.Serialization;
using DfT.DTRO.Models.Conditions.Base;
using DfT.DTRO.Models.Conditions.ValueRules;

namespace DfT.DTRO.Models.Conditions;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum LicenseCharacteristicsType
{
    Permanent,

    Provisional
}

public class DriverCondition : Condition
{
    public string DriverCharacteristicsType { get; init; }

    public LicenseCharacteristicsType? LicenseCharacteristicsType { get; init; }

    public IValueRule<int> AgeOfDriver { get; init; }

    public IValueRule<int> TimeDriversLicenseHeld { get; init; }

    public override object Clone()
    {
        return new DriverCondition
        {
            DriverCharacteristicsType = DriverCharacteristicsType,
            LicenseCharacteristicsType = LicenseCharacteristicsType,
            AgeOfDriver = AgeOfDriver,
            TimeDriversLicenseHeld = TimeDriversLicenseHeld,
            Negate = Negate
        };
    }

    public override bool Contradicts(Condition other)
    {
        if (other is not DriverCondition otherDriverCondition
            || DriverCharacteristicsType != otherDriverCondition.DriverCharacteristicsType
            || LicenseCharacteristicsType != otherDriverCondition.LicenseCharacteristicsType)
        {
            return false;
        }

        var ageOfDriver = AgeOfDriver?.MaybeInverted(Negate);
        var timeDriversLicenseHeld = TimeDriversLicenseHeld?.MaybeInverted(Negate);

        var otherAgeOfDriver = otherDriverCondition?.AgeOfDriver?.MaybeInverted(otherDriverCondition.Negate);
        var otherTimeDriversLicenseHeld = otherDriverCondition?.TimeDriversLicenseHeld?.MaybeInverted(otherDriverCondition.Negate);

        return (ageOfDriver?.Contradicts(otherAgeOfDriver) ?? false)
            || (timeDriversLicenseHeld?.Contradicts(otherTimeDriversLicenseHeld) ?? false);
    }

    public override Condition Negated()
    {
        return new DriverCondition
        {
            DriverCharacteristicsType = DriverCharacteristicsType,
            LicenseCharacteristicsType = LicenseCharacteristicsType,
            AgeOfDriver = AgeOfDriver,
            TimeDriversLicenseHeld = TimeDriversLicenseHeld,
            Negate = !Negate
        };
    }
}
