using Newtonsoft.Json;

namespace Dft.DTRO.Tests.ServicesTests.Validations;

public class CasingValidationServiceTests
{
    private readonly CasingValidationService _casingValidationService;

    public CasingValidationServiceTests()
    {
        _casingValidationService = new CasingValidationService();
    }

    [Fact]
    public void ValidateRootNonCamelCaseKeyThrowsError()
    {
        string jsonString = @"
            {
                ""Source"": {
                ""actionType"": ""new"",
                ""currentTraOwner"": 9999
            }
        }
        ";

        dynamic json = JsonConvert.DeserializeObject<ExpandoObject>(jsonString);
        List<string> invalidProperties = _casingValidationService.ValidateCamelCase(json);
        List<string> expected = ["Source"];

        Assert.Equal(invalidProperties, expected);
    }

    [Fact]
    public void ValidateNestedNonCamelCaseKeyThrowsError()
    {
        string jsonString = @"
            {
                ""source"": {
                ""actionType"": ""new"",
                ""currentTraOwner"": 9999,
                ""nested"": {
                    ""key"": 1,
                    ""InvalidKey"": 2
                }
            }
        }
        ";

        dynamic json = JsonConvert.DeserializeObject<ExpandoObject>(jsonString);
        List<string> invalidProperties = _casingValidationService.ValidateCamelCase(json);
        List<string> expected = ["InvalidKey"];

        Assert.Equal(invalidProperties, expected);
    }

    [Fact]
    public void ValidateMultipleNonCamelCaseKeyThrowsError()
    {
        string jsonString = @"
            {
                ""Source"": {
                ""actionType"": ""new"",
                ""CurrentTraOwner"": 9999,
                ""nested"": {
                    ""key"": 1,
                    ""InvalidKey"": 2
                }
            }
        }
        ";

        dynamic json = JsonConvert.DeserializeObject<ExpandoObject>(jsonString);
        List<string> invalidProperties = _casingValidationService.ValidateCamelCase(json);
        List<string> expected = ["Source", "CurrentTraOwner", "InvalidKey"];

        Assert.Equal(invalidProperties, expected);
    }

    [Fact]
    public void ValidateComplexNonCamelCaseKeyThrowsError()
    {
        string jsonString = @"
            {
                ""Source"": {
                ""actionType"": ""new"",
                ""CurrentTraOwner"": 9999,
                ""nested"": {
                    ""key"": 1,
                    ""InvalidKey"": 2,
                    ""Collection"": [
                        1, 2, 3, 4, 5
                    ],
                    ""collection"": [
                        {
                            ""test"": 1,
                            ""Another"": {
                            ""Again"": 1,
                            ""testKey"": ""test""
                            }
                        }
                    ]
                }
            }
        }
        ";

        dynamic json = JsonConvert.DeserializeObject<ExpandoObject>(jsonString);
        List<string> invalidProperties = _casingValidationService.ValidateCamelCase(json);
        List<string> expected = ["Source", "CurrentTraOwner", "InvalidKey", "Collection", "Another", "Again"];

        Assert.Equal(invalidProperties, expected);
    }

    [Fact]
    public void ValidateNoErrorsWhenAllKeysAreCamelCase()
    {
        string jsonString = @"
            {
                ""source"": {
                ""actionType"": ""new"",
                ""currentTraOwner"": 9999,
                ""nested"": {
                    ""key"": 1,
                    ""invalidKey"": 2,
                    ""collection"": [
                        1, 2, 3, 4, 5
                    ],
                    ""collection"": [
                        {
                            ""test"": 1,
                            ""another"": {
                            ""again"": 1,
                            ""testKey"": ""test""
                            }
                        }
                    ]
                }
            }
        }
        ";

        dynamic json = JsonConvert.DeserializeObject<ExpandoObject>(jsonString);
        List<string> invalidProperties = _casingValidationService.ValidateCamelCase(json);

        Assert.Empty(invalidProperties);
    }

