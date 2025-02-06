namespace DfT.DTRO.DatabaseInitializer;

public static class DatabaseFeeder
{
    public static async Task Seed<T>(DtroContext context, List<T> records) where T : class
    {
        var entities = await context.Set<T>().ToListAsync();
        if (entities.Count != 0)
        {
            return;
        }

        await context.Set<T>().AddRangeAsync(records);
        await context.SaveChangesAsync();
    }

    public static List<User> Users =>
    [
        new()
        {
            Id = Guid.Parse("4021579b-c9c4-4382-83ea-f0c2ef8fdd49"),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Forename = "John",
            Surname = "Doe",
            Email = "jon.doe@email.com",
            IsCentralServiceOperator = true,
            Status = "active",
            UserStatusId = Guid.Parse("71f5b6eb-3e28-422a-b154-51cbd5767fb9"),
            DigitalServiceProviders = [],
        },
        new()
        {
            Id = Guid.Parse("5007c29d-18c0-494b-ad00-611a326fd063"),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Forename = "John",
            Surname = "Doe",
            Email = "jon.doe@email.com",
            IsCentralServiceOperator = true,
            Status = "active",
            UserStatusId = Guid.Parse("9bd349e1-ffed-4e85-b87d-66fcf6d3707d"),
            DigitalServiceProviders = [],
        },
        new()
        {
            Id = Guid.Parse("14414134-1aca-49dd-aeaf-c05fc5ccc8e6"),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Forename = "Jane",
            Surname = "Doe",
            Email = "jane.doe@email.com",
            IsCentralServiceOperator = false,
            Status = "active",
            UserStatusId = Guid.Parse("9bd349e1-ffed-4e85-b87d-66fcf6d3707d"),
            DigitalServiceProviders = []
        },
        new()
        {
            Id = Guid.Parse("eddc3e66-242f-4003-90b7-e57807d0a968"),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Forename = "Jake",
            Surname = "Blogs",
            Email = "george.blogs@email.com",
            IsCentralServiceOperator = false,
            Status = "active",
            UserStatusId = Guid.Parse("803e7960-e772-4a7d-9690-b4a45f27b607"),
            DigitalServiceProviders = []
        },
        new()
        {
            Id = Guid.Parse("4519447b-1a1c-4e0f-9a51-9270896c8f93"),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Forename = "Jenny",
            Surname = "Blogs",
            Email = "jenny.blogs@email.com",
            IsCentralServiceOperator = false,
            Status = "active",
            UserStatusId = Guid.Parse("803e7960-e772-4a7d-9690-b4a45f27b607"),
            DigitalServiceProviders = []
        }
    ];

    public static List<UserStatus> UserStatuses =>
    [
        new()
        {
            Id = Guid.Parse("71f5b6eb-3e28-422a-b154-51cbd5767fb9"),
            Status = "administrator"
        },
        new()
        {
            Id = Guid.Parse("9bd349e1-ffed-4e85-b87d-66fcf6d3707d"),
            Status = "company"
        },
        new()
        {
            Id = Guid.Parse("803e7960-e772-4a7d-9690-b4a45f27b607"),
            Status = "hobbyist"
        }
    ];

    public static List<TrafficRegulationAuthority> TrafficRegulationAuthorities =>
    [
        new()
        {
            Id = Guid.Parse("ae153c4b-64bc-4c60-a315-f1fa3bffffb3"),
            Name = "First County Council",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = "approved"
        },
        new()
        {
            Id = Guid.Parse("0682f832-3cde-4ba3-9cc1-226d47ea8a75"),
            Name = "Second County Council",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = "rejected"
        },
        new()
        {
            Id = Guid.Parse("ccca0818-b4b6-405b-a350-5811ff0a383c"),
            Name = "Third County Council",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = "pending"
        },
        new()
        {
            Id = Guid.Parse("f8be7166-4d50-4bb5-a562-942dea4234a8"),
            Name = "Fourth County Council",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = "on-review"
        },
        new()
        {
            Id = Guid.Parse("72cec2a0-3344-4463-acf5-0b8e7a246a8f"),
            Name = "Fifth County Council",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = "approved"
        }
    ];

