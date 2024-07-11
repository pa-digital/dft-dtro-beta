﻿using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using DfT.DTRO.DAL;
using DfT.DTRO.Models.DtroDtos;
using DfT.DTRO.Models.Validation;
using DfT.DTRO.Services.Validation;
using Newtonsoft.Json;

namespace Dft.DTRO.Tests;

[ExcludeFromCodeCoverage]
public class RecordManagementServiceTests
{
    private const string SourceJsonBasePath = "./DtroJsonDataExamples/v3.2.0";

    [Theory]
    [InlineData("3.2.0", "valid-new-x", true)]
    [InlineData("3.2.0", "invalid-source-reference", false)]
    [InlineData("3.2.0", "invalid-source-actionType", false)]
    [InlineData("3.2.0", "invalid-provision-reference", false)]
    [InlineData("3.2.0", "invalid-provision-actionType", false)]
    [InlineData("3.2.0", "invalid-duplicate-provision-reference", false)]
    public void ProducesCorrectResults(string schemaVersion, string file, bool isValid)
    {
        var mockSwaCodeDal = new Mock<ISwaCodeDal>();
        IRecordManagementService sut = new RecordManagementService(mockSwaCodeDal.Object);


        mockSwaCodeDal.Setup(it => it.GetAllCodes().Result).Returns(() => SwaCodesResponse);

        var input = File.ReadAllText(Path.Join(SourceJsonBasePath, $"{file}.json"));

        DtroSubmit dtroSubmit = new()
        {
            SchemaVersion = schemaVersion,
            Data = JsonConvert.DeserializeObject<ExpandoObject>(input)
        };

        List<SemanticValidationError> actual = sut.ValidateCreationRequest(dtroSubmit, 1585);

        Assert.Equal(isValid, !actual.Any());
    }
}