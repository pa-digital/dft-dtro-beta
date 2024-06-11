using DfT.DTRO.Extensions;
using DfT.DTRO.Models.DtroDtos;
using DfT.DTRO.Models.Errors;
using DfT.DTRO.Services.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace DfT.DTRO.Services;
public class DtroGroupValidatorService : IDtroGroupValidatorService
{
    private readonly IJsonSchemaValidationService _jsonSchemaValidationService;
    private readonly ISchemaTemplateService _schemaTemplateService;
    private readonly ISemanticValidationService _semanticValidationService;
    private readonly IJsonLogicValidationService _jsonLogicValidationService;
    private readonly IRecordManagementService _recordManagementService;

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="jsonSchemaValidationService">An <see cref="IJsonSchemaValidationService"/> instance.</param>
    /// <param name="semanticValidationService">An <see cref="ISemanticValidationService"/> instance.</param>
    /// <param name="schemaTemplateService">An <see cref="ISchemaTemplateService"/> instance.</param>
    /// <param name="jsonLogicValidationService">An <see cref="IJsonLogicValidationService"/> instance.</param>
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

    public async Task<DtroValidationException> ValidateDtro(DtroSubmit dtroSubmit)
    {
        var schemaVersion = dtroSubmit.SchemaVersion;
        var schema = await _schemaTemplateService.GetSchemaTemplateAsync(schemaVersion);
        if (schema == null)
        {
            var error = new ApiErrorResponse("Not found", "Schema version not found");
            return new DtroValidationException { RequestComparedToSchemaVersion = error };
        }

        if (!schema.IsActive)
        {
            var error = new ApiErrorResponse("Not found", "Schema version is not active");
            return new DtroValidationException { RequestComparedToSchemaVersion = error };
        }

        var jsonSchemaAsString = schema.Template.ToIndentedJsonString();
        var dtroSubmitJson = dtroSubmit.Data.ToIndentedJsonString();

        var requestComparedToSchema = _jsonSchemaValidationService.ValidateRequestAgainstJsonSchema(jsonSchemaAsString, dtroSubmitJson);
        if (requestComparedToSchema.Count > 0)
        {
            return new DtroValidationException { RequestComparedToSchema = requestComparedToSchema.ToList() };
        }

        var requests = _recordManagementService.ValidateCreationRequest(dtroSubmit);
        if (requests.Count > 0)
        {
            return new DtroValidationException { RequestComparedToRules = requests.ToList() };
        }

        var requestComparedToRules = await _jsonLogicValidationService.ValidateCreationRequest(dtroSubmit);
        if (requestComparedToRules.Count > 0)
        {
            return new DtroValidationException { RequestComparedToRules = requestComparedToRules.ToList() };
        }

        var requestComparedToConstraints = await _semanticValidationService.ValidateCreationRequest(dtroSubmit);

        if (requestComparedToRules.Count > 0)
        {
            return new DtroValidationException { RequestComparedToRules = requestComparedToConstraints.ToList() };
        }

        return null;
    }
}
