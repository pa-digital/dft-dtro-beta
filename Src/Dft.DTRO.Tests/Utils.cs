using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Text;
using DfT.DTRO.Models.DataBase;
using DfT.DTRO.Models.DtroDtos;
using DfT.DTRO.Models.DtroHistory;
using DfT.DTRO.Models.SchemaTemplate;
using DfT.DTRO.Services;
using DfT.DTRO.Services.Conversion;
using DfT.DTRO.Services.Mapping;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Dft.DTRO.Tests
{
    [ExcludeFromCodeCoverage]
    public static class Utils
    {
        public static DtroSubmit PrepareDtro(string jsonData, SchemaVersion? schemaVersion = null)
            => new()
            {
                SchemaVersion = schemaVersion ?? "3.1.2",
                Data = JsonConvert.DeserializeObject<ExpandoObject>(jsonData, new ExpandoObjectConverter())
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
            var builder = new ConfigurationBuilder();
            var mappingService = new SchemaTemplateMappingService();
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
            var mappingService = new DtroMappingService(configuration, new Proj4SpatialProjectionService());
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
                var mappingService = new DtroMappingService(configuration, new Proj4SpatialProjectionService());
                mappingService.InferIndexFields(ref dtro);
            }

            string payload = JsonConvert.SerializeObject(dtro);

            return new StringContent(payload, Encoding.UTF8, "application/json");
        }

        public static async Task<List<DtroHistoryResponse>> CreateResponseDtroHistoryObject(string[] dtroJsonPath)
        {
            List<string> items = dtroJsonPath.Select(File.ReadAllText).ToList();

            DateTime createdAt = DateTime.Now;

            List<DtroHistoryResponse> sampleDtroHistories = items
                .Select(item => JsonConvert.DeserializeObject<ExpandoObject>(item, new ExpandoObjectConverter()))
                .Select(dtroData => new DtroHistoryResponse
                {
                    Id = Guid.NewGuid(), 
                    DtroId = Guid.Parse("C3B3BB0C-E3A6-47EF-83ED-4C48E56F9DD4"),
                    Created = new DateTime(2024,6,19,16,38,00), 
                    LastUpdated = createdAt, 
                    Data = dtroData
                }).ToList();

            return sampleDtroHistories;
        }

        public static async Task<List<DtroHistoryRequest>> CreateRequestDtroHistoryObject(string[] dtroJsonPath)
        {
            List<string> items = dtroJsonPath.Select(File.ReadAllText).ToList();

            List<ExpandoObject?> data = items.Select(item =>
                JsonConvert.DeserializeObject<ExpandoObject>(item, new ExpandoObjectConverter()))
                .ToList();


            var requests = new List<DtroHistoryRequest>();
            foreach (ExpandoObject? expando in data)
            {
                var request = new DtroHistoryRequest
                {
                    Id = Guid.NewGuid(),
                    DtroId = Guid.Parse("C3B3BB0C-E3A6-47EF-83ED-4C48E56F9DD4"),
                    Created = new DateTime(2024, 6, 19, 16, 44, 00),
                    LastUpdated = DateTime.Now,
                    Data = expando,
                    TrafficAuthorityOwnerId = 1585,
                    TrafficAuthorityCreatorId = 1585
                };

                requests.Add(request);
            }

            return requests;
        }
    }
}