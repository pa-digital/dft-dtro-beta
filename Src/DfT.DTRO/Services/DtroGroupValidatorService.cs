using DfT.DTRO.Extensions.Exceptions;

namespace DfT.DTRO.Services;
public class DtroGroupValidatorService : IDtroGroupValidatorService
{
    private readonly IJsonSchemaValidationService _jsonSchemaValidationService;
    private readonly ISchemaTemplateService _schemaTemplateService;
    private readonly ISemanticValidationService _semanticValidationService;
    private readonly IRulesValidation _rulesValidation;
    private readonly ISourceValidationService _sourceValidationService;
    private readonly IProvisionValidationService _provisionValidationService;
    private readonly IRegulatedPlaceValidationService _regulatedPlaceValidationService;
    private readonly IGeometryValidationService _geometryValidationService;
    private readonly IRegulationValidation _regulationValidation;
    private readonly IConditionValidation _conditionValidation;

    public DtroGroupValidatorService(
        IJsonSchemaValidationService jsonSchemaValidationService,
        ISemanticValidationService semanticValidationService,
        ISchemaTemplateService schemaTemplateService,
        IRulesValidation rulesValidation,
        ISourceValidationService sourceValidationService,
        IProvisionValidationService provisionValidationService,
        IRegulatedPlaceValidationService regulatedPlaceValidationService,
        IGeometryValidationService geometryValidationService,
        IRegulationValidation regulationValidation,
        IConditionValidation conditionValidation)
    {
        _jsonSchemaValidationService = jsonSchemaValidationService;
        _semanticValidationService = semanticValidationService;
        _schemaTemplateService = schemaTemplateService;
        _rulesValidation = rulesValidation;
        _sourceValidationService = sourceValidationService;
        _provisionValidationService = provisionValidationService;
        _regulatedPlaceValidationService = regulatedPlaceValidationService;
        _geometryValidationService = geometryValidationService;
        _regulationValidation = regulationValidation;
        _conditionValidation = conditionValidation;
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

        var jsonSchemaAsString = schema.Template.ToIndentedJsonString();
        var dtroSubmitJson = dtroSubmit.Data.ToIndentedJsonString();

        var requestComparedToSchema = _jsonSchemaValidationService.ValidateSchema(jsonSchemaAsString, dtroSubmitJson);
        if (requestComparedToSchema.Count > 0)
        {
            return new DtroValidationException { RequestComparedToSchema = requestComparedToSchema.ToList() };
        }

        var sourceValidationErrors = _sourceValidationService.ValidateSource(dtroSubmit, traCode);
        if (sourceValidationErrors.Count > 0)
        {
            return new DtroValidationException { RequestComparedToRules = sourceValidationErrors.MapFrom() };
        }

        var provisionValidationErrors = _provisionValidationService.ValidateProvision(dtroSubmit);
        if (provisionValidationErrors.Count > 0)
        {
            return new DtroValidationException { RequestComparedToRules = provisionValidationErrors.MapFrom() };

        }

        var regulatedPlacesValidationErrors = _regulatedPlaceValidationService.ValidateRegulatedPlaces(dtroSubmit);
        if (regulatedPlacesValidationErrors.Count > 0)
        {
            return new DtroValidationException { RequestComparedToRules = regulatedPlacesValidationErrors.MapFrom() };
        }

        var geometryValidationErrors = _geometryValidationService.ValidateGeometry(dtroSubmit);
        if (geometryValidationErrors.Count > 0)
        {
            return new DtroValidationException { RequestComparedToRules = geometryValidationErrors.MapFrom() };
        }

        var requestComparedToRules = await _rulesValidation.ValidateRules(dtroSubmit, schemaVersion.ToString());
        if (requestComparedToRules.Count > 0)
        {
            return new DtroValidationException { RequestComparedToRules = requestComparedToRules.MapFrom() };
        }

        var requestComparedToRegulations = _regulationValidation.ValidateRegulation(dtroSubmit, schemaVersion);
        if (requestComparedToRegulations.Count > 0)
        {
            return new DtroValidationException { RequestComparedToRules = requestComparedToRegulations.MapFrom() };
        }

        var requestComparedToConditions = _conditionValidation.ValidateCondition(dtroSubmit, schemaVersion);
        if (requestComparedToConditions.Count > 0)
        {
            return new DtroValidationException { RequestComparedToRules = requestComparedToConditions.MapFrom() };
        }

        var tuple = await _semanticValidationService.ValidateCreationRequest(dtroSubmit);
        if (tuple.Item2.Count > 0)
        {
            return new DtroValidationException { RequestComparedToRules = tuple.Item2.MapFrom() };
        }

        return null;
    }
}
