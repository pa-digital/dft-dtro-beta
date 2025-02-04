namespace DfT.DTRO.Migrations;

/// <summary>
/// Seed data
/// </summary>
[ExcludeFromCodeCoverage]
public static class SeedData
{
    /// <summary>
    /// List of users
    /// </summary>
    public static List<User> Users => new()
    {
        new User
        {
            Id = Guid.NewGuid(),
            Name = "First User",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Forename = "John",
            Surname = "Doe",
            Email = "jon.doe@email.com",
            IsCentralServiceOperator = true,
            Status = true,
            UserStatus = UserStatuses.FirstOrDefault(userStatus=>userStatus.Status),
            DigitalServiceProviders = new List<DigitalServiceProvider>(),
        },
        new User
        {
            Id = Guid.NewGuid(),
            Name = "Second User",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Forename = "Jane",
            Surname = "Doe",
            Email = "jane.doe@email.com",
            IsCentralServiceOperator = false,
            Status = true,
            UserStatus = UserStatuses.FirstOrDefault(userStatus=>userStatus.Status),
            DigitalServiceProviders = new List<DigitalServiceProvider>()
        },
        new User
        {
            Id = Guid.NewGuid(),
            Name = "Third User",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Forename = "Jake",
            Surname = "Blogs",
            Email = "george.blogs@email.com",
            IsCentralServiceOperator = false,
            Status = true,
            UserStatus = UserStatuses.FirstOrDefault(userStatus=>userStatus.Status),
            DigitalServiceProviders = new List<DigitalServiceProvider>()
        },
        new User
        {
            Id = Guid.NewGuid(),
            Name = "Fourth User",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Forename = "Jenny",
            Surname = "Blogs",
            Email = "jenny.blogs@email.com",
            IsCentralServiceOperator = false,
            Status = true,
            UserStatus = UserStatuses.FirstOrDefault(userStatus=>userStatus.Status),
            DigitalServiceProviders = new List<DigitalServiceProvider>()
        }

    };

    /// <summary>
    /// List of user statuses 
    /// </summary>
    public static List<UserStatus> UserStatuses => new() {
        new UserStatus
        {
            Id = Guid.NewGuid(),
            Name = "Active",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = true,
        },
        new UserStatus
        {
            Id = Guid.NewGuid(),
            Name = "Inactive",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = false,
        }
    };

    /// <summary>
    /// List of traffic regulation authorities
    /// </summary>
    public static List<TrafficRegulationAuthority> TrafficRegulationAuthorities => new()
    {
        new TrafficRegulationAuthority
        {
            Id = Guid.NewGuid(),
            Name = "First County Council",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = true
        },
        new TrafficRegulationAuthority
        {
            Id = Guid.NewGuid(),
            Name = "Second County Council",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = true
        },
        new TrafficRegulationAuthority
        {
            Id = Guid.NewGuid(),
            Name = "Third County Council",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = true
        },
        new TrafficRegulationAuthority
        {
            Id = Guid.NewGuid(),
            Name = "Fourth County Council",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = true
        },
        new TrafficRegulationAuthority
        {
            Id = Guid.NewGuid(),
            Name = "Fifth County Council",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = true
        }
    };

    /// <summary>
    /// List of digital service providers
    /// </summary>
    public static List<DigitalServiceProvider> DigitalServiceProviders => new()
    {
        new DigitalServiceProvider
        {
            Id = Guid.NewGuid(),
            Name = "First Publisher Company",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = true,
        },
        new DigitalServiceProvider
        {
            Id = Guid.NewGuid(),
            Name = "Second Publisher Company",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = true,
        },
        new DigitalServiceProvider
        {
            Id = Guid.NewGuid(),
            Name = "Third Publisher Company",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = true,
        },
        new DigitalServiceProvider
        {
            Id = Guid.NewGuid(),
            Name = "Fourth Publisher Company",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = true,
        },
        new DigitalServiceProvider
        {
            Id = Guid.NewGuid(),
            Name = "Fifth Publisher Company",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = false,
        }
    };

