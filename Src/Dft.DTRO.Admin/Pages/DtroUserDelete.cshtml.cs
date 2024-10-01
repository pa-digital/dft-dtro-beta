namespace Dft.DTRO.Admin.Pages
{
    public class DtroUserDeleteModel : PageModel
    {
        private readonly IDtroUserService _dtroUserService;
        private readonly IErrHandlingService _errHandlingService;

        public DtroUserDeleteModel(IDtroUserService dtroUserService, IErrHandlingService errHandlingService)
        {
            _dtroUserService = dtroUserService;
            _errHandlingService = errHandlingService;
        }

        public List<DtroUser> AllDtroUsers { get; set; }

        [BindProperty]
        public List<Guid> SelectedUsers { get; set; }

        public async Task OnGet()
        {
            AllDtroUsers = (await _dtroUserService.GetDtroUsersAsync())
                .OrderBy(user => user.UserGroup)
                .ThenBy(user => user.Name)
                .ThenBy(user => user.xAppId)
                .ToList();
        }

        public async Task<IActionResult> OnPost(string action)
        {
            if (action == "delete")
            {
                try
                {
                    bool isUserDeleted = await _dtroUserService.DeleteDtroUserAsync(SelectedUsers);
                    return isUserDeleted ? RedirectToPage() : RedirectToPage("Error");
                }
                catch (Exception ex)
                {
                    return _errHandlingService.HandleUiError(ex);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "No users selected for deletion.");
            }

            return RedirectToPage("Index");
        }
    }
}
