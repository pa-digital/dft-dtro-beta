using System;

namespace DfT.DTRO.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class SaveOnceAttribute : Attribute
{
}
