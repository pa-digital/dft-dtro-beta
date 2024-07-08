using System.Runtime.Serialization;

public class SchemaTemplateOverview
{
    /// <summary>
    /// Gets or sets the schema identifier of the SchemaTemplate payload being submitted.
    /// </summary>
    /// <example>3.1.1.</example>
    [DataMember(Name = "schemaVersion")]
    public string SchemaVersion { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the template is active.
    /// </summary>
    [DataMember(Name = "isActive")]
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a rules record exists for this schema.
    /// </summary>
    [DataMember(Name = "rulesExist")]
    public bool RulesExist { get; set; }
}
