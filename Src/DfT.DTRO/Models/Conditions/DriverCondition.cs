using DfT.DTRO.Models.Conditions.Base;
using DfT.DTRO.Models.Conditions.ValueRules;
using System.Text.Json.Serialization;

namespace DfT.DTRO.Models.Conditions;

/// <summary>
/// The types of license referenced in <see cref="DriverCondition"/>.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum LicenseCharacteristicsType
{
    /// <summary>
    /// A permanent license.
    /// </summary>
    Permanent,

    /// <summary>
    /// A provisional license.
    /// </summary>
    Provisional
}

/// <summary>
/// Represents a condition regarding the driver.
/// </summary>
public class DriverCondition : Condition
{
    /// <summary>
    /// Indicates a specific type of driver characteristic.
    /// Permissible values include ‘disabledWithPermit’, ‘learnerDriver’, ‘localResident’, etc.
    /// </summary>
    public string DriverCharacteristicsType { get; init; }

    /// <summary>
    /// Indicates a characteristic relating to the driver's license.
    /// </summary>
    public LicenseCharacteristicsType? LicenseCharacteristicsType { get; init; }

    /// <summary>
    /// Indicates the allowed age of driver.
    /// </summary>
    public IValueRule<int> AgeOfDriver { get; init; }

    /// <summary>
    /// Indicates the allowed time of license ownership.
    /// </summary>
    public IValueRule<int> TimeDriversLicenseHeld { get; init; }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
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

    /// <inheritdoc/>
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