    public static List<DigitalServiceProvider> DigitalServiceProviders =>
    [
        new()
        {
            Id = Guid.Parse("53e9542b-9d53-4371-85ad-81d704428378"),
            Name = "First Publisher Company",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = "approved",
        },
        new()
        {
            Id = Guid.Parse("d1b93a0b-3295-4425-84db-9f66e045610b"),
            Name = "Second Publisher Company",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = "rejected",
        },
        new()
        {
            Id = Guid.Parse("b93719ad-2f83-4ac6-9fa4-33596050cd8e"),
            Name = "Third Publisher Company",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = "pending",
        },
        new()
        {
            Id = Guid.Parse("f9bed16d-5fd7-42d0-8ff3-779e4fc98604"),
            Name = "Fourth Publisher Company",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = "on-review",
        },
        new()
        {
            Id = Guid.Parse("41672159-53d5-4f8f-85e1-f551f377c730"),
            Name = "Fifth Publisher Company",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = "approved",
        }
    ];

    public static List<TrafficRegulationAuthorityDigitalServiceProvider> TrafficRegulationAuthorityDigitalServiceProviders =>
    [
        new()
        {
            Id = Guid.Parse("679f0ce5-6626-40c8-96b7-8753c08d6006"),
            DigitalServiceProviderId = Guid.Parse("53e9542b-9d53-4371-85ad-81d704428378"),
            TrafficRegulationAuthorityId = Guid.Parse("ae153c4b-64bc-4c60-a315-f1fa3bffffb3"),
            TrafficRegulationAuthorityDigitalServiceProviderStatusId = Guid.Parse("72101c68-5e78-49c4-addf-e32d8e4d0ca4")
        },
        new()
        {
            Id = Guid.Parse("767888f6-cde3-4ecc-bbd6-c37b2b2a2b62"),
            DigitalServiceProviderId = Guid.Parse("d1b93a0b-3295-4425-84db-9f66e045610b"),
            TrafficRegulationAuthorityId = Guid.Parse("0682f832-3cde-4ba3-9cc1-226d47ea8a75"),
            TrafficRegulationAuthorityDigitalServiceProviderStatusId = Guid.Parse("72101c68-5e78-49c4-addf-e32d8e4d0ca4")
        },
        new()
        {
            Id = Guid.Parse("5a0faef2-6636-4adc-aa9c-8a4cf98dfa24"),
            DigitalServiceProviderId = Guid.Parse("b93719ad-2f83-4ac6-9fa4-33596050cd8e"),
            TrafficRegulationAuthorityId = Guid.Parse("ccca0818-b4b6-405b-a350-5811ff0a383c"),
            TrafficRegulationAuthorityDigitalServiceProviderStatusId = Guid.Parse("72101c68-5e78-49c4-addf-e32d8e4d0ca4")
        },
        new()
        {
            Id = Guid.Parse("fd82fb48-3d56-4cb2-9b6c-36125a056b1c"),
            DigitalServiceProviderId = Guid.Parse("f9bed16d-5fd7-42d0-8ff3-779e4fc98604"),
            TrafficRegulationAuthorityId = Guid.Parse("f8be7166-4d50-4bb5-a562-942dea4234a8"),
            TrafficRegulationAuthorityDigitalServiceProviderStatusId = Guid.Parse("857efb7b-121a-4b86-9bae-d303c3a657b1")
        },
        new()
        {
            Id = Guid.Parse("962d7dc0-6c22-46f8-9bf5-bae79f859da0"),
            DigitalServiceProviderId = Guid.Parse("41672159-53d5-4f8f-85e1-f551f377c730"),
            TrafficRegulationAuthorityId = Guid.Parse("72cec2a0-3344-4463-acf5-0b8e7a246a8f"),
            TrafficRegulationAuthorityDigitalServiceProviderStatusId = Guid.Parse("2d9bfcbc-e745-4b32-b417-372a3ca8eaca")
        }
    ];

