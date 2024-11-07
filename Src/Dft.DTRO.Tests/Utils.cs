using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Dft.DTRO.Tests;

[ExcludeFromCodeCoverage]
public static class Utils
{
    public static DtroSubmit PrepareDtro(string jsonData) =>
        new()
        {
            Data = JsonConvert.DeserializeObject<ExpandoObject>(jsonData, new ExpandoObjectConverter()),
            SchemaVersion = new SchemaVersion(3, 2, 3)
        };

    public static async Task<StringContent> CreateSchemaPayload(string schemaPath, string schemaVersion)
    {
        string sampleSchema = await File.ReadAllTextAsync(schemaPath);

        ExpandoObject? schemaData =
            JsonConvert.DeserializeObject<ExpandoObject>(sampleSchema, new ExpandoObjectConverter());

        string payload = JsonConvert.SerializeObject(new { SchemaVersion = schemaVersion, Data = schemaData });

        return new StringContent(payload, Encoding.UTF8, "application/json");
    }

    public static async Task<SchemaTemplate> CreateSchemaTemplate(string schemaPath, string schemaVersion)
    {
        string sampleSchema = await File.ReadAllTextAsync(schemaPath);

        ExpandoObject? schemaData =
            JsonConvert.DeserializeObject<ExpandoObject>(sampleSchema, new ExpandoObjectConverter());

        SchemaTemplate schemaTemplate = new()
        {
            Id = Guid.NewGuid(),
            SchemaVersion = schemaVersion,
            Created = DateTime.Now,
            LastUpdated = DateTime.Now,
            CreatedCorrelationId = Guid.NewGuid().ToString(),
            LastUpdatedCorrelationId = Guid.NewGuid().ToString(),
            IsActive = true,
            Template = schemaData
        };

        SchemaTemplateMappingService mappingService = new();
        mappingService.MapToSchemaTemplateResponse(schemaTemplate);

        return schemaTemplate;
    }

    public static async Task<DfT.DTRO.Models.DataBase.DTRO> CreateDtroObject(string dtroJsonPath)
    {
        string sampleDtroDataJson = await File.ReadAllTextAsync(dtroJsonPath);

        DateTime createdAt = DateTime.Now;

        ExpandoObject? dtroData =
            JsonConvert.DeserializeObject<ExpandoObject>(sampleDtroDataJson, new ExpandoObjectConverter());
        DfT.DTRO.Models.DataBase.DTRO sampleDtro = new()
        {
            Id = Guid.NewGuid(),
            Created = createdAt,
            LastUpdated = createdAt,
            Data = dtroData
        };

        var builder = new ConfigurationBuilder();
        var configuration = builder.Build();
        var loggingExtension = new LoggingExtension.Builder().Build();
        var mappingService = new DtroMappingService(configuration, new BoundingBoxService(), loggingExtension);
        mappingService.InferIndexFields(ref sampleDtro);

        return sampleDtro;
    }

    public static async Task<StringContent> CreateDtroJsonPayload(string dtroJsonPath, string schemaVersion,
        bool inferIndexFields = true)
    {
        string sampleDtroDataJson = await File.ReadAllTextAsync(dtroJsonPath);

        ExpandoObject? dtroData =
            JsonConvert.DeserializeObject<ExpandoObject>(sampleDtroDataJson, new ExpandoObjectConverter());
        DfT.DTRO.Models.DataBase.DTRO dtro = new() { SchemaVersion = schemaVersion, Data = dtroData };
        if (inferIndexFields)
        {
            var builder = new ConfigurationBuilder();
            var configuration = builder.Build();
            var loggingExtension = new LoggingExtension.Builder().Build();
            var mappingService = new DtroMappingService(configuration, new BoundingBoxService(), loggingExtension);
            mappingService.InferIndexFields(ref dtro);
        }

        string payload = JsonConvert.SerializeObject(dtro);

        return new StringContent(payload, Encoding.UTF8, "application/json");
    }

    public static List<DtroHistorySourceResponse> CreateResponseDtroHistoryObject(string[] dtroJsonPath)
    {
        List<string> items = dtroJsonPath.Select(File.ReadAllText).ToList();

        DateTime createdAt = DateTime.Now;

        List<DtroHistorySourceResponse> sampleDtroHistories = items
            .Select(item => JsonConvert.DeserializeObject<ExpandoObject>(item, new ExpandoObjectConverter()))
            .Select(_ => new DtroHistorySourceResponse
            {
                Created = new DateTime(2024, 6, 19, 16, 38, 00),
                LastUpdated = createdAt,
            }).ToList();

        return sampleDtroHistories;
    }

    public static List<DtroHistoryProvisionResponse> CreateResponseDtroProvisionHistory(string[] dtroJsonPath)
    {
        List<string> items = dtroJsonPath.Select(File.ReadAllText).ToList();

        List<IList<object>> objects = items
            .Select(JsonConvert.DeserializeObject<ExpandoObject>)
            .Select(item => item.GetValueOrDefault<IList<object>>("Source.provision"))
            .ToList();

        List<DtroHistoryProvisionResponse> provisions = new();

        foreach (IList<object> obj in objects)
        {
            DtroHistoryProvisionResponse provision = new();

            string? data = obj[0].ToIndentedJsonString();

            provisions.Add(provision);
        }

        return provisions;
    }

    public static List<DTROHistory> CreateRequestDtroHistoryObject(string[] dtroJsonPath)
    {
        List<string> items = dtroJsonPath.Select(File.ReadAllText).ToList();

        List<ExpandoObject?> data = items.Select(item =>
                JsonConvert.DeserializeObject<ExpandoObject>(item, new ExpandoObjectConverter()))
            .ToList();


        var requests = new List<DTROHistory>();
        foreach (ExpandoObject? expando in data)
        {
            DTROHistory request = new()
            {
                Id = Guid.NewGuid(),
                DtroId = Guid.Parse("C3B3BB0C-E3A6-47EF-83ED-4C48E56F9DD4"),
                Created = new DateTime(2024, 6, 19, 16, 44, 00),
                LastUpdated = DateTime.Now,
                Data = expando,
                TrafficAuthorityOwnerId = 1000,
                TrafficAuthorityCreatorId = 1000,
                SchemaVersion = "3.2.3"
            };

            requests.Add(request);
        }

        return requests;
    }

    public static List<DtroUserResponse> SwaCodesResponse => new()
    {
        new DtroUserResponse
        {
            TraId = 1002,
            UserGroup = UserGroup.Admin,
            xAppId = Guid.NewGuid(),
            Name = "Department for Transport",
            Prefix = "DfT"
        }, new DtroUserResponse
        {
            TraId = 1000,
            UserGroup = UserGroup.Tra,
            xAppId = Guid.NewGuid(),
            Name = "Essex Council",
            Prefix = "GP"
        },
        new DtroUserResponse
        {
            TraId = 1001,
            UserGroup = UserGroup.Tra,
            xAppId = Guid.NewGuid(),
            Name = "Cornwall Council",
            Prefix = "DP"
        }
    };
}