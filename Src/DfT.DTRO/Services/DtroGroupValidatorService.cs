using DfT.DTRO.Extensions.Exceptions;
using DfT.DTRO.Services.Validation.Contracts;

namespace DfT.DTRO.Services;
public class DtroGroupValidatorService : IDtroGroupValidatorService
{
    private readonly IJsonSchemaValidationService _jsonSchemaValidationService;
    private readonly ISchemaTemplateService _schemaTemplateService;
    private readonly ISemanticValidationService _semanticValidationService;
    private readonly IRulesValidation _rulesValidation;
    private readonly IRecordManagementService _recordManagementService;
    private readonly IRegulatedPlaceValidation _regulatedPlaceValidation;
    private readonly IRegulationValidation _regulationValidation;
    private readonly IConditionValidation _conditionValidation;

    public DtroGroupValidatorService(
        IJsonSchemaValidationService jsonSchemaValidationService,
        ISemanticValidationService semanticValidationService,
        ISchemaTemplateService schemaTemplateService,
        IRulesValidation rulesValidation,
        IRecordManagementService recordManagementService,
        IRegulatedPlaceValidation regulatedPlaceValidation,
        IRegulationValidation regulationValidation,
        IConditionValidation conditionValidation)
    {
        _jsonSchemaValidationService = jsonSchemaValidationService;
        _semanticValidationService = semanticValidationService;
        _schemaTemplateService = schemaTemplateService;
        _rulesValidation = rulesValidation;
        _recordManagementService = recordManagementService;
        _regulatedPlaceValidation = regulatedPlaceValidation;
        _regulationValidation = regulationValidation;
        _conditionValidation = conditionValidation;
    }

    public async Task<DtroValidationException> ValidateDtro(DtroSubmit dtroSubmit, int? headerTa)
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

        var requests = _recordManagementService.ValidateRecordManagement(dtroSubmit, headerTa);
        if (requests.Count > 0)
        {
            return new DtroValidationException { RequestComparedToRules = requests.MapFrom() };
        }

        var requestComparedToRules = await _rulesValidation.ValidateRules(dtroSubmit, schemaVersion.ToString());
        if (requestComparedToRules.Count > 0)
        {
            return new DtroValidationException { RequestComparedToRules = requestComparedToRules.MapFrom() };
        }

        var requestComparedRegulatedPlaces = _regulatedPlaceValidation.ValidateRegulatedPlacesType(dtroSubmit, schemaVersion);
        if (requestComparedRegulatedPlaces.Count > 0)
        {
            return new DtroValidationException { RequestComparedToRules = requestComparedRegulatedPlaces.MapFrom() };
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
