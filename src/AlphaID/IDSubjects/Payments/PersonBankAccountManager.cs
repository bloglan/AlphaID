﻿namespace IdSubjects.Payments;

/// <summary>
///     银行账户管理器。
/// </summary>
/// <remarks>
/// </remarks>
/// <param name="store"></param>
public class PersonBankAccountManager(IPersonBankAccountStore store)
{
    /// <summary>
    ///     获取某个人的银行账户信息
    /// </summary>
    /// <param name="person"></param>
    /// <returns></returns>
    public IEnumerable<PersonBankAccount> GetBankAccounts(NaturalPerson person)
    {
        return store.BankAccounts.Where(bankAccount => bankAccount.PersonId == person.Id);
    }

    /// <summary>
    ///     为指定个人添加银行账户信息。
    /// </summary>
    /// <param name="person"></param>
    /// <param name="bankAccount"></param>
    /// <returns></returns>
    public async Task<IdOperationResult> AddBankAccountAsync(NaturalPerson person, BankAccountInfo bankAccount)
    {
        if (store.BankAccounts.Any(a => a.PersonId == person.Id && a.AccountNumber == bankAccount.AccountNumber))
            return IdOperationResult.Failed("Bank account exists.");

        return await store.CreateAsync(new PersonBankAccount
        {
            AccountNumber = bankAccount.AccountNumber,
            AccountName = bankAccount.AccountName,
            BankName = bankAccount.BankName,
            Person = person,
            PersonId = person.Id
        });
    }

    /// <summary>
    ///     删除个人的银行账户。
    /// </summary>
    /// <param name="person"></param>
    /// <param name="accountNumber"></param>
    /// <returns></returns>
    public async Task<IdOperationResult> RemoveBankAccountAsync(NaturalPerson person, string accountNumber)
    {
        PersonBankAccount? bankAccount =
            store.BankAccounts.FirstOrDefault(b => b.PersonId == person.Id && b.AccountNumber == accountNumber);
        if (bankAccount == null)
            return IdOperationResult.Failed("Bank account not found.");

        return await store.DeleteAsync(bankAccount);
    }
}