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
                            Id = Guid.NewGuid(),
                            xAppId = new Guid("2e95f17a-d8bd-4ab3-840f-6bf49c5d989c"),
                            UserGroup = (int) UserGroup.Admin,
                            Name = "Prod_CSO",
                            Prefix = "",
                            TraId = null
                        }
    };

}

