﻿namespace DfT.DTRO.Services.Validation.Implementation;

/// <inheritdoc cref="IDtroGroupValidatorService"/>
public class DtroGroupValidatorService : IDtroGroupValidatorService
{
    private readonly IJsonSchemaValidationService _jsonSchemaValidationService;
    private readonly ISchemaTemplateService _schemaTemplateService;
    private readonly ISemanticValidationService _semanticValidationService;
    private readonly IRulesValidation _rulesValidation;
    private readonly IConsultationValidationService _consultationValidationService;
    private readonly ISourceValidationService _sourceValidationService;
    private readonly IProvisionValidationService _provisionValidationService;
    private readonly IRegulatedPlaceValidationService _regulatedPlaceValidationService;
    private readonly IGeometryValidationService _geometryValidationService;
    private readonly IExternalReferenceValidationService _externalReferenceValidationService;
    private readonly IUniqueStreetReferenceNumberValidationService _uniqueStreetReferenceNumberValidationService;
    private readonly IElementaryStreetUnitValidationService _elementaryStreetUnitValidationService;
    private readonly IRegulationValidation _regulationValidation;
    private readonly IConditionValidationService _conditionValidationService;
    private readonly IRateTableValidationService _rateTableValidationService;
    private readonly IRateLineCollectionValidationService _rateLineCollectionValidationService;
    private readonly IRateLineValidationService _rateLineValidationService;
    private readonly IVehicleCharacteristicsValidationService _vehicleCharacteristicsValidationService;
    private readonly IPermitConditionValidationService _permitConditionValidationService;

    /// <inheritdoc cref="IDtroGroupValidatorService"/>
    public DtroGroupValidatorService(
        IJsonSchemaValidationService jsonSchemaValidationService,
        ISemanticValidationService semanticValidationService,
        ISchemaTemplateService schemaTemplateService,
        IRulesValidation rulesValidation,
        IConsultationValidationService consultationValidationService,
        ISourceValidationService sourceValidationService,
        IProvisionValidationService provisionValidationService,
        IRegulatedPlaceValidationService regulatedPlaceValidationService,
        IGeometryValidationService geometryValidationService,
        IExternalReferenceValidationService externalReferenceValidationService,
        IUniqueStreetReferenceNumberValidationService uniqueStreetReferenceNumberValidationService,
        IElementaryStreetUnitValidationService elementaryStreetUnitValidationService,
        IRegulationValidation regulationValidation,
        IConditionValidationService conditionValidationService,
        IRateTableValidationService rateTableValidationService,
        IRateLineCollectionValidationService rateLineCollectionValidationService,
        IRateLineValidationService rateLineValidationService,
        IVehicleCharacteristicsValidationService vehicleCharacteristicsValidationService,
        IPermitConditionValidationService permitConditionValidationService)
    {
        _jsonSchemaValidationService = jsonSchemaValidationService;
        _semanticValidationService = semanticValidationService;
        _schemaTemplateService = schemaTemplateService;
        _rulesValidation = rulesValidation;
        _conditionValidationService = conditionValidationService;
        _sourceValidationService = sourceValidationService;
        _consultationValidationService = consultationValidationService;
        _provisionValidationService = provisionValidationService;
        _regulatedPlaceValidationService = regulatedPlaceValidationService;
        _geometryValidationService = geometryValidationService;
        _externalReferenceValidationService = externalReferenceValidationService;
        _uniqueStreetReferenceNumberValidationService = uniqueStreetReferenceNumberValidationService;
        _elementaryStreetUnitValidationService = elementaryStreetUnitValidationService;
        _regulationValidation = regulationValidation;
        _conditionValidationService = conditionValidationService;
        _rateTableValidationService = rateTableValidationService;
        _rateLineCollectionValidationService = rateLineCollectionValidationService;
        _rateLineValidationService = rateLineValidationService;
        _vehicleCharacteristicsValidationService = vehicleCharacteristicsValidationService;
        _permitConditionValidationService = permitConditionValidationService;
    }

