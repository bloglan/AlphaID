﻿namespace IDSubjects.RealName.Tests;
internal class StubRealNameValidationStore : IChineseIdCardValidationStore
{
    private readonly HashSet<ChineseIdCardValidation> set = new();

    public IQueryable<ChineseIdCardValidation> RealNameValidations => this.set.AsQueryable();

    public Task CreateAsync(ChineseIdCardValidation request)
    {
        var lastId = this.set.OrderByDescending(x => x.Id).Select(x => x.Id).FirstOrDefault();
        var nextId = lastId + 1;
        request.Id = nextId;
        this.set.Add(request);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(ChineseIdCardValidation request)
    {
        this.set.Remove(request);
        return Task.CompletedTask;
    }

    public ValueTask<ChineseIdCardValidation?> FindByIdAsync(int id)
    {
        var result = this.set.FirstOrDefault(x => x.Id == id);
        return ValueTask.FromResult(result);
    }

    public Task<ChineseIdCardValidation?> GetCurrentAsync(NaturalPerson person)
    {
        var result = this.set.OrderByDescending(p => p.Result!.ValidateTime).FirstOrDefault(p => p.Result!.Accepted && p.PersonId == person.Id);
        return Task.FromResult(result);
    }

    public Task<ChineseIdCardValidation?> GetPendingRequestAsync(NaturalPerson person)
    {
        var result = this.set.OrderByDescending(p => p.CommitTime).FirstOrDefault(p => p.Result == null && p.PersonId == person.Id);
        return Task.FromResult(result);
    }

    public Task UpdateAsync(ChineseIdCardValidation request)
    {
        //do nothing here.
        return Task.CompletedTask;
    }
}
