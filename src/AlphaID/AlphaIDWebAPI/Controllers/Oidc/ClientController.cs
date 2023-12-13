using Duende.IdentityServer.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlphaIdWebAPI.Controllers.Oidc;

/// <summary>
/// OIDC Client.
/// </summary>
[ApiController]
[Route("api/Oidc/Client")]
[Authorize]
public class ClientController : ControllerBase
{
    private readonly ConfigurationDbContext dbContext;

    /// <summary>
    /// Ctor.
    /// </summary>
    /// <param name="dbContext"></param>
    public ClientController(ConfigurationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <summary>
    /// Gets client name by Client Id.
    /// 通过 Client ID 获取客户端名称。
    /// </summary>
    /// <param name="clientId">Client ID</param>
    /// <returns>Client name when exists, otherwise 404 not found.</returns>
    /// <response code="200">如果找到了客户端，则返回其名称。</response>
    /// <response code="404">没有找到客户端。</response>
    [HttpGet("{clientId}")]
    public ActionResult<string> GetClientName(string clientId)
    {
        var client = this.dbContext.Clients.FirstOrDefault(p => p.ClientId == clientId);
        return client == null ? this.NotFound() : client.ClientName;
    }
}
