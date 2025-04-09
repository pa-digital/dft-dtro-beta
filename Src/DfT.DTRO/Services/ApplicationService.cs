namespace DfT.DTRO.Services;

public class ApplicationService : IApplicationService
{
    private readonly IApplicationDal _applicationDal;
    private readonly IApigeeAppRepository _apigeeAppRepository;
    private readonly ITraDal _traDal;
    private readonly IUserDal _userDal;
    private readonly DtroContext _dtroContext;
    private readonly IDtroUserDal _dtroUserDal;
    private readonly IWebHostEnvironment _env;


    public ApplicationService(IApplicationDal applicationDal, IApigeeAppRepository apigeeAppRepository, ITraDal traDal, IUserDal userDal, DtroContext dtroContext, IDtroUserDal dtroUserDal, IWebHostEnvironment env)
    {
        _applicationDal = applicationDal;
        _apigeeAppRepository = apigeeAppRepository;
        _traDal = traDal;
        _userDal = userDal;
        _dtroContext = dtroContext;
        _dtroUserDal = dtroUserDal;
        _env = env;
    }

    public async Task<bool> ValidateAppBelongsToUser(string email, Guid appId)
    {
        string userEmail = await _applicationDal.GetApplicationUser(appId);
        return userEmail == email;
    }

    public async Task<bool> ValidateApplicationName(string appName)
    {
        return await _applicationDal.CheckApplicationNameDoesNotExist(appName);
    }

    public async Task<ApplicationResponse> GetApplication(string email, Guid appId)
    {
        var application = await _applicationDal.GetApplicationDetails(appId);
        var name = application.Name;
        var developerApp = await _apigeeAppRepository.GetApp(email, name);
        var applicationResponse = JsonHelper.ConvertObject<ApigeeDeveloperApp, ApplicationResponse>(developerApp);
        applicationResponse.Purpose = application.Purpose;
        applicationResponse.SwaCode = application.SwaCode;
        return applicationResponse;
    }

    public async Task<PaginatedResponse<ApplicationListDto>> GetApplications(string email, PaginatedRequest paginatedRequest)
    {
        PaginatedResponse<ApplicationListDto> paginatedResponse = await _applicationDal.GetApplicationList(email, paginatedRequest);
        return new(paginatedResponse.Results.ToList().AsReadOnly(), paginatedRequest.Page, paginatedResponse.TotalCount);
    }

    public async Task<PaginatedResponse<ApplicationInactiveListDto>> GetInactiveApplications(PaginatedRequest paginatedRequest)
    {
        PaginatedResult<ApplicationInactiveListDto> paginatedResult = await _applicationDal.GetInactiveApplications(paginatedRequest);
        return new(paginatedResult.Results.ToList().AsReadOnly(), paginatedRequest.Page, paginatedResult.TotalCount);
    }

    public async Task<bool> ActivateApplicationById(string email, Guid appId)
    {
        var application = await _applicationDal.GetApplicationDetails(appId);
        var name = application.Name;
        await _apigeeAppRepository.UpdateAppStatus(email, name, "approve");
        return await _applicationDal.ActivateApplicationById(appId);
    }

    public async Task<App> CreateApplication(string email, AppInput appInput)
    {
        var developerAppInput = JsonHelper.ConvertObject<AppInput, ApigeeDeveloperAppInput>(appInput);
        developerAppInput.ApiProducts = appInput.Type == "Publish" ? new[] { "dev-publisher" } : new[] { "dev-consumer" };
        var developerApp = await _apigeeAppRepository.CreateApp(email, developerAppInput);

        // In production, API products have to be manually approved
        // TODO: approve API product in production

        using (var transaction = await _dtroContext.Database.BeginTransactionAsync())
        {
            try
            {
                // Consumer apps don't have a TRA linked
                // In integration, we need to make a dummy TRA for the application
                TrafficRegulationAuthority? tra = null;
                if (appInput.Type == "Publish")
                {
                    if (!_env.IsProduction())
                    {
                        tra = await _traDal.CreateTra();
                    }
                    else
                    {
                        if (!appInput.SwaCode.HasValue)
                        {
                            throw new Exception("SWA code is required");
                        }
                        tra = await _traDal.GetTraBySwaCode(appInput.SwaCode.Value);
                    }
                }

                // Create app in database
                var appId = Guid.Parse(developerApp.AppId);
                var typeId = appInput.Type == "Publish" ? ApplicationTypeType.Publish : ApplicationTypeType.Consume;
                var user = await _userDal.GetUserFromEmail(email);

                Application application = new Application
                {
                    Id = appId,
                    Nickname = appInput.Name,
                    ApplicationTypeId = typeId,
                    UserId = user.Id,
                    StatusId = _env.IsProduction() ? ApplicationStatusType.Inactive : ApplicationStatusType.Active,
                    TrafficRegulationAuthorityId = tra?.Id,
                    Purpose = appInput.Purpose,
                    AdditionalInformation = appInput.AdditionalInformation,
                    Activity = appInput.Activity,
                    Regions = appInput.Regions,
                    DataType = appInput.DataType
                };

                await _applicationDal.CreateApplication(application);

                // Also create in DtroUsers table
                DtroUserRequest dtroUser = new DtroUserRequest
                {
                    Id = Guid.NewGuid(),
                    AppId = appId,
                    TraId = tra?.SwaCode,
                    Name = user.Forename + " " + user.Surname,
                    Prefix = appInput.Type == "Publish" ? "PUB" : "CON",
                    UserGroup = appInput.Type == "Publish" ? UserGroup.All : UserGroup.Consumer,
                };
                await _dtroUserDal.SaveDtroUserAsync(dtroUser);
                await transaction.CommitAsync();

                return JsonHelper.ConvertObject<ApigeeDeveloperApp, App>(developerApp);
            }
            catch (Exception)
            {
                await _apigeeAppRepository.DeleteApp(email, appInput.Name);
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}