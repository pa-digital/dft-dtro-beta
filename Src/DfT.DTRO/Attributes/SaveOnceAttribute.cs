namespace DfT.DTRO.Attributes;

/// <summary>
/// Attribute that makes sure property only gets saved once
/// by <see cref="IDtroService"/> implementation when document is created.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class SaveOnceAttribute : Attribute
{
}
