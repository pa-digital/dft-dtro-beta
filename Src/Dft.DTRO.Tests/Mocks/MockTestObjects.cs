namespace Dft.DTRO.Tests.Mocks;

[ExcludeFromCodeCoverage]
public static class MockTestObjects
{
    public static IFormFile TestFile
    {
        get
        {
            var bytes = Encoding.UTF8.GetBytes("{ \"schemaVersion\": \"1.0.0\", \"data\": { \"item\": \"This is a test file\"}}");
            IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.txt");
            return file;
        }
    }

    public static GuidResponse GuidResponse =>
        new()
        {
            Id = Guid.NewGuid()
        };

    public static List<DtroUserResponse> UserResponses => new() {
        new DtroUserResponse
        {
            Id = Guid.NewGuid(),
            Name  = "Somerset Council",
            TraId = 3300,
            Prefix = "LG",
            UserGroup = UserGroup.Tra,
            xAppId = Guid.NewGuid()
        },
        new DtroUserResponse
        {
            Id = Guid.NewGuid(),
            Name  = "Derbyshire",
            TraId = 1050,
            Prefix = "DJ",
            UserGroup = UserGroup.Tra,
            xAppId = Guid.NewGuid()
        },
        new DtroUserResponse
        {
            Id = Guid.NewGuid(),
            Name  = "Appyway",
            TraId = 4,
            Prefix = "AP",
            UserGroup = UserGroup.Tra,
            xAppId = Guid.NewGuid()
        }
    };

    public static List<DfT.DTRO.Models.DataBase.DTRO> Dtros => new()
    {
        new DfT.DTRO.Models.DataBase.DTRO
        {
            Id = Guid.NewGuid(),
            Created = new DateTime(2025, 1, 21, 16, 21, 26),
            TrafficAuthorityCreatorId = 1000,
            TrafficAuthorityOwnerId = 1000
        },
        new DfT.DTRO.Models.DataBase.DTRO
        {
            Id = Guid.NewGuid(),
            Created = new DateTime(2025, 1, 22, 16, 21, 26),
            TrafficAuthorityCreatorId = 1000,
            TrafficAuthorityOwnerId = 1000
        },
        new DfT.DTRO.Models.DataBase.DTRO
        {
            Id = Guid.NewGuid(),
            Created = new DateTime(2025, 1, 21, 16, 21, 26),
            TrafficAuthorityCreatorId = 1000,
            TrafficAuthorityOwnerId = 1000
        },
        new DfT.DTRO.Models.DataBase.DTRO
        {
            Id = Guid.NewGuid(),
            Created = new DateTime(2025, 1, 23, 11, 2, 44),
            TrafficAuthorityCreatorId = 1000,
            TrafficAuthorityOwnerId = 1000
        },
        new DfT.DTRO.Models.DataBase.DTRO
        {
            Id = Guid.NewGuid(),
            Created = new DateTime(2025, 1, 21, 16, 21, 26),
            TrafficAuthorityCreatorId = 3300,
            TrafficAuthorityOwnerId = 3300
        },
        new DfT.DTRO.Models.DataBase.DTRO
        {
            Id = Guid.NewGuid(),
            Created = new DateTime(2025, 1, 22, 16, 21, 26),
            TrafficAuthorityCreatorId = 3300,
            TrafficAuthorityOwnerId = 3300
        },
        new DfT.DTRO.Models.DataBase.DTRO
        {
            Id = Guid.NewGuid(),
            Created = new DateTime(2025, 1, 23, 16, 21, 26),
            TrafficAuthorityCreatorId = 3300,
            TrafficAuthorityOwnerId = 3300
        },
        new DfT.DTRO.Models.DataBase.DTRO
        {
            Id = Guid.NewGuid(),
            Created = new DateTime(2025, 1, 24, 11, 2, 41),
            TrafficAuthorityCreatorId = 3300,
            TrafficAuthorityOwnerId = 3300
        },
        new DfT.DTRO.Models.DataBase.DTRO
        {
            Id = Guid.NewGuid(),
            Created = new DateTime(2025, 1, 21, 16, 21, 26),
            TrafficAuthorityCreatorId = 5000,
            TrafficAuthorityOwnerId = 5000
        },
        new DfT.DTRO.Models.DataBase.DTRO
        {
            Id = Guid.NewGuid(),
            Created = new DateTime(2025, 1, 22, 16, 21, 26),
            TrafficAuthorityCreatorId = 5000,
            TrafficAuthorityOwnerId = 5000
        },
        new DfT.DTRO.Models.DataBase.DTRO
        {
            Id = Guid.NewGuid(),
            Created = new DateTime(2025, 1, 23, 16, 21, 26),
            TrafficAuthorityCreatorId = 5000,
            TrafficAuthorityOwnerId = 5000
        },
        new DfT.DTRO.Models.DataBase.DTRO
        {
            Id = Guid.NewGuid(),
            Created = new DateTime(2025, 1, 24, 11, 2, 41),
            TrafficAuthorityCreatorId = 5000,
            TrafficAuthorityOwnerId = 5000
        }
    };

    public static List<DfT.DTRO.Models.DataBase.DTRO> GetDtros(GetAllQueryParameters queryParameters)
    {
        var dtros = Dtros.Where(it => !it.Deleted);

        if (queryParameters.TraCode != null && queryParameters.TraCode != 0)
        {
            dtros = dtros
                .Where(it => it.TrafficAuthorityCreatorId.Equals(queryParameters.TraCode) ||
                             it.TrafficAuthorityOwnerId.Equals(queryParameters.TraCode))
                .ToList();
        }

        if (queryParameters.StartDate.HasValue)
        {
            dtros = dtros
                .Where(it => it.Created >= queryParameters.StartDate.Value.ToDateTimeTruncated());
        }

        if (queryParameters.EndDate.HasValue)
        {
            dtros = dtros
                .Where(it => it.Created <= queryParameters.EndDate.Value.ToDateTimeTruncated());
        }

        return dtros.ToList();
    }

    public static List<DtroResponse> DtroResponses => new()
    {
        new DtroResponse
        {
            Id = Guid.NewGuid(),
            SchemaVersion = new SchemaVersion("3.3.1"),
            Data = new ExpandoObject()
        },
        new DtroResponse
        {
            Id = Guid.NewGuid(),
            SchemaVersion = new SchemaVersion("3.3.1"),
            Data = new ExpandoObject()
        },
        new DtroResponse
        {
            Id = Guid.NewGuid(),
            SchemaVersion = new SchemaVersion("3.3.1"),
            Data = new ExpandoObject()
        }
    };
}