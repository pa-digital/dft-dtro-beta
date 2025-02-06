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
            UserStatusId = Guid.Parse("2620b9a5-bc88-491a-82da-3060db1699e6"),
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
            UserStatusId = Guid.Parse("7cc4f298-c655-4268-984b-4a091b1a9b2e"),
            DigitalServiceProviders = []
        }
    ];

    public static List<UserStatus> UserStatuses =>
    [
        new() { Id = Guid.Parse(""), Status = "company" },

        new() { Id = Guid.Parse(""), Status = "hobbyist" }
    ];

    public static List<TrafficRegulationAuthority> TrafficRegulationAuthorities =>
    [
        new()
        {
            Id = Guid.Parse(""),
            Name = "First County Council",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = "approved"
        },

        new()
        {
            Id = Guid.Parse(""),
            Name = "Second County Council",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = "rejected"
        },

        new()
        {
            Id = Guid.Parse(""),
            Name = "Third County Council",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = "pending"
        },

        new()
        {
            Id = Guid.Parse(""),
            Name = "Fourth County Council",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = "on-review"
        },

        new()
        {
            Id = Guid.Parse(""),
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
            Id = Guid.Parse(""),
            Name = "First Publisher Company",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = "approved",
        },

        new()
        {
            Id = Guid.Parse(""),
            Name = "Second Publisher Company",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = "rejected",
        },

        new()
        {
            Id = Guid.Parse(""),
            Name = "Third Publisher Company",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = "pending",
        },

        new()
        {
            Id = Guid.Parse(""),
            Name = "Fourth Publisher Company",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = "on-review",
        },

        new()
        {
            Id = Guid.Parse(""),
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
            Id = Guid.Parse(""),
            DigitalServiceProviderId = Guid.Parse(""),
            TrafficRegulationAuthorityId = Guid.Parse(""),
            TrafficRegulationAuthorityDigitalServiceProviderStatusId = Guid.Parse("")
        },

        new()
        {
            Id = Guid.Parse(""),
            DigitalServiceProviderId = Guid.Parse(""),
            TrafficRegulationAuthorityId = Guid.Parse(""),
            TrafficRegulationAuthorityDigitalServiceProviderStatusId = Guid.Parse("")
        },

        new()
        {
            Id = Guid.Parse(""),
            DigitalServiceProviderId = Guid.Parse(""),
            TrafficRegulationAuthorityId = Guid.Parse(""),
            TrafficRegulationAuthorityDigitalServiceProviderStatusId = Guid.Parse("")
        },

        new()
        {
            Id = Guid.Parse(""),
            DigitalServiceProviderId = Guid.Parse(""),
            TrafficRegulationAuthorityId = Guid.Parse(""),
            TrafficRegulationAuthorityDigitalServiceProviderStatusId = Guid.Parse("")
        },

        new()
        {
            Id = Guid.Parse(""),
            DigitalServiceProviderId = Guid.Parse(""),
            TrafficRegulationAuthorityId = Guid.Parse(""),
            TrafficRegulationAuthorityDigitalServiceProviderStatusId = Guid.Parse("")
        }
    ];

    public static List<TrafficRegulationAuthorityDigitalServiceProviderStatus> TrafficRegulationAuthorityDigitalServiceProviderStatuses =>
    [
        new() { Id = Guid.Parse(""), Status = "accepted" },

        new() { Id = Guid.Parse(""), Status = "rejected" }
    ];

    public static List<Application> Applications =>
    [
        new()
        {
            Id = Guid.Parse(""),
            Name = "First-Publisher-Company-First-County-Council",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Nickname = "First Application",
            Status = "active",
            ApplicationPurposeId= Guid.Parse(""),
            ApplicationTypeId= Guid.Parse(""),
            TrafficRegulationAuthorityId = Guid.Parse(""),
            DigitalServiceProviderId= Guid.Parse("")
        },
        new()
        {
            Id = Guid.Parse(""),
            Name = "Second-Publisher-Company-Second-County-Council",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Nickname = "Second Application",
            Status = "active",
            ApplicationPurposeId= Guid.Parse(""),
            ApplicationTypeId= Guid.Parse(""),
            TrafficRegulationAuthorityId = Guid.Parse(""),
            DigitalServiceProviderId= Guid.Parse("")
        },
        new()
        {
            Id = Guid.Parse(""),
            Name = "Third-Publisher-Company-Third-County-Council",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Nickname = "Third Application",
            Status = "active",
            ApplicationPurposeId= Guid.Parse(""),
            ApplicationTypeId= Guid.Parse(""),
            TrafficRegulationAuthorityId = Guid.Parse(""),
            DigitalServiceProviderId= Guid.Parse("")
        },
        new()
        {
            Id = Guid.Parse(""),
            Name = "Fourth-Publisher-Company-Fourth-County-Council",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Nickname = "Fourth Application",
            Status = "active",
            ApplicationPurposeId= Guid.Parse(""),
            ApplicationTypeId= Guid.Parse(""),
            TrafficRegulationAuthorityId = Guid.Parse(""),
            DigitalServiceProviderId= Guid.Parse("")
        },
        new()
        {
            Id = Guid.Parse(""),
            Name = "Fifth-Publisher-Company-Fifth-County-Council",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Nickname = "Fifth Application",
            Status = "active",
            ApplicationPurposeId= Guid.Parse(""),
            ApplicationTypeId= Guid.Parse(""),
            TrafficRegulationAuthorityId = Guid.Parse(""),
            DigitalServiceProviderId= Guid.Parse("")
        }
        ];

    public static List<ApplicationType> ApplicationTypes =>
    [
        new()
        {
            Id = Guid.Parse(""),
            Name = "Publisher",
        },
        new()
        {
            Id = Guid.Parse(""),
            Name = "Publisher",
        },
        new()
        {
            Id = Guid.Parse(""),
            Name = "Publisher",
        },
        new()
        {
            Id = Guid.Parse(""),
            Name = "Publisher",
        },
        new()
        {
            Id = Guid.Parse(""),
            Name = "Publisher",
        }
    ];

    public static List<ApplicationPurpose> ApplicationPurposes =>
    [
        new()
        {
            Id = Guid.Parse(""),
            Description = "some description"
        },
        new()
        {
            Id = Guid.Parse(""),
            Description = "some other description"
        },
        new()
        {
            Id = Guid.Parse(""),
            Description = "lorem ipsum"
        },
        new()
        {
            Id = Guid.Parse(""),
            Description = "yet another description"
        },
        new()
        {
            Id = Guid.Parse(""),
            Description = "description"
        }
    ];

    public static List<SchemaTemplate> SchemaTemplates =>
    [
        new()
        {
            Id = Guid.Parse(""),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.0"),
            CreatedCorrelationId = "",
            LastUpdatedCorrelationId = "",
            Template = new ExpandoObject(),
            IsActive = true,
            Application = Applications.FirstOrDefault()
        },

        new()
        {
            Id = Guid.Parse(""),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.1"),
            CreatedCorrelationId = "",
            LastUpdatedCorrelationId = "",
            Template = new ExpandoObject(),
            IsActive = true,
            Application = Applications.FirstOrDefault()
        },

        new()
        {
            Id = Guid.Parse(""),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.2"),
            CreatedCorrelationId = "",
            LastUpdatedCorrelationId = "",
            Template = new ExpandoObject(),
            IsActive = true,
            Application = Applications.FirstOrDefault()
        },

        new()
        {
            Id = Guid.Parse(""),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.3"),
            CreatedCorrelationId = "",
            LastUpdatedCorrelationId = "",
            Template = new ExpandoObject(),
            IsActive = true,
            Application = Applications.FirstOrDefault()
        },

        new()
        {
            Id = Guid.Parse(""),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.4"),
            CreatedCorrelationId = "",
            LastUpdatedCorrelationId = "",
            Template = new ExpandoObject(),
            IsActive = true,
            Application = Applications.FirstOrDefault()
        }
    ];

    public static List<RuleTemplate> RuleTemplates =>
    [
        new()
        {
            Id = Guid.Parse(""),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.0"),
            CreatedCorrelationId = "",
            LastUpdatedCorrelationId = "",
            Template = "",
            Application = Applications.FirstOrDefault()
        },

        new()
        {
            Id = Guid.Parse(""),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.1"),
            CreatedCorrelationId = "",
            LastUpdatedCorrelationId = "",
            Template = "",
            Application = Applications.FirstOrDefault()
        },

        new()
        {
            Id = Guid.Parse(""),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.2"),
            CreatedCorrelationId = "",
            LastUpdatedCorrelationId = "",
            Template = "",
            Application = Applications.FirstOrDefault()
        },

        new()
        {
            Id = Guid.Parse(""),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.3"),
            CreatedCorrelationId = "",
            LastUpdatedCorrelationId = "",
            Template = "",
            Application = Applications.FirstOrDefault()
        },

        new()
        {
            Id = Guid.Parse(""),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.5"),
            CreatedCorrelationId = "",
            LastUpdatedCorrelationId = "",
            Template = "",
            Application = Applications.FirstOrDefault()
        }
    ];

    public static List<Models.DataBase.DTRO> DigitalTrafficRegulationOrders =>
    [
        new()
        {
            Id = Guid.Parse(""),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.0"),
            RegulationStart = new DateTime(2020, 1, 1, 1, 1, 1),
            RegulationEnd = new DateTime(2020, 2, 2, 2, 2, 2),
            TrafficAuthorityCreatorId = 1000,
            TrafficAuthorityOwnerId = 1000,
            TroName = "",
            CreatedCorrelationId = "",
            LastUpdatedCorrelationId = "",
            Deleted = false,
            DeletionTime = null,
            Data = new ExpandoObject(),
            RegulatedPlaceTypes = [],
            RegulationTypes = [],
            OrderReportingPoints = [],
            Location = new BoundingBox(),
            Application = Applications.FirstOrDefault()
        },

        new()
        {
            Id = Guid.Parse(""),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.1"),
            RegulationStart = new DateTime(2020, 1, 1, 1, 1, 1),
            RegulationEnd = new DateTime(2020, 2, 2, 2, 2, 2),
            TrafficAuthorityCreatorId = 1000,
            TrafficAuthorityOwnerId = 1000,
            TroName = "",
            CreatedCorrelationId = "",
            LastUpdatedCorrelationId = "",
            Deleted = false,
            DeletionTime = null,
            Data = new ExpandoObject(),
            RegulatedPlaceTypes = [],
            RegulationTypes = [],
            OrderReportingPoints = [],
            Location = new BoundingBox(),
            Application = Applications.FirstOrDefault()
        },

        new()
        {
            Id = Guid.Parse(""),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.2"),
            RegulationStart = new DateTime(2020, 1, 1, 1, 1, 1),
            RegulationEnd = new DateTime(2020, 2, 2, 2, 2, 2),
            TrafficAuthorityCreatorId = 1000,
            TrafficAuthorityOwnerId = 1000,
            TroName = "",
            CreatedCorrelationId = "",
            LastUpdatedCorrelationId = "",
            Deleted = false,
            DeletionTime = null,
            Data = new ExpandoObject(),
            RegulatedPlaceTypes = [],
            RegulationTypes = [],
            OrderReportingPoints = [],
            Location = new BoundingBox(),
            Application = Applications.FirstOrDefault()
        },

        new()
        {
            Id = Guid.Parse(""),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.3"),
            RegulationStart = new DateTime(2020, 1, 1, 1, 1, 1),
            RegulationEnd = new DateTime(2020, 2, 2, 2, 2, 2),
            TrafficAuthorityCreatorId = 1000,
            TrafficAuthorityOwnerId = 1000,
            TroName = "",
            CreatedCorrelationId = "",
            LastUpdatedCorrelationId = "",
            Deleted = false,
            DeletionTime = null,
            Data = new ExpandoObject(),
            RegulatedPlaceTypes = [],
            RegulationTypes = [],
            OrderReportingPoints = [],
            Location = new BoundingBox(),
            Application = Applications.FirstOrDefault()
        },

        new()
        {
            Id = Guid.Parse(""),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.4"),
            RegulationStart = new DateTime(2020, 1, 1, 1, 1, 1),
            RegulationEnd = new DateTime(2020, 2, 2, 2, 2, 2),
            TrafficAuthorityCreatorId = 1000,
            TrafficAuthorityOwnerId = 1000,
            TroName = "",
            CreatedCorrelationId = "",
            LastUpdatedCorrelationId = "",
            Deleted = false,
            DeletionTime = null,
            Data = new ExpandoObject(),
            RegulatedPlaceTypes = [],
            RegulationTypes = [],
            OrderReportingPoints = [],
            Location = new BoundingBox(),
            Application = Applications.FirstOrDefault()
        }
    ];

    public static List<DTROHistory> DigitalTrafficRegulationOrderHistories =>
    [
        new()
        {
            Id = Guid.Parse(""),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.0"),
            TrafficAuthorityCreatorId = 1000,
            TrafficAuthorityOwnerId = 1000,
            Deleted = false,
            DeletionTime = null,
            Data = new ExpandoObject(),
            DtroId = Guid.Parse("")
        },
        new()
        {
            Id = Guid.Parse(""),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.1"),
            TrafficAuthorityCreatorId = 1001,
            TrafficAuthorityOwnerId = 1001,
            Deleted = false,
            DeletionTime = null,
            Data = new ExpandoObject(),
            DtroId = Guid.Parse("")
        },
        new()
        {
            Id = Guid.Parse(""),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.2"),
            TrafficAuthorityCreatorId = 1002,
            TrafficAuthorityOwnerId = 1002,
            Deleted = false,
            DeletionTime = null,
            Data = new ExpandoObject(),
            DtroId = Guid.Parse("")
        },
        new()
        {
            Id = Guid.Parse(""),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.3"),
            TrafficAuthorityCreatorId = 3300,
            TrafficAuthorityOwnerId = 3300,
            Deleted = false,
            DeletionTime = null,
            Data = new ExpandoObject(),
            DtroId = Guid.Parse("")
        },
        new()
        {
            Id = Guid.Parse(""),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.4"),
            TrafficAuthorityCreatorId = 1050,
            TrafficAuthorityOwnerId = 1050,
            Deleted = false,
            DeletionTime = null,
            Data = new ExpandoObject(),
            DtroId = Guid.Parse("")
        }
    ];
}