    /// <summary>
    /// List of traffic regulation authority and digital service provider
    /// </summary>
    public static List<TrafficRegulationAuthorityDigitalServiceProvider> TrafficRegulationAuthorityDigitalServiceProviders =>
        new()
        {
            new TrafficRegulationAuthorityDigitalServiceProvider
            {
                Id = Guid.NewGuid(),
                Name = "",
                Created = DateTime.UtcNow,
                LastUpdated = null,
                Status = true,
                DigitalServiceProviders = new List<DigitalServiceProvider>
                {
                    DigitalServiceProviders.FirstOrDefault()
                },
                TrafficRegulationAuthorities = new List<TrafficRegulationAuthority>
                {
                    TrafficRegulationAuthorities.FirstOrDefault()
                },
                TrafficRegulationAuthorityDigitalServiceProviderStatus = TrafficRegulationAuthorityDigitalServiceProviderStatuses.First()
            },
            new TrafficRegulationAuthorityDigitalServiceProvider
            {
                Id = Guid.NewGuid(),
                Name = "",
                Created = DateTime.UtcNow,
                LastUpdated = null,
                Status = true,
                DigitalServiceProviders = new List<DigitalServiceProvider>
                {
                    DigitalServiceProviders.Skip(1).FirstOrDefault()
                },
                TrafficRegulationAuthorities = new List<TrafficRegulationAuthority>
                {
                    TrafficRegulationAuthorities.Skip(1).FirstOrDefault()
                },
                TrafficRegulationAuthorityDigitalServiceProviderStatus = TrafficRegulationAuthorityDigitalServiceProviderStatuses.First()
            },
            new TrafficRegulationAuthorityDigitalServiceProvider
            {
                Id = Guid.NewGuid(),
                Name = "",
                Created = DateTime.UtcNow,
                LastUpdated = null,
                Status = true,
                DigitalServiceProviders = new List<DigitalServiceProvider>
                {
                    DigitalServiceProviders.Skip(2).FirstOrDefault()
                },
                TrafficRegulationAuthorities = new List<TrafficRegulationAuthority>
                {
                    TrafficRegulationAuthorities.Skip(2).FirstOrDefault()
                },
                TrafficRegulationAuthorityDigitalServiceProviderStatus = TrafficRegulationAuthorityDigitalServiceProviderStatuses.First()
            },
            new TrafficRegulationAuthorityDigitalServiceProvider
            {
                Id = Guid.NewGuid(),
                Name = "",
                Created = DateTime.UtcNow,
                LastUpdated = null,
                Status = true,
                DigitalServiceProviders = new List<DigitalServiceProvider>
                {
                    DigitalServiceProviders.Skip(3).FirstOrDefault()
                },
                TrafficRegulationAuthorities = new List<TrafficRegulationAuthority>
                {
                    TrafficRegulationAuthorities.Skip(3).FirstOrDefault()
                },
                TrafficRegulationAuthorityDigitalServiceProviderStatus = TrafficRegulationAuthorityDigitalServiceProviderStatuses.First()
            },
            new TrafficRegulationAuthorityDigitalServiceProvider
            {
                Id = Guid.NewGuid(),
                Name = "",
                Created = DateTime.UtcNow,
                LastUpdated = null,
                Status = true,
                DigitalServiceProviders = new List<DigitalServiceProvider>
                {
                    DigitalServiceProviders.Skip(4).FirstOrDefault()
                },
                TrafficRegulationAuthorities = new List<TrafficRegulationAuthority>
                {
                    TrafficRegulationAuthorities.Skip(4).FirstOrDefault()
                },
                TrafficRegulationAuthorityDigitalServiceProviderStatus = TrafficRegulationAuthorityDigitalServiceProviderStatuses.First()
            }
        };

    /// <summary>
    /// List of traffic regulation authority digital service provider status
    /// </summary>
    public static List<TrafficRegulationAuthorityDigitalServiceProviderStatus> TrafficRegulationAuthorityDigitalServiceProviderStatuses => new()
    {
        new TrafficRegulationAuthorityDigitalServiceProviderStatus
        {
            Id = Guid.NewGuid(),
            Name = "Active",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = true
        },
        new TrafficRegulationAuthorityDigitalServiceProviderStatus
        {
            Id = Guid.NewGuid(),
            Name = "Inactive",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = false
        }
    };

