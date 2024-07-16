﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DfT.DTRO.FeatureManagement;
using DfT.DTRO.Models.Errors;
using DfT.DTRO.Models.SwaCode;
using DfT.DTRO.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DfT.DTRO.Controllers;

/// <summary>
/// Controller implementation that allows users to obtain swa codes.
/// </summary>
[Tags("Tra")]
[ApiController]
public class TraController : ControllerBase
{
    private readonly ITraService _traService;
    private readonly ILogger<TraController> _logger;

    /// <summary>
    /// The default constructor.
    /// </summary>
    /// <param name="traService">An <see cref="ITraService"/> instance.</param>
    /// <param name="logger">An <see cref="ILogger{TraController}"/> instance.</param>
    public TraController(
        ITraService traService,
        ILogger<TraController> logger)
    {
        _traService = traService;
        _logger = logger;
    }

    /// <summary>
    /// Gets all swa codes.
    /// </summary>
    /// <response code="200">Swa Codes retrieved successfully.</response>
    /// <response code="500">Internal server error.</response>

    [HttpGet("/v1/swaCodes")]
    [FeatureGate(RequirementType.Any, FeatureNames.DtroRead, FeatureNames.DtroWrite)]
    [SwaggerResponse(statusCode: 200, description: "Tra swa codes retrieved successfully.")]
    [SwaggerResponse(statusCode: 500, description: "Internal server error.")]

    public async Task<ActionResult<List<SwaCodeResponse>>> GetSwaCodes()
    {
        try
        {
            var swaCodeResponses = await _traService.GetUiFormattedSwaCodeAsync();
            if (swaCodeResponses == null)
            {
                swaCodeResponses = new List<SwaCodeResponse>();
            }
            return Ok(swaCodeResponses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving swa codes for TRA.");
            return StatusCode(500, new ApiErrorResponse("Internal Server Error", "An unexpected error occurred."));
        }
    }
}