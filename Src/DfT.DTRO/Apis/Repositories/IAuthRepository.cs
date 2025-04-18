﻿using DfT.DTRO.Models.Auth;

namespace DfT.DTRO.Apis.Repositories;

public interface IAuthRepository
{

    /// <summary>
    /// Get auth token
    /// </summary>
    /// <param name="authTokenInput">Parameters passed</param>
    /// <returns>Auth token</returns>
    Task<AuthToken> GetToken(AuthTokenInput authTokenInput);

}