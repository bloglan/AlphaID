using AlphaIdWebAPI.Models;
using IdSubjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlphaIdWebAPI.Controllers.RealName;

/// <summary>
/// 实名信息。
/// </summary>
[Route("api/RealName/[controller]")]
[ApiController]
[Authorize("RealNameScopeRequired")]
public class ChineseIdCardController : ControllerBase
{
    /// <summary>
    /// 
    /// </summary>
    public ChineseIdCardController()
    {
    }

    /// <summary>
    /// 获取有关自然人的实名信息。
    /// </summary>
    /// <param name="personId"></param>
    /// <returns></returns>
    [HttpGet("{personId}")]
    public Task<ActionResult<ChineseIdCardModel>> GetChineseIdCardInfo(string personId)
    {
        throw new NotImplementedException();
    }
}