    [Fact]
    public void Validate_CommentKeyIsIgnored()
    {
        string jsonString = @"
            {
                ""source"": {
                ""actionType"": ""new"",
                ""currentTraOwner"": 9999,
                ""_comment"": ""Some comment here""
                }
            }
        ";

        string expectedJsonString = @"
            {
                ""Source"": {
                ""actionType"": ""new"",
                ""currentTraOwner"": 9999,
                ""_comment"": ""Some comment here""
                }
            }
        ";

        dynamic json = JsonConvert.DeserializeObject<ExpandoObject>(jsonString);
        List<string> invalidProperties = _casingValidationService.ValidateCamelCase(json);

        Assert.Empty(invalidProperties);
    }

    [Fact]
    public void SimpleObjectCanBeConvertedToPascalCase()
    {
        string jsonString = @"
            {
                ""source"": {
                ""actionType"": ""new"",
                ""currentTraOwner"": 9999
                }
            }
        ";


        string expectedJsonString = @"
            {
                ""Source"": {
                ""actionType"": ""new"",
                ""currentTraOwner"": 9999
                }
            }
        ";

        dynamic json = JsonConvert.DeserializeObject<ExpandoObject>(jsonString);
        dynamic actual = _casingValidationService.ConvertKeysToPascalCase(json);
        dynamic expected = JsonConvert.DeserializeObject<ExpandoObject>(expectedJsonString);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void NestedObjectCanBeConvertedToPascalCase()
    {
        string jsonString = @"
            {
                ""source"": {
                ""actionType"": ""new"",
                ""currentTraOwner"": 9999,
                ""nested"": {
                    ""item"": 1,
                    ""anotherItem"": ""hello""
                },
                ""provision"": {
                  ""vehicleCharacteristics"": {
                    ""someKey"": 1,
                    ""collection"": [
                        {
                            ""timeValidity"": 2
                        }
                    ]
                  }
                }
                }
            }
        ";


        string expectedJsonString = @"
            {
                ""Source"": {
                ""actionType"": ""new"",
                ""currentTraOwner"": 9999,
                ""nested"": {
                    ""item"": 1,
                    ""anotherItem"": ""hello""
                },
                ""Provision"": {
                    ""VehicleCharacteristics"": {
                      ""someKey"": 1,
                      ""collection"": [
                        {
                            ""timeValidity"": 2
                        }
                      ]
                    }
                }
                }
            }
        ";

        dynamic json = JsonConvert.DeserializeObject<ExpandoObject>(jsonString);
        dynamic actual = _casingValidationService.ConvertKeysToPascalCase(json);
        dynamic expected = JsonConvert.DeserializeObject<ExpandoObject>(expectedJsonString);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Validate_CommentKeyIsIgnoredWhenConverting()
    {
        string jsonString = @"
            {
                ""source"": {
                ""actionType"": ""new"",
                ""currentTraOwner"": 9999,
                ""_comment"": ""Some comment here"",
                ""nested"": {
                    ""item"": 1,
                    ""anotherItem"": ""hello"",
                    ""_comment"": ""Some comment here"",
                },
                ""provision"": {
                    ""_comment"": ""Some comment here"",
                  ""vehicleCharacteristics"": {
                    ""someKey"": 1,
                    ""collection"": [
                        {
                            ""_comment"": ""Some comment here"",
                            ""timeValidity"": 2
                        }
                    ]
                  }
                }
                }
            }
        ";


        string expectedJsonString = @"
            {
                ""Source"": {
                ""actionType"": ""new"",
                ""currentTraOwner"": 9999,
                ""_comment"": ""Some comment here"",
                ""nested"": {
                    ""item"": 1,
                    ""anotherItem"": ""hello"",
                    ""_comment"": ""Some comment here"",
                },
                ""Provision"": {
                    ""_comment"": ""Some comment here"",
                  ""VehicleCharacteristics"": {
                    ""someKey"": 1,
                    ""collection"": [
                        {
                            ""_comment"": ""Some comment here"",
                            ""timeValidity"": 2
                        }
                    ]
                  }
                }
                }
            }
        ";

        dynamic json = JsonConvert.DeserializeObject<ExpandoObject>(jsonString);
        dynamic actual = _casingValidationService.ConvertKeysToPascalCase(json);
        dynamic expected = JsonConvert.DeserializeObject<ExpandoObject>(expectedJsonString);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ComplexObjectCanBeConvertedToPascalCase()
    {
        string jsonString = @"
            {
                ""source"": {
                ""actionType"": ""new"",
                ""currentTraOwner"": 9999,
                ""nested"": {
                    ""item"": 1,
                    ""anotherItem"": ""hello"",
                    ""collection"": [
                        1, 2, 3, 4, 5
                    ],
                    ""anotherCollection"": [
                        {
                            ""test"": {
                                ""name"": ""testing""
                            }
                        }
                    ],
                    ""key"": ""value"",
                    ""object"": {
                        ""keyName"": ""value"",
                        ""timeValidity"": {
                            ""vehicleCharacteristics"": ""value""
                        },
                        ""geometry"": {
                            ""pointGeometry"": {
                                ""value"": 1
                            }
                        }
                    }
                }
                }
            }
        ";


        string expectedJsonString = @"
            {
                ""Source"": {
                ""actionType"": ""new"",
                ""currentTraOwner"": 9999,
                ""nested"": {
                    ""item"": 1,
                    ""anotherItem"": ""hello"",
                    ""collection"": [
                        1, 2, 3, 4, 5
                    ],
                    ""anotherCollection"": [
                        {
                            ""test"": {
                                ""name"": ""testing""
                            }
                        }
                    ],
                    ""key"": ""value"",
                    ""object"": {
                        ""keyName"": ""value"",
                        ""TimeValidity"": {
                            ""vehicleCharacteristics"": ""value"",
                        },
                        ""Geometry"": {
                            ""PointGeometry"": {
                                ""value"": 1
                            }
                        }
                    }
                }
                }
            }
        ";

        dynamic json = JsonConvert.DeserializeObject<ExpandoObject>(jsonString);
        dynamic actual = _casingValidationService.ConvertKeysToPascalCase(json);
        dynamic expected = JsonConvert.DeserializeObject<ExpandoObject>(expectedJsonString);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ValidatePolygonEdgeCaseIsHandledCorrectly()
    {
        string jsonString = @"
            {
                ""source"": {
                    ""provision"": {
                        ""regulatedPlace"": [
                        {
                            ""geometry"": [
                                {
                                ""polygon"": {
                                    ""polygon"": ""some polygon""
                                }
                                }   
                            ]
                        }
                        ]
                    }
                }
            }
        ";

        string expectedJsonString = @"
            {
                ""Source"": {
                    ""Provision"": {
                        ""RegulatedPlace"": [
                            {
                                ""Geometry"": [
                                    {
                                    ""Polygon"": {
                                        ""polygon"": ""some polygon""
                                    }
                                }   
                            ]
                            }
                        ]
                    }
                }
            }
        ";

        dynamic json = JsonConvert.DeserializeObject<ExpandoObject>(jsonString);
        dynamic actual = _casingValidationService.ConvertKeysToPascalCase(json);
        dynamic expected = JsonConvert.DeserializeObject<ExpandoObject>(expectedJsonString);

        string a = JsonConvert.SerializeObject(actual);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ValidateSchemaVersionCamelCaseCheckReturnsFalseForSchemaVersionPriorTo332()
    {
        SchemaVersion schemaVersion = "3.3.1";
        Assert.False(_casingValidationService.SchemaVersionEnforcesCamelCase(schemaVersion));

        schemaVersion = "3.3.0";
        Assert.False(_casingValidationService.SchemaVersionEnforcesCamelCase(schemaVersion));
    }

    [Fact]
    public void ValidateSchemaVersionCamelCaseCheckReturnsTrueForSchemaVersion332Onwards()
    {
        SchemaVersion schemaVersion = "3.3.2";
        Assert.True(_casingValidationService.SchemaVersionEnforcesCamelCase(schemaVersion));

        schemaVersion = "3.3.3";
        Assert.True(_casingValidationService.SchemaVersionEnforcesCamelCase(schemaVersion));

        schemaVersion = "3.3.10";
        Assert.True(_casingValidationService.SchemaVersionEnforcesCamelCase(schemaVersion));

        schemaVersion = "4.0.0";
        Assert.True(_casingValidationService.SchemaVersionEnforcesCamelCase(schemaVersion));
    }
}