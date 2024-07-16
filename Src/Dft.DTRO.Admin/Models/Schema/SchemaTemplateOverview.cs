﻿using System.Runtime.Serialization;

public class SchemaTemplateOverview
{
    [DataMember(Name = "schemaVersion")]
    public string SchemaVersion { get; set; }

    [DataMember(Name = "isActive")]
    public bool IsActive { get; set; }

    [DataMember(Name = "rulesExist")]
    public bool RulesExist { get; set; }
}