    public static List<TrafficRegulationAuthorityDigitalServiceProviderStatus> TrafficRegulationAuthorityDigitalServiceProviderStatuses =>
    [
        new()
        {
            Id = Guid.Parse("72101c68-5e78-49c4-addf-e32d8e4d0ca4"),
            Status = "accepted"
        },
        new()
        {
            Id = Guid.Parse("2d9bfcbc-e745-4b32-b417-372a3ca8eaca"),
            Status = "pending"
        },
        new()
        {
            Id = Guid.Parse("857efb7b-121a-4b86-9bae-d303c3a657b1"),
            Status = "rejected"
        }
    ];

    public static List<Application> Applications =>
    [
        new()
        {
            Id = Guid.Parse("d39e04c9-fc3a-4c95-8955-37464d71c4ed"),
            Name = "First-Publisher-Company-First-County-Council",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Nickname = "First Application",
            Status = "active",
            ApplicationPurposeId= Guid.Parse("ca8d2434-0ab4-4e7b-bee6-45507049cb9a"),
            ApplicationTypeId= Guid.Parse("ad5a8607-1e6e-45ac-9310-c3f7d073dcf5"),
            TrafficRegulationAuthorityId = Guid.Parse("ae153c4b-64bc-4c60-a315-f1fa3bffffb3"),
            DigitalServiceProviderId= Guid.Parse("53e9542b-9d53-4371-85ad-81d704428378")
        },
        new()
        {
            Id = Guid.Parse("dd7c984d-09db-48c0-a12f-73cd84413df0"),
            Name = "Second-Publisher-Company-Second-County-Council",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Nickname = "Second Application",
            Status = "active",
            ApplicationPurposeId= Guid.Parse("61e6895c-7036-4dca-a9f3-0e5f712effc4"),
            ApplicationTypeId= Guid.Parse("92a183ce-285b-4073-a6b8-50c00935520e"),
            TrafficRegulationAuthorityId = Guid.Parse("0682f832-3cde-4ba3-9cc1-226d47ea8a75"),
            DigitalServiceProviderId= Guid.Parse("d1b93a0b-3295-4425-84db-9f66e045610b")
        },
        new()
        {
            Id = Guid.Parse("3fb08442-92d3-4c30-816c-47023aed6326"),
            Name = "Third-Publisher-Company-Third-County-Council",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Nickname = "Third Application",
            Status = "active",
            ApplicationPurposeId= Guid.Parse("439df921-f2d2-4db8-8308-3124a1c65776"),
            ApplicationTypeId= Guid.Parse("7f2d185f-5ec9-4896-9f44-fa1a39980763"),
            TrafficRegulationAuthorityId = Guid.Parse("ccca0818-b4b6-405b-a350-5811ff0a383c"),
            DigitalServiceProviderId= Guid.Parse("b93719ad-2f83-4ac6-9fa4-33596050cd8e")
        },
        new()
        {
            Id = Guid.Parse("026159ac-8dd8-4cb8-a374-78394c58bb17"),
            Name = "Fourth-Publisher-Company-Fourth-County-Council",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Nickname = "Fourth Application",
            Status = "active",
            ApplicationPurposeId= Guid.Parse("148168f9-f2c2-4019-8994-78ce843289af"),
            ApplicationTypeId= Guid.Parse("6825ffb9-65c5-4df8-91fe-a6bc570a42e7"),
            TrafficRegulationAuthorityId = Guid.Parse("f8be7166-4d50-4bb5-a562-942dea4234a8"),
            DigitalServiceProviderId= Guid.Parse("f9bed16d-5fd7-42d0-8ff3-779e4fc98604")
        },
        new()
        {
            Id = Guid.Parse("3035a615-9fbe-4976-a1f3-e1ab19a0c503"),
            Name = "Fifth-Publisher-Company-Fifth-County-Council",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Nickname = "Fifth Application",
            Status = "active",
            ApplicationPurposeId= Guid.Parse("6bbb6870-5ff5-450c-b216-199a64168800"),
            ApplicationTypeId= Guid.Parse("43568fb2-e5f3-4aca-8a0e-7b6a2ebd1ff3"),
            TrafficRegulationAuthorityId = Guid.Parse("72cec2a0-3344-4463-acf5-0b8e7a246a8f"),
            DigitalServiceProviderId= Guid.Parse("41672159-53d5-4f8f-85e1-f551f377c730")
        },
        new()
        {
            Id = Guid.Parse("f3ba4506-93f2-457d-928b-3a30fc0139b6"),
            Name = "First-Central-Service-Operator",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Nickname = "First CSO Application",
            Status = "active",
            ApplicationPurposeId= Guid.Parse("9f866867-169e-443b-b74b-dad287ce6d13"),
            ApplicationTypeId= Guid.Parse("02f91edd-f743-46ed-be32-7aac37f2fc01"),
            TrafficRegulationAuthorityId = Guid.Empty,
            DigitalServiceProviderId= Guid.Empty
        }
    ];