    /// <inheritdoc cref="IDtroGroupValidatorService"/>
    public async Task<DtroValidationException> ValidateDtro(DtroSubmit dtroSubmit, int? traCode)
    {
        var schemaVersion = dtroSubmit.SchemaVersion;
        var schema = await _schemaTemplateService.GetSchemaTemplateAsync(schemaVersion);
        if (schema == null)
        {
            var error = new ApiErrorResponse("Not found", $"Schema version '{schemaVersion}' not found");
            return new DtroValidationException { RequestComparedToSchemaVersion = error };
        }

        if (!schema.IsActive)
        {
            var error = new ApiErrorResponse("Not found", $"Schema version '{schemaVersion}' is not active");
            return new DtroValidationException { RequestComparedToSchemaVersion = error };
        }

        // Temporarily remove schema validation logic while we improve it
        // var jsonSchemaAsString = schema.Template.ToIndentedJsonString();
        // var dtroSubmitJson = dtroSubmit.Data.ToIndentedJsonString();
        // var requestComparedToSchema = _jsonSchemaValidationService.ValidateSchema(jsonSchemaAsString, dtroSubmitJson);
        // if (requestComparedToSchema.Count > 0)
        // {
        //     return new DtroValidationException { RequestComparedToSchema = requestComparedToSchema.ToList() };
        // }

        // Validation of camel case for schemas >= 3.3.2
        CasingValidationService casingValidationService = new();
        if (casingValidationService.SchemaVersionEnforcesCamelCase(dtroSubmit.SchemaVersion))
        {
            List<string> invalidProperties = casingValidationService.ValidateCamelCase(dtroSubmit.Data);
            if (invalidProperties.Count > 0)
            {
                string message = $"All property names must conform to camel case naming conventions. The following properties violate this: [{string.Join(", ", invalidProperties)}]";
                throw new CaseException(message);
            }

            // Here, we turn all the Dtro object keys into Pascal case
            dtroSubmit.Data = casingValidationService.ConvertKeysToPascalCase(dtroSubmit.Data);
        }
        else
        {
            List<string> invalidProperties = casingValidationService.ValidatePascalCase(dtroSubmit.Data);
            if (invalidProperties.Count > 0)
            {
                string message = $"All property names must conform to pascal case naming conventions. The following properties violate this: [{string.Join(", ", invalidProperties)}]";
                throw new CaseException(message);
            }
        }

        var errors = _consultationValidationService.Validate(dtroSubmit);
        if (errors.Count > 0)
        {
            return new DtroValidationException { RequestComparedToRules = errors.MapFrom() };
        }

        errors = _sourceValidationService.Validate(dtroSubmit, traCode);
        if (errors.Count > 0)
        {
            return new DtroValidationException { RequestComparedToRules = errors.MapFrom() };
        }

        errors = _provisionValidationService.Validate(dtroSubmit);
        if (errors.Count > 0)
        {
            return new DtroValidationException { RequestComparedToRules = errors.MapFrom() };
        }

        errors = _regulatedPlaceValidationService.Validate(dtroSubmit);
        if (errors.Count > 0)
        {
            return new DtroValidationException { RequestComparedToRules = errors.MapFrom() };
        }

        errors = _geometryValidationService.Validate(dtroSubmit);
        if (errors.Count > 0)
        {
            return new DtroValidationException { RequestComparedToRules = errors.MapFrom() };
        }

        errors = _externalReferenceValidationService.Validate(dtroSubmit);
        if (errors.Count > 0)
        {
            return new DtroValidationException { RequestComparedToRules = errors.MapFrom() };
        }

        errors = _uniqueStreetReferenceNumberValidationService.Validate(dtroSubmit);
        if (errors.Count > 0)
        {
            return new DtroValidationException { RequestComparedToRules = errors.MapFrom() };
        }

        errors = _elementaryStreetUnitValidationService.Validate(dtroSubmit);
        if (errors.Count > 0)
        {
            return new DtroValidationException { RequestComparedToRules = errors.MapFrom() };
        }

        errors = await _rulesValidation.ValidateRules(dtroSubmit, schemaVersion.ToString());
        if (errors.Count > 0)
        {
            return new DtroValidationException { RequestComparedToRules = errors.MapFrom() };
        }

        errors = _regulationValidation.ValidateRegulation(dtroSubmit, schemaVersion);
        if (errors.Count > 0)
        {
            return new DtroValidationException { RequestComparedToRules = errors.MapFrom() };
        }

        errors = _conditionValidationService.ValidateCondition(dtroSubmit, schemaVersion);
        if (errors.Count > 0)
        {
            return new DtroValidationException { RequestComparedToRules = errors.MapFrom() };
        }

        errors = _rateTableValidationService.Validate(dtroSubmit);
        if (errors.Count > 0)
        {
            return new DtroValidationException { RequestComparedToRules = errors.MapFrom() };
        }

        errors = _rateLineCollectionValidationService.Validate(dtroSubmit);
        if (errors.Count > 0)
        {
            return new DtroValidationException { RequestComparedToRules = errors.MapFrom() };
        }

        errors = _rateLineValidationService.Validate(dtroSubmit);
        if (errors.Count > 0)
        {
            return new DtroValidationException { RequestComparedToRules = errors.MapFrom() };
        }

        errors = _vehicleCharacteristicsValidationService.Validate(dtroSubmit);
        if (errors.Count > 0)
        {
            return new DtroValidationException { RequestComparedToRules = errors.MapFrom() };
        }

        errors = _permitConditionValidationService.Validate(dtroSubmit);
        if (errors.Count > 0)
        {
            return new DtroValidationException { RequestComparedToRules = errors.MapFrom() };
        }

        var tuple = await _semanticValidationService.ValidateCreationRequest(dtroSubmit);
        if (tuple.Item2.Count > 0)
        {
            return new DtroValidationException { RequestComparedToRules = tuple.Item2.MapFrom() };
        }

        return null;
    }
}