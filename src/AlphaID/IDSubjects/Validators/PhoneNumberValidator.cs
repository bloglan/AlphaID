﻿using IdSubjects.Subjects;
using Microsoft.AspNetCore.Identity;

namespace IdSubjects.Validators;

/// <summary>
///     移动电话号码验证器。
/// </summary>
public class PhoneNumberValidator : IUserValidator<NaturalPerson>
{
    /// <summary>
    ///     添加对移动电话号码格式和唯一性的验证。
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    public virtual async Task<IdentityResult> ValidateAsync(UserManager<NaturalPerson> manager, NaturalPerson user)
    {
        NaturalPersonManager naturalPersonManager = manager as NaturalPersonManager ??
                                                    throw new InvalidOperationException(
                                                        "无法转换UserManger到NaturalPersonManager。");
        var errors = new List<IdentityError>();
        string? phoneNumber = await naturalPersonManager.GetPhoneNumberAsync(user);
        if (phoneNumber == null) return errors.Count > 0 ? IdentityResult.Failed([.. errors]) : IdentityResult.Success;

        if (MobilePhoneNumber.TryParse(phoneNumber, out MobilePhoneNumber number))
        {
            NaturalPerson? owner =
                await naturalPersonManager.FindByMobileAsync(number.ToString(), CancellationToken.None);
            if (owner != null && !string.Equals(user.Id, owner.Id))
                //手机号已存在且不隶属该用户，则提示手机号重复。
                errors.Add(naturalPersonManager.ErrorDescriber.DuplicatePhoneNumber());
        }
        else
        {
            errors.Add(naturalPersonManager.ErrorDescriber.InvalidPhoneNumberFormat());
        }

        return errors.Count > 0 ? IdentityResult.Failed([.. errors]) : IdentityResult.Success;
    }
}