namespace DfT.DTRO.Migrations;

public static class SeedData
{

    public static SystemConfig SystemConfig = new SystemConfig()
    {
        Id = new Guid("a7ab6da8-4d24-4f7f-a58b-fb7443ae8abe"),
        SystemName = "TRA Test System",
        IsTest = true
    };

    public static List<DtroUser> TrafficAuthorities = new List<DtroUser>(){
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

