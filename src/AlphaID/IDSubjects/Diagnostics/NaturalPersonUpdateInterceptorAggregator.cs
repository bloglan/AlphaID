using Microsoft.AspNetCore.Identity;

namespace IdSubjects.Diagnostics;
internal class NaturalPersonUpdateInterceptorAggregator
{
    private readonly IEnumerable<INaturalPersonUpdateInterceptor> interceptors;
    private readonly Stack<INaturalPersonUpdateInterceptor> stack = new();

    public NaturalPersonUpdateInterceptorAggregator(IEnumerable<INaturalPersonUpdateInterceptor> interceptors)
    {
        this.interceptors = interceptors;
    }

    public async Task<IdentityResult> PreUpdateAsync(NaturalPersonManager manager, NaturalPerson person)
    {
        bool success = true;
        List<IdentityError> errors = new();
        foreach (var interceptor in this.interceptors)
        {
            this.stack.Push(interceptor);
            var interceptorResult = await interceptor.PreUpdateAsync(manager, person);
            if (!interceptorResult.Succeeded)
                success = false;
            errors.AddRange(interceptorResult.Errors);
        }

        return success ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray());
    }

    public async Task PostUpdateAsync(NaturalPersonManager manager, NaturalPerson person)
    {
        while (this.stack.TryPop(out var interceptor))
        {
            await interceptor.PostUpdateAsync(manager, person);
        }
    }
}