    public static List<ApplicationType> ApplicationTypes =>
    [
        new()
        {
            Id = Guid.Parse("ad5a8607-1e6e-45ac-9310-c3f7d073dcf5"),
            Name = "Publisher",
        },
        new()
        {
            Id = Guid.Parse("92a183ce-285b-4073-a6b8-50c00935520e"),
            Name = "Publisher",
        },
        new()
        {
            Id = Guid.Parse("7f2d185f-5ec9-4896-9f44-fa1a39980763"),
            Name = "Publisher",
        },
        new()
        {
            Id = Guid.Parse("6825ffb9-65c5-4df8-91fe-a6bc570a42e7"),
            Name = "Publisher",
        },
        new()
        {
            Id = Guid.Parse("43568fb2-e5f3-4aca-8a0e-7b6a2ebd1ff3"),
            Name = "Publisher",
        },
        new()
        {
            Id = Guid.Parse("02f91edd-f743-46ed-be32-7aac37f2fc01"),
            Name = "Central Service Operator"
        }
    ];

    public static List<ApplicationPurpose> ApplicationPurposes =>
    [
        new()
        {
            Id = Guid.Parse("ca8d2434-0ab4-4e7b-bee6-45507049cb9a"),
            Description = "some description"
        },
        new()
        {
            Id = Guid.Parse("61e6895c-7036-4dca-a9f3-0e5f712effc4"),
            Description = "some other description"
        },
        new()
        {
            Id = Guid.Parse("439df921-f2d2-4db8-8308-3124a1c65776"),
            Description = "lorem ipsum"
        },
        new()
        {
            Id = Guid.Parse("148168f9-f2c2-4019-8994-78ce843289af"),
            Description = "yet another description"
        },
        new()
        {
            Id = Guid.Parse("6bbb6870-5ff5-450c-b216-199a64168800"),
            Description = "description"
        },
        new()
        {
            Id = Guid.Parse("9f866867-169e-443b-b74b-dad287ce6d13"),
            Description = "Application for Central Service Operator"
        }
    ];

