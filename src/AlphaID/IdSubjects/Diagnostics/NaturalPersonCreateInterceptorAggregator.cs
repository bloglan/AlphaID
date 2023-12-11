using Microsoft.AspNetCore.Identity;

namespace IdSubjects.Diagnostics;
internal class NaturalPersonCreateInterceptorAggregator
{
    private readonly IEnumerable<INaturalPersonCreateInterceptor> interceptors;
    private readonly Stack<INaturalPersonCreateInterceptor> stack = new();

    public NaturalPersonCreateInterceptorAggregator(IEnumerable<INaturalPersonCreateInterceptor> interceptors)
    {
        this.interceptors = interceptors;
    }

    public async Task<IdentityResult> PreCreate(NaturalPersonManager manager, NaturalPerson person, string? password = null)
    {
        List<IdentityError> errors = new();
        bool success = true;
        foreach (var interceptor in this.interceptors)
        {
            this.stack.Push(interceptor);
            var result = await interceptor.PreCreateAsync(manager, person, password);
            if (!result.Succeeded)
                success = false;
            errors.AddRange(result.Errors);
        }

        return success ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray());
    }

    public async Task PostCreate(NaturalPersonManager manager, NaturalPerson person)
    {
        while (this.stack.TryPop(out var interceptor))
        {
            await interceptor.PostCreateAsync(manager, person);
        }
    }
}
