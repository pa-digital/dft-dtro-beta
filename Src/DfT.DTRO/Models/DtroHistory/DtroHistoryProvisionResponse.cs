﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using DfT.DTRO.Models.SchemaTemplate;

namespace DfT.DTRO.Models.DtroHistory;

public class DtroHistoryProvisionResponse
{
    public string ActionType { get; set; }

    public DateTime? Created { get; set; }

    public ExpandoObject Data { get; set; }
    
    public DateTime? LastUpdated { get; set; }

    public string Reference { get; set; }

    public SchemaVersion SchemaVersion { get; set; }
}