    public static List<SchemaTemplate> SchemaTemplates =>
    [
        new()
        {
            Id = Guid.Parse("fdba1a31-adc7-4c52-a508-0cd4c398837d"),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.0"),
            CreatedCorrelationId ="41ae0471-d7de-4737-907f-cab2f0089796",
            LastUpdatedCorrelationId ="41ae0471-d7de-4737-907f-cab2f0089796",
            Template = new ExpandoObject(),
            IsActive = true,
            ApplicationId = Guid.Parse("f3ba4506-93f2-457d-928b-3a30fc0139b6")
        },
        new()
        {
            Id = Guid.Parse("73ec34ff-f7e2-4091-976c-c1feb3660da0"),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.1"),
            CreatedCorrelationId ="41ae0471-d7de-4737-907f-cab2f0089796",
            LastUpdatedCorrelationId ="41ae0471-d7de-4737-907f-cab2f0089796",
            Template = new ExpandoObject(),
            IsActive = true,
            ApplicationId = Guid.Parse("f3ba4506-93f2-457d-928b-3a30fc0139b6")
        },
        new()
        {
            Id = Guid.Parse("59ad051f-98a0-40f2-b775-67499b909966"),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.2"),
            CreatedCorrelationId ="41ae0471-d7de-4737-907f-cab2f0089796",
            LastUpdatedCorrelationId ="41ae0471-d7de-4737-907f-cab2f0089796",
            Template = new ExpandoObject(),
            IsActive = true,
            ApplicationId = Guid.Parse("f3ba4506-93f2-457d-928b-3a30fc0139b6")
        },
        new()
        {
            Id = Guid.Parse("2e92d6bd-5cf8-4790-89d2-db04b6f9d74a"),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.3"),
            CreatedCorrelationId ="41ae0471-d7de-4737-907f-cab2f0089796",
            LastUpdatedCorrelationId ="41ae0471-d7de-4737-907f-cab2f0089796",
            Template = new ExpandoObject(),
            IsActive = true,
            ApplicationId = Guid.Parse("f3ba4506-93f2-457d-928b-3a30fc0139b6")
        },
        new()
        {
            Id = Guid.Parse("a51d4094-16eb-4dc9-a9b7-a56e12d06f6b"),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.4"),
            CreatedCorrelationId ="41ae0471-d7de-4737-907f-cab2f0089796",
            LastUpdatedCorrelationId ="41ae0471-d7de-4737-907f-cab2f0089796",
            Template = new ExpandoObject(),
            IsActive = true,
            ApplicationId = Guid.Parse("f3ba4506-93f2-457d-928b-3a30fc0139b6")
        }
    ];

    public static List<RuleTemplate> RuleTemplates =>
    [
        new()
        {
            Id = Guid.Parse("d92e7da9-c23e-4a6b-bce5-8ab738e28d88"),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.0"),
            CreatedCorrelationId ="41ae0471-d7de-4737-907f-cab2f0089796",
            LastUpdatedCorrelationId ="41ae0471-d7de-4737-907f-cab2f0089796",
            Template = "",
            ApplicationId = Guid.Parse("f3ba4506-93f2-457d-928b-3a30fc0139b6")
        },
        new()
        {
            Id = Guid.Parse("87aa850b-0fbd-407b-965e-d5a250f0708a"),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.1"),
            CreatedCorrelationId ="41ae0471-d7de-4737-907f-cab2f0089796",
            LastUpdatedCorrelationId ="41ae0471-d7de-4737-907f-cab2f0089796",
            Template = "",
            ApplicationId = Guid.Parse("f3ba4506-93f2-457d-928b-3a30fc0139b6")
        },
        new()
        {
            Id = Guid.Parse("2f53e493-0bd8-4dbd-86fb-b1b2cb162436"),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.2"),
            CreatedCorrelationId ="41ae0471-d7de-4737-907f-cab2f0089796",
            LastUpdatedCorrelationId ="41ae0471-d7de-4737-907f-cab2f0089796",
            Template = "",
            ApplicationId = Guid.Parse("f3ba4506-93f2-457d-928b-3a30fc0139b6")
        },
        new()
        {
            Id = Guid.Parse("89e86722-897f-4e14-bb50-5c82c1e31ca3"),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.3"),
            CreatedCorrelationId ="41ae0471-d7de-4737-907f-cab2f0089796",
            LastUpdatedCorrelationId ="41ae0471-d7de-4737-907f-cab2f0089796",
            Template = "",
            ApplicationId = Guid.Parse("f3ba4506-93f2-457d-928b-3a30fc0139b6")
        },
        new()
        {
            Id = Guid.Parse("6cf608d9-3d15-4125-92b4-e7942b513fbe"),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.5"),
            CreatedCorrelationId ="41ae0471-d7de-4737-907f-cab2f0089796",
            LastUpdatedCorrelationId ="41ae0471-d7de-4737-907f-cab2f0089796",
            Template = "",
            ApplicationId = Guid.Parse("f3ba4506-93f2-457d-928b-3a30fc0139b6")
        }
    ];

