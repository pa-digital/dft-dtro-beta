namespace DfT.DTRO.Models.Conditions;

public class PermitSubjectToFee
{
    public decimal AmountDue { get; init; }

    public TimeSpan MaximumAccessDuration { get; init; }

    public TimeSpan MinimumTimeToNextEntry { get; init; }

    public Uri PaymentInformation { get; init; }
}
