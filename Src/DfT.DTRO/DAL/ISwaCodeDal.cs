namespace DfT.DTRO.DAL;

/// <summary>
/// Service interface providing methods that storage Street Work Act Codes.
/// </summary>
public interface ISwaCodeDal
{
    /// <summary>
    /// Activate traffic regulation authority by its <paramref name="traId"/>.
    /// </summary>
    /// <param name="traId">Traffic regulation authority ID.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous activate operation.</returns>
    Task<GuidResponse> ActivateTraAsync(int traId);

    /// <summary>
    /// Deactivate traffic regulation authority by its <paramref name="traId"/>.
    /// </summary>
    /// <param name="traId">Traffic regulation authority ID.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous deactivate operation.</returns>
    Task<GuidResponse> DeActivateTraAsync(int traId);

    /// <summary>
    /// Get all street work act codes.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous get operation.</returns>
    Task<List<SwaCodeResponse>> GetAllCodesAsync();

    /// <summary>
    /// Find existing SWA codes by <paramref name="partialName"/>
    /// </summary>
    /// <param name="partialName">Partial name of the street work act.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous search operation.</returns>
    Task<List<SwaCodeResponse>> SearchSwaCodesAsync(string partialName);

    /// <summary>
    /// Get traffic regulation authority by <paramref name="traId"/>
    /// </summary>
    /// <param name="traId">Traffic authority regulation ID.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous get operation.</returns>
    Task<SwaCodeResponse> GetSwaCodeAsync(int traId);
    Task<SwaCode> GetTraAsync(int traId);

    /// <summary>
    /// Save traffic regulation authority by <paramref name="swaCodeRequest"/>
    /// </summary>
    /// <param name="swaCodeRequest">Street work act code request.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous save operation.</returns>
    Task<GuidResponse> SaveTraAsync(SwaCodeRequest swaCodeRequest);

    /// <summary>
    /// Check if traffic regulation authority exists by <paramref name="traId"/>
    /// </summary>
    /// <param name="traId">Traffic regulation authority ID.</param>
    /// <returns>
    /// A <see cref="Task"/> whose result is <see langword="true" /> if traffic
    /// regulation authority with specified ID exists. Otherwise <see langword="false" />
    /// </returns>
    Task<bool> TraExistsAsync(int traId);

    /// <summary>
    /// Update traffic regulation authority by <paramref name="swaCodeRequest"/>
    /// </summary>
    /// <param name="swaCodeRequest">Street work act code request.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous update operation.</returns>
    Task<GuidResponse> UpdateTraAsync(SwaCodeRequest swaCodeRequest);
}