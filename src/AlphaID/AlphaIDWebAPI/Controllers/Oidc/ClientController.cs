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
    /// 通过 Client ID 获取客户端名称。
    /// </summary>
    /// <param name="clientId">Client ID</param>
    /// <returns>Client name when exists, otherwise 404 not found.</returns>
    /// <response code="200">如果找到了客户端，则返回其名称。</response>
    /// <response code="404">没有找到客户端。</response>
    [HttpGet("{clientId}")]
    public ActionResult<ClientModel> GetClientName(string clientId)
    {
        var client = this.dbContext.Clients.FirstOrDefault(p => p.ClientId == clientId);
        return client == null ? this.NotFound() : new ClientModel(client.ClientName);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Name"></param>
    public record ClientModel(string Name);
}
