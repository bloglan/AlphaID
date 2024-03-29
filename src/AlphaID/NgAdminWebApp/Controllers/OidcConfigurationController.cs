﻿using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Mvc;

namespace NgAdminWebApp.Controllers;

public class OidcConfigurationController(
    IClientRequestParametersProvider clientRequestParametersProvider) : Controller
{
    public IClientRequestParametersProvider ClientRequestParametersProvider { get; } = clientRequestParametersProvider;

    [HttpGet("_configuration/{clientId}")]
    public IActionResult GetClientRequestParameters([FromRoute] string clientId)
    {
        var parameters = this.ClientRequestParametersProvider.GetClientParameters(this.HttpContext, clientId);
        return this.Ok(parameters);
    }
}