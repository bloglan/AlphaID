using IdSubjects.Diagnostics;

namespace IdSubjects.RealName;
internal class RealNameDeleteInterceptor : NaturalPersonDeleteInterceptor
{
    private readonly IRealNameAuthenticationStore store;

    public RealNameDeleteInterceptor(IRealNameAuthenticationStore store)
    {
        this.store = store;
    }

    public override async Task PostDeleteAsync(NaturalPersonManager personManager, NaturalPerson person)
    {
        await this.store.DeleteByPersonIdAsync(person.Id);
    }

}
