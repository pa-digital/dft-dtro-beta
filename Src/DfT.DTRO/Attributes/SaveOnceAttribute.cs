using DfT.DTRO.Services;
using System;

namespace DfT.DTRO.Attributes;

/// <summary>
/// Attribute that makes sure property only gets saved to
/// by <see cref="IDtroService"/> implementations
/// when the document is created.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class SaveOnceAttribute : Attribute
{
}