    public static List<Models.DataBase.DTRO> DigitalTrafficRegulationOrders =>
    [
        new()
        {
            Id = Guid.Parse("ef76adf7-5bd2-4870-9b52-08e45be33779"),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.0"),
            RegulationStart = new DateTime(2020, 1, 1, 1, 1, 1),
            RegulationEnd = new DateTime(2020, 2, 2, 2, 2, 2),
            TrafficAuthorityCreatorId = 1000,
            TrafficAuthorityOwnerId = 1000,
            TroName = "",
            CreatedCorrelationId ="41ae0471-d7de-4737-907f-cab2f0089796",
            LastUpdatedCorrelationId ="41ae0471-d7de-4737-907f-cab2f0089796",
            Deleted = false,
            DeletionTime = null,
            Data = new ExpandoObject(),
            RegulatedPlaceTypes = [],
            RegulationTypes = [],
            OrderReportingPoints = [],
            Location = new BoundingBox(),
            ApplicationId = Guid.Parse("d39e04c9-fc3a-4c95-8955-37464d71c4ed")
        },
        new()
        {
            Id = Guid.Parse("a954b3e2-a698-4006-9c2d-2a7b43a568a4"),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.1"),
            RegulationStart = new DateTime(2020, 1, 1, 1, 1, 1),
            RegulationEnd = new DateTime(2020, 2, 2, 2, 2, 2),
            TrafficAuthorityCreatorId = 1000,
            TrafficAuthorityOwnerId = 1000,
            TroName = "",
            CreatedCorrelationId ="41ae0471-d7de-4737-907f-cab2f0089796",
            LastUpdatedCorrelationId ="41ae0471-d7de-4737-907f-cab2f0089796",
            Deleted = false,
            DeletionTime = null,
            Data = new ExpandoObject(),
            RegulatedPlaceTypes = [],
            RegulationTypes = [],
            OrderReportingPoints = [],
            Location = new BoundingBox(),
            ApplicationId = Guid.Parse("dd7c984d-09db-48c0-a12f-73cd84413df0")
        },
        new()
        {
            Id = Guid.Parse("4b9f4209-bba8-4dbb-97b1-defd55f56565"),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.2"),
            RegulationStart = new DateTime(2020, 1, 1, 1, 1, 1),
            RegulationEnd = new DateTime(2020, 2, 2, 2, 2, 2),
            TrafficAuthorityCreatorId = 1000,
            TrafficAuthorityOwnerId = 1000,
            TroName = "",
            CreatedCorrelationId ="41ae0471-d7de-4737-907f-cab2f0089796",
            LastUpdatedCorrelationId ="41ae0471-d7de-4737-907f-cab2f0089796",
            Deleted = false,
            DeletionTime = null,
            Data = new ExpandoObject(),
            RegulatedPlaceTypes = [],
            RegulationTypes = [],
            OrderReportingPoints = [],
            Location = new BoundingBox(),
            ApplicationId = Guid.Parse("3fb08442-92d3-4c30-816c-47023aed6326")
        },
        new()
        {
            Id = Guid.Parse("f571992f-7075-4b0e-b799-2e15acea4706"),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.3"),
            RegulationStart = new DateTime(2020, 1, 1, 1, 1, 1),
            RegulationEnd = new DateTime(2020, 2, 2, 2, 2, 2),
            TrafficAuthorityCreatorId = 1000,
            TrafficAuthorityOwnerId = 1000,
            TroName = "",
            CreatedCorrelationId ="41ae0471-d7de-4737-907f-cab2f0089796",
            LastUpdatedCorrelationId ="41ae0471-d7de-4737-907f-cab2f0089796",
            Deleted = false,
            DeletionTime = null,
            Data = new ExpandoObject(),
            RegulatedPlaceTypes = [],
            RegulationTypes = [],
            OrderReportingPoints = [],
            Location = new BoundingBox(),
            ApplicationId = Guid.Parse("3035a615-9fbe-4976-a1f3-e1ab19a0c503")
        },
        new()
        {
            Id = Guid.Parse("60c13911-9438-4c2f-aff4-58b934b0908a"),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.4"),
            RegulationStart = new DateTime(2020, 1, 1, 1, 1, 1),
            RegulationEnd = new DateTime(2020, 2, 2, 2, 2, 2),
            TrafficAuthorityCreatorId = 1000,
            TrafficAuthorityOwnerId = 1000,
            TroName = "",
            CreatedCorrelationId ="41ae0471-d7de-4737-907f-cab2f0089796",
            LastUpdatedCorrelationId ="41ae0471-d7de-4737-907f-cab2f0089796",
            Deleted = false,
            DeletionTime = null,
            Data = new ExpandoObject(),
            RegulatedPlaceTypes = [],
            RegulationTypes = [],
            OrderReportingPoints = [],
            Location = new BoundingBox(),
            ApplicationId = Guid.Parse("3035a615-9fbe-4976-a1f3-e1ab19a0c503")
        }
    ];

