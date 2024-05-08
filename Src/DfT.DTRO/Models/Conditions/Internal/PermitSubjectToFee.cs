using System;

namespace DfT.DTRO.Models.Conditions;

/// <summary>
/// Represents the PermitSubjectToFee data related to <see cref="PermitCondition"/>.
/// </summary>
public class PermitSubjectToFee
{
    /// <summary>
    ///  indicates the monetary amount, in pounds sterling,
    ///  related to use of or purchase of permit.
    /// </summary>
    public decimal AmountDue { get; init; }

    /// <summary>
    /// expresses the maximum duration that is permitted in
    /// relationship to use of this permit.
    /// </summary>
    public TimeSpan MaximumAccessDuration { get; init; }

    /// <summary>
    /// expresses the minimum duration between last use
    /// of permit and next permitted use of permit.
    /// </summary>
    public TimeSpan MinimumTimeToNextEntry { get; init; }

    /// <summary>
    /// provides a web address (URL) for further information on
    /// permit and related payment.
    /// </summary>
    public Uri PaymentInformation { get; init; }
}