    /// <summary>
    /// List of application
    /// </summary>
    public static List<Application> Applications => new()
    {
        new Application
        {
            Id = Guid.NewGuid(),
            Name = "First Application",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Nickname = "First-Publisher-Company-First-County-Council",
            Status = true,
            ApplicationPurpose = ApplicationPurposes.FirstOrDefault(),
            ApplicationType = ApplicationTypes.FirstOrDefault(),
            TrafficRegulationAuthorities = new List<TrafficRegulationAuthority>
            {
                TrafficRegulationAuthorities.FirstOrDefault()
            },
            DigitalServiceProviders = new List<DigitalServiceProvider>
            {
                DigitalServiceProviders.FirstOrDefault()
            }
        },
        new Application
        {
            Id = Guid.NewGuid(),
            Name = "Second Application",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Nickname = "Second-Publisher-Company-Second-County-Council",
            Status = true,
            ApplicationPurpose = ApplicationPurposes.FirstOrDefault(),
            ApplicationType = ApplicationTypes.FirstOrDefault(),
            TrafficRegulationAuthorities = new List<TrafficRegulationAuthority>
            {
                TrafficRegulationAuthorities.Skip(1).FirstOrDefault()
            },
            DigitalServiceProviders = new List<DigitalServiceProvider>
            {
                DigitalServiceProviders.Skip(1).FirstOrDefault()
            }
        },
        new Application
        {
            Id = Guid.NewGuid(),
            Name = "Third Application",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Nickname = "Third-Publisher-Company-Third-County-Council",
            Status = true,
            ApplicationPurpose = ApplicationPurposes.FirstOrDefault(),
            ApplicationType = ApplicationTypes.FirstOrDefault(),
            TrafficRegulationAuthorities = new List<TrafficRegulationAuthority>
            {
                TrafficRegulationAuthorities.Skip(2).FirstOrDefault()
            },
            DigitalServiceProviders = new List<DigitalServiceProvider>
            {
                DigitalServiceProviders.Skip(2).FirstOrDefault()
            }
        },
        new Application
        {
            Id = Guid.NewGuid(),
            Name = "Fourth Application",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Nickname = "Fourth-Publisher-Company-Fourth-County-Council",
            Status = true,
            ApplicationPurpose = ApplicationPurposes.FirstOrDefault(),
            ApplicationType = ApplicationTypes.FirstOrDefault(),
            TrafficRegulationAuthorities = new List<TrafficRegulationAuthority>
            {
                TrafficRegulationAuthorities.Skip(3).FirstOrDefault()
            },
            DigitalServiceProviders = new List<DigitalServiceProvider>
            {
                DigitalServiceProviders.Skip(3).FirstOrDefault()
            }
        },
        new Application
        {
            Id = Guid.NewGuid(),
            Name = "Fifth Application",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Nickname = "Fifth-Publisher-Company-Fifth-County-Council",
            Status = true,
            ApplicationPurpose = ApplicationPurposes.FirstOrDefault(),
            ApplicationType = ApplicationTypes.FirstOrDefault(),
            TrafficRegulationAuthorities = new List<TrafficRegulationAuthority>
            {
                TrafficRegulationAuthorities.Skip(4).FirstOrDefault()
            },
            DigitalServiceProviders = new List<DigitalServiceProvider>
            {
                DigitalServiceProviders.Skip(4).FirstOrDefault()
            }
        }
    };

    /// <summary>
    /// List of application types
    /// </summary>
    public static List<ApplicationType> ApplicationTypes => new()
    {
        new ApplicationType
        {
            Id = Guid.NewGuid(),
            Name = "Publisher",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = true
        },
        new ApplicationType
        {
            Id = Guid.NewGuid(),
            Name = "Publisher",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = true
        },
        new ApplicationType
        {
            Id = Guid.NewGuid(),
            Name = "Publisher",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = true
        },
        new ApplicationType
        {
            Id = Guid.NewGuid(),
            Name = "Publisher",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = true
        },
        new ApplicationType
        {
            Id = Guid.NewGuid(),
            Name = "Publisher",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = true
        }
    };

    /// <summary>
    /// List of application purpose
    /// </summary>
    public static List<ApplicationPurpose> ApplicationPurposes => new()
    {
        new ApplicationPurpose
        {
            Id = Guid.NewGuid(),
            Name = "Publisher",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = true
        },
        new ApplicationPurpose
        {
            Id = Guid.NewGuid(),
            Name = "Administrator",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = true
        },
        new ApplicationPurpose
        {
            Id = Guid.NewGuid(),
            Name = "Consumer",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = true
        }
    };

