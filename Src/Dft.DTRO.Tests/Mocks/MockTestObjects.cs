namespace Dft.DTRO.Tests.Mocks;

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
            AppId = Guid.NewGuid()
        },
        new DtroUserResponse
        {
            Id = Guid.NewGuid(),
            Name  = "Derbyshire",
            TraId = 1050,
            Prefix = "DJ",
            UserGroup = UserGroup.Tra,
            AppId = Guid.NewGuid()
        },
        new DtroUserResponse
        {
            Id = Guid.NewGuid(),
            Name  = "Appyway",
            TraId = 4,
            Prefix = "AP",
            UserGroup = UserGroup.Tra,
            AppId = Guid.NewGuid()
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

    public static List<TrafficRegulationAuthority> Tras => new()
    {
        new TrafficRegulationAuthority()
        {
            Name = "tra1"
        },
        new TrafficRegulationAuthority()
        {
            Name = "tra2"
        },
        new TrafficRegulationAuthority()
        {
            Name = "tra3"
        },
        new TrafficRegulationAuthority()
        {
            Name = "tra4"
        },
        new TrafficRegulationAuthority()
        {
            Name = "tra5"
        }
    };

    public static List<TrafficRegulationAuthority> GetTras(GetAllTrasQueryParameters queryParameters)
    {
        var tras = Tras;

        if (queryParameters.TraName != null)
        {
            tras = tras
                .Where(it => it.Name.Equals(queryParameters.TraName))
                .ToList();
        }

        return tras.ToList();
    }

    public static List<DfT.DTRO.Models.DataBase.DTRO> GetDtros()
    {
        var dtros = Dtros.Where(dtro => !dtro.Deleted);

        if (!dtros.Any())
        {
            throw new NotFoundException("Active D-TRO records are not found.");
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

    public static List<TraFindAllResponse> TraFindAllResponse => new()
    {
        new TraFindAllResponse
        {
            Name = "name"
        }
    };

    public static AuthToken AuthToken => new() { AccessToken = "accessToken" };

    public static ApigeeDeveloperApp ApigeeDeveloperApp => new()
    {
        Name = "Test",
        Status = "Active",
        AppId = Guid.NewGuid().ToString(),
        CreatedAt = DateTime.UtcNow.Ticks,
        LastModifiedAt = DateTime.UtcNow.Ticks,
        DeveloperId = Guid.NewGuid().ToString(),
        Credentials = new List<ApigeeDeveloperAppCredential>()
        {
            new ApigeeDeveloperAppCredential()
            {
                ConsumerKey = "consumerKey",
                ConsumerSecret = "consumerSecret",
                IssuedAt = DateTime.UtcNow.Ticks,
                ExpiresAt = -1
            }
        }
    };

    public static App App => new()
    {
        AppId = "appId",
        CreatedAt = -1,
        Credentials =
           [
               new AppCredential
               {
                   ConsumerKey = "consumerKey",
                   ConsumerSecret = "consumerSecret",
                   ExpiresAt = -1,
                   IssuedAt = -1,
                   Status = "status"
               }
           ],
        DeveloperId = "developerId",
        LastModifiedAt = -1,
        Name = "name",
        Status = "status",
    };


}