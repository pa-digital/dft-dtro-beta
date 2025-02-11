namespace DfT.DTRO.DatabaseFeeder;

public static class DatabaseFeeder
{
    public static async Task Seed<T>(DtroContext context, List<T> items) where T : class
    {
        var entities = await context.Set<T>().ToListAsync();
        if (entities.Count != 0)
        {
            return;
        }

        await context.Set<T>().AddRangeAsync(items);
        await context.SaveChangesAsync();
    }

    public static List<User> Users =>
    [
        new()
        {
            Id = Guid.Parse("ec264d8e-80f5-41fc-86c9-2c391acea9d7"),
            Forename = "Jon",
            Surname = "Doe",
            Email = "jon.doe@email.com",
            IsCentralServiceOperator = true,
            Created = DateTime.UtcNow,
            LastUpdated = null,
            UserStatus = UserStatuses.FirstOrDefault(it=>it.Id==Guid.Parse("da6e869a-631c-4158-acba-b606cb82bad1"))
        },
        new()
        {
            Id = Guid.Parse("425b4e39-bf0b-4e1d-8aed-4ee3e5f385b8"),
            Forename = "Jane",
            Surname = "Doe",
            Email = "jane.doe@email.com",
            IsCentralServiceOperator = false,
            Created = DateTime.UtcNow,
            LastUpdated = null,
            UserStatus = UserStatuses.FirstOrDefault(it=>it.Id==Guid.Parse("da6e869a-631c-4158-acba-b606cb82bad1"))
        }
    ];

    public static List<UserStatus> UserStatuses =>
    [
        new() { Id = Guid.Parse("da6e869a-631c-4158-acba-b606cb82bad1"), Status = "accepted" },
        new() { Id = Guid.Parse("ef2d75a9-38b4-4b48-9e29-41870b223e47"), Status = "pending" },
        new() { Id = Guid.Parse("a6bf4c36-2a5d-49ed-8d92-453d385f0fa5"), Status = "rejected" },
    ];

    public static List<DigitalServiceProvider> DigitalServiceProviders =>
    [
        new()
        {
            Id = Guid.Parse("6a977466-5ad2-4a28-80c0-3eebe977653d"),
            Name = "First Digital Service Provider",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = "accepted",
            Applications = [Applications.FirstOrDefault()],
            TrafficRegulationAuthorityDigitalServiceProviders = [TrafficRegulationAuthorityDigitalServiceProviders.FirstOrDefault()],
            Users = [Users.FirstOrDefault()]
        }
    ];

    public static List<TrafficRegulationAuthority> TrafficRegulationAuthorities =>
    [
        new()
        {
            Id = Guid.Parse("bfd3184c-8105-4ab4-b4cc-c6c09baf5df1"),
            Name = "",
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Status = "",
            TrafficRegulationAuthorityDigitalServiceProviders = [TrafficRegulationAuthorityDigitalServiceProviders.FirstOrDefault()],
            Applications = [Applications.FirstOrDefault()]
        }
    ];

    public static List<TrafficRegulationAuthorityDigitalServiceProvider> TrafficRegulationAuthorityDigitalServiceProviders =>
    [
        new() { Id = Guid.Parse("6a977466-5ad2-4a28-80c0-3eebe977653d") }
    ];

    public static List<TrafficRegulationAuthorityDigitalServiceProviderStatus> TrafficRegulationAuthorityDigitalServiceProviderStatuses =>
    [
        new()
        {
            Id = Guid.Parse("429cfe4f-d159-44a9-9d1f-0b9bfa4d0aa2"),
            TrafficRegulationAuthorityDigitalServiceProvider = TrafficRegulationAuthorityDigitalServiceProviders.FirstOrDefault()
        }
    ];

    public static List<Application> Applications =>
    [
        new()
        {
            Id = Guid.Parse("2c61985e-3962-4f39-aacc-1fe4ccb05448"),
            Nickname = "",
            Status = "",
            ApplicationTypes = [ApplicationTypes.FirstOrDefault()],
            Created = DateTime.UtcNow,
            LastUpdated = null,
            Dtros = [Dtros.FirstOrDefault()],
            RuleTemplates = [RuleTemplates.FirstOrDefault()],
            SchemaTemplates = [SchemaTemplates.FirstOrDefault()]
        }
    ];

    public static List<ApplicationPurpose> ApplicationPurposes =>
    [
        new()
        {
            Id = Guid.Parse("0030ac9e-eb59-4a7a-acd2-86995cb28d87"),
            Application = Applications.FirstOrDefault(),
            Description = ""
        }
    ];

    public static List<ApplicationType> ApplicationTypes =>
    [
        new() { Id = Guid.Parse("99b48464-814c-4840-b10e-f08327d6dbc8"), Name = "company" },
        new() { Id = Guid.Parse("03f79cd8-5103-4345-9167-ce9dee704565"), Name = "hobbyist" },
        new() { Id = Guid.Parse("8b1678a2-1189-4e76-986c-36c4d779a60d"), Name = "consumer" }
    ];

    public static List<Models.DataBase.DTRO> Dtros =>
    [
        new()
        {
            Id = Guid.Parse("5f9423c3-2615-4d5a-bde0-be540d3664b8"),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            CreatedCorrelationId = "41ae0471-d7de-4737-907f-cab2f0089796",
            LastUpdatedCorrelationId = "41ae0471-d7de-4737-907f-cab2f0089796",
            Data = new ExpandoObject(),
            Location = BoundingBox.ForWktSrid27700,
            OrderReportingPoints = [],
            RegulatedPlaceTypes = [],
            RegulationTypes = [],
            VehicleTypes = [],
            SchemaVersion = new SchemaVersion("1.0.0"),
            TrafficAuthorityCreatorId = 1000,
            TrafficAuthorityOwnerId = 1000,
            TroName = ""
        }
    ];

    public static List<DTROHistory> DtroHistories =>
    [
        new()
        {
            Id = Guid.Parse("6fff5f7c-0003-4f39-8bfe-e36f1546e3e5"),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.0"),
            Data = new ExpandoObject(),
            TrafficAuthorityCreatorId = 1000,
            TrafficAuthorityOwnerId = 1000,
            DtroId = Dtros.First().Id
        }
    ];

    public static List<RuleTemplate> RuleTemplates =>
    [
        new()
        {
            Id = Guid.Parse("d09f0d19-c8cf-4654-9813-640e1278d9ca"),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.0"),
            CreatedCorrelationId = "41ae0471-d7de-4737-907f-cab2f0089796",
            LastUpdatedCorrelationId = "41ae0471-d7de-4737-907f-cab2f0089796",
            Template = ""
        }
    ];

    public static List<SchemaTemplate> SchemaTemplates =>
    [
        new()
        {
            Id = Guid.Parse("1c2d6e30-6992-483a-992c-f736abd32b91"),
            Created = DateTime.UtcNow,
            LastUpdated = null,
            SchemaVersion = new SchemaVersion("1.0.0"),
            CreatedCorrelationId = "41ae0471-d7de-4737-907f-cab2f0089796",
            LastUpdatedCorrelationId = "41ae0471-d7de-4737-907f-cab2f0089796",
            Template = new ExpandoObject(),
            IsActive = true
        }
    ];
}