    /// <summary>
    /// List of schema templates
    /// </summary>
    public static List<SchemaTemplate> SchemaTemplates => new()
    {
        new SchemaTemplate
        {
            Id = Guid.NewGuid(),
            Name = "First schema template",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.0"),
            CreatedCorrelationId = "",
            LastUpdatedCorrelationId = "",
            Template = new ExpandoObject(),
            Status = true,
            IsActive = true,
            Application = Applications.FirstOrDefault()
        },
        new SchemaTemplate
        {
            Id = Guid.NewGuid(),
            Name = "First schema template",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.1"),
            CreatedCorrelationId = "",
            LastUpdatedCorrelationId = "",
            Template = new ExpandoObject(),
            Status = true,
            IsActive = true,
            Application = Applications.FirstOrDefault()
        },
        new SchemaTemplate
        {
            Id = Guid.NewGuid(),
            Name = "First schema template",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.2"),
            CreatedCorrelationId = "",
            LastUpdatedCorrelationId = "",
            Template = new ExpandoObject(),
            Status = true,
            IsActive = true,
            Application = Applications.FirstOrDefault()
        },
        new SchemaTemplate
        {
            Id = Guid.NewGuid(),
            Name = "First schema template",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.3"),
            CreatedCorrelationId = "",
            LastUpdatedCorrelationId = "",
            Template = new ExpandoObject(),
            Status = true,
            IsActive = true,
            Application = Applications.FirstOrDefault()
        },
        new SchemaTemplate
        {
            Id = Guid.NewGuid(),
            Name = "First schema template",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.4"),
            CreatedCorrelationId = "",
            LastUpdatedCorrelationId = "",
            Template = new ExpandoObject(),
            Status = true,
            IsActive = true,
            Application = Applications.FirstOrDefault()
        }
    };

    /// <summary>
    /// List of rule templates
    /// </summary>
    public static List<RuleTemplate> RuleTemplates => new()
    {
        new RuleTemplate
        {
            Id = Guid.NewGuid(),
            Name = "First Rule template",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.0"),
            CreatedCorrelationId = "",
            LastUpdatedCorrelationId = "",
            Template = "",
            Status = true,
            Application = Applications.FirstOrDefault()
        },
        new RuleTemplate
        {
            Id = Guid.NewGuid(),
            Name = "Second Rule template",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.1"),
            CreatedCorrelationId = "",
            LastUpdatedCorrelationId = "",
            Template = "",
            Status = true,
            Application = Applications.FirstOrDefault()
        },
        new RuleTemplate
        {
            Id = Guid.NewGuid(),
            Name = "Third Rule template",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.2"),
            CreatedCorrelationId = "",
            LastUpdatedCorrelationId = "",
            Template = "",
            Status = true,
            Application = Applications.FirstOrDefault()
        },
        new RuleTemplate
        {
            Id = Guid.NewGuid(),
            Name = "Fourth Rule template",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.3"),
            CreatedCorrelationId = "",
            LastUpdatedCorrelationId = "",
            Template = "",
            Status = true,
            Application = Applications.FirstOrDefault()
        },
        new RuleTemplate
        {
            Id = Guid.NewGuid(),
            Name = "Fifth Rule template",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.5"),
            CreatedCorrelationId = "",
            LastUpdatedCorrelationId = "",
            Template = "",
            Status = true,
            Application = Applications.FirstOrDefault()
        }
    };

    /// <summary>
    /// List of digital traffic regulation order
    /// </summary>
    public static List<DigitalTrafficRegulationOrder> DigitalTrafficRegulationOrders => new()
    {
        new DigitalTrafficRegulationOrder
        {
            Id = Guid.NewGuid(),
            Name = "First Record",
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
            RegulatedPlaceTypes = new List<string>(),
            RegulationTypes = new List<string>(),
            OrderReportingPoints = new List<string>(),
            Location = new BoundingBox(),
            Status = true,
            Application = Applications.FirstOrDefault()
        },
        new DigitalTrafficRegulationOrder
        {
            Id = Guid.NewGuid(),
            Name = "Second Record",
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
            RegulatedPlaceTypes = new List<string>(),
            RegulationTypes = new List<string>(),
            OrderReportingPoints = new List<string>(),
            Location = new BoundingBox(),
            Status = true,
            Application = Applications.FirstOrDefault()
        },
        new DigitalTrafficRegulationOrder
        {
            Id = Guid.NewGuid(),
            Name = "Third Record",
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
            RegulatedPlaceTypes = new List<string>(),
            RegulationTypes = new List<string>(),
            OrderReportingPoints = new List<string>(),
            Location = new BoundingBox(),
            Status = true,
            Application = Applications.FirstOrDefault()
        },
        new DigitalTrafficRegulationOrder
        {
            Id = Guid.NewGuid(),
            Name = "Fourth Record",
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
            RegulatedPlaceTypes = new List<string>(),
            RegulationTypes = new List<string>(),
            OrderReportingPoints = new List<string>(),
            Location = new BoundingBox(),
            Status = true,
            Application = Applications.FirstOrDefault()
        },
        new DigitalTrafficRegulationOrder
        {
            Id = Guid.NewGuid(),
            Name = "Fifth Record",
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
            RegulatedPlaceTypes = new List<string>(),
            RegulationTypes = new List<string>(),
            OrderReportingPoints = new List<string>(),
            Location = new BoundingBox(),
            Status = true,
            Application = Applications.FirstOrDefault()
        }
    };

