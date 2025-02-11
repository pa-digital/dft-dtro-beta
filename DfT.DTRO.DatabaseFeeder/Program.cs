var builder = WebApplication.CreateBuilder(args);
builder.Services.AddStorage(builder.Configuration);

var application = builder.Build();

using IServiceScope serviceScope = application.Services.CreateAsyncScope();
var context = serviceScope.ServiceProvider.GetService<DtroContext>();
if (context != null)
{
    await DatabaseFeeder.Seed(context, DatabaseFeeder.UserStatuses);
    await DatabaseFeeder.Seed(context, DatabaseFeeder.Users);
    await DatabaseFeeder.Seed(context, DatabaseFeeder.ApplicationPurposes);
    await DatabaseFeeder.Seed(context, DatabaseFeeder.ApplicationTypes);
    await DatabaseFeeder.Seed(context, DatabaseFeeder.Applications);
    await DatabaseFeeder.Seed(context, DatabaseFeeder.DigitalServiceProviders);
    await DatabaseFeeder.Seed(context, DatabaseFeeder.TrafficRegulationAuthorities);
    await DatabaseFeeder.Seed(context, DatabaseFeeder.TrafficRegulationAuthorityDigitalServiceProviders);
    await DatabaseFeeder.Seed(context, DatabaseFeeder.TrafficRegulationAuthorityDigitalServiceProviderStatuses);
    await DatabaseFeeder.Seed(context, DatabaseFeeder.Dtros);
    await DatabaseFeeder.Seed(context, DatabaseFeeder.DtroHistories);
    await DatabaseFeeder.Seed(context, DatabaseFeeder.RuleTemplates);
    await DatabaseFeeder.Seed(context, DatabaseFeeder.SchemaTemplates);
}