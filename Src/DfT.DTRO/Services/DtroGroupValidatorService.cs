using DfT.DTRO.Extensions.Exceptions;

namespace DfT.DTRO.Services;
public class DtroGroupValidatorService : IDtroGroupValidatorService
{
    private readonly IJsonSchemaValidationService _jsonSchemaValidationService;
    private readonly ISchemaTemplateService _schemaTemplateService;
    private readonly ISemanticValidationService _semanticValidationService;
    private readonly IJsonLogicValidationService _jsonLogicValidationService;
    private readonly IRecordManagementService _recordManagementService;

    public DtroGroupValidatorService(
        IJsonSchemaValidationService jsonSchemaValidationService,
        ISemanticValidationService semanticValidationService,
        ISchemaTemplateService schemaTemplateService,
        IJsonLogicValidationService jsonLogicValidationService,
        IRecordManagementService recordManagementService)
    {
        _jsonSchemaValidationService = jsonSchemaValidationService;
        _semanticValidationService = semanticValidationService;
        _schemaTemplateService = schemaTemplateService;
        _jsonLogicValidationService = jsonLogicValidationService;
        _recordManagementService = recordManagementService;
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

        var requestComparedToSchema = _jsonSchemaValidationService.ValidateRequestAgainstJsonSchema(jsonSchemaAsString, dtroSubmitJson);
        if (requestComparedToSchema.Count > 0)
        {
            return new DtroValidationException { RequestComparedToSchema = requestComparedToSchema.ToList() };
        }

        var requests = _recordManagementService.ValidateCreationRequest(dtroSubmit, headerTa);
        if (requests.Count > 0)
        {
            return new DtroValidationException { RequestComparedToRules = requests.MapFrom() };
        }

        var requestComparedToRules = await _jsonLogicValidationService.ValidateCreationRequest(dtroSubmit, schemaVersion.ToString());
        if (requestComparedToRules.Count > 0)
        {
            return new DtroValidationException { RequestComparedToRules = requestComparedToRules.MapFrom() };
        }

        var tuple = await _semanticValidationService.ValidateCreationRequest(dtroSubmit);

        if (tuple.Item2.Count > 0)
        {
            return new DtroValidationException { RequestComparedToRules = tuple.Item2.MapFrom() };
        }

        return null;
    }
}
