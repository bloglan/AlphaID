using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdSubjects;
public interface ISubjectGenerator
{
    string Generate(ClaimsPrincipal principal);
}
