using Microsoft.AspNetCore.Identity;

namespace IdSubjects.Diagnostics;
internal class AggregatedUserPasswordInterceptor : IUserPasswordInterceptor
{
    private readonly IEnumerable<IUserPasswordInterceptor> interceptors;
    private readonly Stack<IUserPasswordInterceptor> stack = new Stack<IUserPasswordInterceptor>();

    public AggregatedUserPasswordInterceptor(IEnumerable<IUserPasswordInterceptor> interceptors)
    {
        this.interceptors = interceptors;
    }

    public async Task<IdentityResult> PasswordChangingAsync(NaturalPerson person, string? plainPassword, CancellationToken cancellation)
    {
        List<IdentityError> errors = new List<IdentityError>();
        bool success = true;
        foreach (var interceptor in this.interceptors)
        {
            this.stack.Push(interceptor);
            var result = await interceptor.PasswordChangingAsync(person, plainPassword, cancellation);
            if (!result.Succeeded)
                success = false;
            errors.AddRange(result.Errors);
        }

        return success ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray());
    }

    public async Task PasswordChangedAsync(NaturalPerson person, CancellationToken cancellation)
    {
        while (this.stack.TryPop(out var interceptor))
        {
            await interceptor.PasswordChangedAsync(person, cancellation);
        }
    }
}
