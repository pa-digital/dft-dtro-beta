using DfT.DTRO.Models.Validation;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;

namespace DfT.DTRO.Models.Errors;

/// <summary>
/// Model for capturing semantic validation errors.
/// </summary>
public class DtroValidationException : Exception
{
    public DtroValidationException()
       : base("Dtro Validation Failure")
    {
    }

    public DtroValidationException(string message) : base(message)
    {
    }

    public ApiErrorResponse RequestComparedToSchemaVersion { get; set; }

    public List<ValidationError> RequestComparedToSchema { get; set; }

    public List<SemanticValidationError> RequestComparedToRules { get; set; }
}