    public static List<DTROHistory> DigitalTrafficRegulationOrderHistories =>
    [
        new()
        {
            Id = Guid.Parse("c3bcb944-9879-4b71-bf4f-53c2b18f07e2"),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.0"),
            TrafficAuthorityCreatorId = 1000,
            TrafficAuthorityOwnerId = 1000,
            Deleted = false,
            DeletionTime = null,
            Data = new ExpandoObject(),
            DtroId = Guid.Parse("ef76adf7-5bd2-4870-9b52-08e45be33779")
        },
        new()
        {
            Id = Guid.Parse("74c72d71-034c-4c98-b387-0c48886f48e9"),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.1"),
            TrafficAuthorityCreatorId = 1001,
            TrafficAuthorityOwnerId = 1001,
            Deleted = false,
            DeletionTime = null,
            Data = new ExpandoObject(),
            DtroId = Guid.Parse("a954b3e2-a698-4006-9c2d-2a7b43a568a4")
        },
        new()
        {
            Id = Guid.Parse("998d8296-0f95-48de-9e25-fabf6537f472"),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.2"),
            TrafficAuthorityCreatorId = 1002,
            TrafficAuthorityOwnerId = 1002,
            Deleted = false,
            DeletionTime = null,
            Data = new ExpandoObject(),
            DtroId = Guid.Parse("4b9f4209-bba8-4dbb-97b1-defd55f56565")
        },
        new()
        {
            Id = Guid.Parse("5163a135-1e00-444e-a219-3780bf57649d"),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.3"),
            TrafficAuthorityCreatorId = 3300,
            TrafficAuthorityOwnerId = 3300,
            Deleted = false,
            DeletionTime = null,
            Data = new ExpandoObject(),
            DtroId = Guid.Parse("f571992f-7075-4b0e-b799-2e15acea4706")
        },
        new()
        {
            Id = Guid.Parse("3655dc94-999f-45fe-bd37-c37d68713af3"),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.4"),
            TrafficAuthorityCreatorId = 1050,
            TrafficAuthorityOwnerId = 1050,
            Deleted = false,
            DeletionTime = null,
            Data = new ExpandoObject(),
            DtroId = Guid.Parse("60c13911-9438-4c2f-aff4-58b934b0908a")
        }
    ];
}