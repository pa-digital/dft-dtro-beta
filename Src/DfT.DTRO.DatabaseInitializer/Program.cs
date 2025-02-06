using (var context = new DtroContext(new DbContextOptions<DtroContext>()))
{
    var isCreated = context.Database.EnsureCreated();
    if (!isCreated)
    {
        return;
    }

    await DatabaseFeeder.Seed(context, DatabaseFeeder.Users);
    await DatabaseFeeder.Seed(context, DatabaseFeeder.UserStatuses);
    await DatabaseFeeder.Seed(context, DatabaseFeeder.TrafficRegulationAuthorities);
    await DatabaseFeeder.Seed(context, DatabaseFeeder.DigitalServiceProviders);
    await DatabaseFeeder.Seed(context, DatabaseFeeder.TrafficRegulationAuthorityDigitalServiceProviders);
    await DatabaseFeeder.Seed(context, DatabaseFeeder.TrafficRegulationAuthorityDigitalServiceProviderStatuses);
    await DatabaseFeeder.Seed(context, DatabaseFeeder.Applications);
    await DatabaseFeeder.Seed(context, DatabaseFeeder.ApplicationTypes);
    await DatabaseFeeder.Seed(context, DatabaseFeeder.ApplicationPurposes);
    await DatabaseFeeder.Seed(context, DatabaseFeeder.SchemaTemplates);
    await DatabaseFeeder.Seed(context, DatabaseFeeder.RuleTemplates);
    await DatabaseFeeder.Seed(context, DatabaseFeeder.DigitalTrafficRegulationOrders);
    await DatabaseFeeder.Seed(context, DatabaseFeeder.DigitalTrafficRegulationOrderHistories);
}