    /// <summary>
    /// List of digital traffic regulation order history
    /// </summary>
    public static List<DigitalTrafficRegulationOrderHistory> DigitalTrafficRegulationOrderHistories => new()
    {
        new DigitalTrafficRegulationOrderHistory
        {
            Id = Guid.NewGuid(),
            Name = "First Record",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.0"),
            TrafficAuthorityCreatorId = 1000,
            TrafficAuthorityOwnerId = 1000,
            Deleted = false,
            DeletionTime = null,
            Data = new ExpandoObject(),
            Status = true,
            DigitalTrafficRegulationOrderId = DigitalTrafficRegulationOrders.First().Id
        },
        new DigitalTrafficRegulationOrderHistory
        {
            Id = Guid.NewGuid(),
            Name = "Second Record",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.1"),
            TrafficAuthorityCreatorId = 1000,
            TrafficAuthorityOwnerId = 1000,
            Deleted = false,
            DeletionTime = null,
            Data = new ExpandoObject(),
            Status = true,
            DigitalTrafficRegulationOrderId = DigitalTrafficRegulationOrders.Skip(1).First().Id
        },
        new DigitalTrafficRegulationOrderHistory
        {
            Id = Guid.NewGuid(),
            Name = "Third Record",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.2"),
            TrafficAuthorityCreatorId = 1000,
            TrafficAuthorityOwnerId = 1000,
            Deleted = false,
            DeletionTime = null,
            Data = new ExpandoObject(),
            Status = true,
            DigitalTrafficRegulationOrderId = DigitalTrafficRegulationOrders.Skip(2).First().Id
        },
        new DigitalTrafficRegulationOrderHistory
        {
            Id = Guid.NewGuid(),
            Name = "Fourth Record",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.3"),
            TrafficAuthorityCreatorId = 1000,
            TrafficAuthorityOwnerId = 1000,
            Deleted = false,
            DeletionTime = null,
            Data = new ExpandoObject(),
            Status = true,
            DigitalTrafficRegulationOrderId = DigitalTrafficRegulationOrders.Skip(3).First().Id
        },
        new DigitalTrafficRegulationOrderHistory
        {
            Id = Guid.NewGuid(),
            Name = "Fifth Record",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.4"),
            TrafficAuthorityCreatorId = 1000,
            TrafficAuthorityOwnerId = 1000,
            Deleted = false,
            DeletionTime = null,
            Data = new ExpandoObject(),
            Status = true,
            DigitalTrafficRegulationOrderId = DigitalTrafficRegulationOrders.Skip(4).First().Id
        }
    };

    /// <summary>
    /// System configuration
    /// </summary>
    public static readonly SystemConfig SystemConfig = new()
    {
        Id = new Guid("a7ab6da8-4d24-4f7f-a58b-fb7443ae8abe"),
        SystemName = "TRA Test System",
        IsTest = true
    };

    /// <summary>
    /// Traffic authorities
    /// </summary>
    public static readonly List<DtroUser> TrafficAuthorities = new()
    {
        new DtroUser
        {
            Id = new Guid("67d2adeb-31ac-4962-8025-c14ef2aa7236"),
            xAppId = Guid.Empty,
            UserGroup = (int) UserGroup.Admin,
            Name = "Department of Transport",
            Prefix = "",
            TraId = null
        }
    };

}

