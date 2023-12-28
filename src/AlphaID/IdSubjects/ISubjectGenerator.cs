using System.Security.Claims;

namespace IdSubjects;
public interface ISubjectGenerator
{
    string Generate(ClaimsPrincipal principal);
}
