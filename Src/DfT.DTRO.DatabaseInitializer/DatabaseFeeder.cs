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

    /// <summary>
    /// List of users
    /// </summary>
    public static List<User> Users =>
    [
        new()
        {
            Id = Guid.Parse(""),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Forename = "John",
            Surname = "Doe",
            Email = "jon.doe@email.com",
            IsCentralServiceOperator = true,
            Status = "active",
            UserStatusId = Guid.Parse(""),
            DigitalServiceProviders = [],
        },

        new()
        {
            Id = Guid.Parse(""),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Forename = "Jane",
            Surname = "Doe",
            Email = "jane.doe@email.com",
            IsCentralServiceOperator = false,
            Status = "active",
            UserStatusId = Guid.Parse(""),
            DigitalServiceProviders = []
        },

        new()
        {
            Id = Guid.NewGuid(),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Forename = "Jake",
            Surname = "Blogs",
            Email = "george.blogs@email.com",
            IsCentralServiceOperator = false,
            Status = "active",
            UserStatusId = Guid.Parse(""),
            DigitalServiceProviders = []
        },

        new()
        {
            Id = Guid.NewGuid(),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Forename = "Jenny",
            Surname = "Blogs",
            Email = "jenny.blogs@email.com",
            IsCentralServiceOperator = false,
            Status = "active",
            UserStatusId = Guid.Parse(""),
            DigitalServiceProviders = []
        }
    ];

    /// <summary>
    /// List of user statuses 
    /// </summary>
    public static List<UserStatus> UserStatuses =>
    [
        new() { Id = Guid.NewGuid(), Status = "company" },

        new() { Id = Guid.NewGuid(), Status = "hobbyist" }
    ];

    /// <summary>
    /// List of traffic regulation authorities
    /// </summary>
    public static List<TrafficRegulationAuthority> TrafficRegulationAuthorities =>
    [
        new()
        {
            Id = Guid.NewGuid(),
            Name = "First County Council",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = "approved"
        },

        new()
        {
            Id = Guid.NewGuid(),
            Name = "Second County Council",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = "rejected"
        },

        new()
        {
            Id = Guid.NewGuid(),
            Name = "Third County Council",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = "pending"
        },

        new()
        {
            Id = Guid.NewGuid(),
            Name = "Fourth County Council",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = "on-review"
        },

        new()
        {
            Id = Guid.NewGuid(),
            Name = "Fifth County Council",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = "approved"
        }
    ];

    /// <summary>
    /// List of digital service providers
    /// </summary>
    public static List<DigitalServiceProvider> DigitalServiceProviders =>
    [
        new()
        {
            Id = Guid.NewGuid(),
            Name = "First Publisher Company",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = "approved",
        },

        new()
        {
            Id = Guid.NewGuid(),
            Name = "Second Publisher Company",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = "rejected",
        },

        new()
        {
            Id = Guid.NewGuid(),
            Name = "Third Publisher Company",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = "pending",
        },

        new()
        {
            Id = Guid.NewGuid(),
            Name = "Fourth Publisher Company",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = "on-review",
        },

        new()
        {
            Id = Guid.NewGuid(),
            Name = "Fifth Publisher Company",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = "approved",
        }
    ];

    /// <summary>
    /// List of traffic regulation authority and digital service provider
    /// </summary>
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

    /// <summary>
    /// List of traffic regulation authority digital service provider status
    /// </summary>
    public static List<TrafficRegulationAuthorityDigitalServiceProviderStatus> TrafficRegulationAuthorityDigitalServiceProviderStatuses => new()
    {
        new TrafficRegulationAuthorityDigitalServiceProviderStatus
        {
            Id = Guid.Parse(""),
            Status = "accepted"
        },
        new TrafficRegulationAuthorityDigitalServiceProviderStatus
        {
            Id = Guid.Parse(""),
            Status = "rejected"
        }
    };

    /// <summary>
    /// List of application
    /// </summary>
    public static List<Application> Applications =>
    [
        new()
        {
            Id = Guid.NewGuid(),
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
            Id = Guid.NewGuid(),
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
            Id = Guid.NewGuid(),
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
            Id = Guid.NewGuid(),
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
            Id = Guid.NewGuid(),
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

    /// <summary>
    /// List of application types
    /// </summary>
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

    /// <summary>
    /// List of application purpose
    /// </summary>
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

    /// <summary>
    /// List of schema templates
    /// </summary>
    public static List<SchemaTemplate> SchemaTemplates =>
    [
        new()
        {
            Id = Guid.NewGuid(),
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
            Id = Guid.NewGuid(),
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
            Id = Guid.NewGuid(),
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
            Id = Guid.NewGuid(),
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
            Id = Guid.NewGuid(),
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

    /// <summary>
    /// List of rule templates
    /// </summary>
    public static List<RuleTemplate> RuleTemplates =>
    [
        new()
        {
            Id = Guid.NewGuid(),
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
            Id = Guid.NewGuid(),
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
            Id = Guid.NewGuid(),
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
            Id = Guid.NewGuid(),
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
            Id = Guid.NewGuid(),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.5"),
            CreatedCorrelationId = "",
            LastUpdatedCorrelationId = "",
            Template = "",
            Application = Applications.FirstOrDefault()
        }
    ];

    /// <summary>
    /// List of digital traffic regulation order
    /// </summary>
    public static List<Models.DataBase.DTRO> DigitalTrafficRegulationOrders => new()
    {
        new Models.DataBase.DTRO
        {
            Id = Guid.NewGuid(),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.0"),
            RegulationStart = new DateTime(2020,1,1,1,1,1),
            RegulationEnd = new DateTime(2020,2,2,2,2,2),
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
        new Models.DataBase.DTRO
        {
            Id = Guid.NewGuid(),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.1"),
            RegulationStart = new DateTime(2020,1,1,1,1,1),
            RegulationEnd = new DateTime(2020,2,2,2,2,2),
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
        new Models.DataBase.DTRO
        {
            Id = Guid.NewGuid(),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.2"),
            RegulationStart = new DateTime(2020,1,1,1,1,1),
            RegulationEnd = new DateTime(2020,2,2,2,2,2),
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
        new Models.DataBase.DTRO
        {
            Id = Guid.NewGuid(),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.3"),
            RegulationStart = new DateTime(2020,1,1,1,1,1),
            RegulationEnd = new DateTime(2020,2,2,2,2,2),
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
        new Models.DataBase.DTRO
        {
            Id = Guid.NewGuid(),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.4"),
            RegulationStart = new DateTime(2020,1,1,1,1,1),
            RegulationEnd = new DateTime(2020,2,2,2,2,2),
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
    };

    /// <summary>
    /// List of digital traffic regulation order history
    /// </summary>
    public static List<DTROHistory> DigitalTrafficRegulationOrderHistories =>
    [
        new()
        {
            Id = Guid.NewGuid(),
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
            Id = Guid.NewGuid(),
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
            Id = Guid.NewGuid(),
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
            Id = Guid.NewGuid(),
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
            Id = Guid.NewGuid(),
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