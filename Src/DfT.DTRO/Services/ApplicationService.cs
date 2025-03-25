namespace DfT.DTRO.Services;

public class ApplicationService : IApplicationService
{
    private readonly IApplicationDal _applicationDal;
    private readonly IApigeeAppRepository _apigeeAppRepository;
    private readonly ITraDal _traDal;
    private readonly IUserDal _userDal;
    private readonly DtroContext _dtroContext;
    private readonly IDtroUserDal _dtroUserDal;


    public ApplicationService(IApplicationDal applicationDal, IApigeeAppRepository apigeeAppRepository, ITraDal traDal, IUserDal userDal, DtroContext dtroContext, IDtroUserDal dtroUserDal)
    {
        _applicationDal = applicationDal;
        _apigeeAppRepository = apigeeAppRepository;
        _traDal = traDal;
        _userDal = userDal;
        _dtroContext = dtroContext;
        _dtroUserDal = dtroUserDal;
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
        return applicationResponse;
    }

    public async Task<List<ApplicationListDto>> GetApplications(string email)
    {
        return await _applicationDal.GetApplicationList(email);
    }

    public async Task<PaginatedResponse<ApplicationInactiveListDto>> GetInactiveApplications(PaginatedRequest paginatedRequest)
    {
        PaginatedResult<ApplicationInactiveListDto> paginatedResult = await _applicationDal.GetInactiveApplications(paginatedRequest);
        return new(paginatedResult.Results.ToList().AsReadOnly(), paginatedRequest.Page, paginatedResult.TotalCount);
    }

    public async Task<bool> ActivateApplicationById(Guid appId)
    {
        // TODO: approve application on Apigee
        return await _applicationDal.ActivateApplicationById(appId);
    }

    public async Task<App> CreateApplication(string email, AppInput appInput)
    {
        var developerAppInput = JsonHelper.ConvertObject<AppInput, ApigeeDeveloperAppInput>(appInput);
        var developerApp = await _apigeeAppRepository.CreateApp(email, developerAppInput);

        using (var transaction = await _dtroContext.Database.BeginTransactionAsync())
        {
            try
            {
                // In integration, we need to make a dummy TRA for the application
                TrafficRegulationAuthority tra = await _traDal.CreateTra();

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
                    StatusId = ApplicationStatusType.Inactive,
                    TrafficRegulationAuthorityId = tra.Id,
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
                    xAppId = appId,
                    // TODO: when the table contains SWA code, update this to use tra.SwaCode
                    TraId = 1,
                    Name = user.Forename + " " + user.Surname,
                    Prefix = appInput.Purpose == "Publish" ? "PUB" : "CON",
                    UserGroup = appInput.Purpose == "Publish" ? UserGroup.All : UserGroup.Consumer,
                };
                await _dtroUserDal.SaveDtroUserAsync(dtroUser);
                await transaction.CommitAsync();

                return JsonHelper.ConvertObject<ApigeeDeveloperApp, App>(developerApp);
            }
            catch (Exception)
            {
                // TODO: Delete app from Apigee
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}