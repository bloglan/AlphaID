﻿using IdSubjects.Subjects;

namespace IdSubjects;

/// <summary>
/// Person builder.
/// </summary>
/// <remarks>
/// Create a builder use user name and person name info.
/// </remarks>
/// <param name="userName"></param>
/// <param name="personName"></param>
/// <exception cref="ArgumentException"></exception>
public class PersonBuilder(string userName, PersonNameInfo personName)
{
    private readonly NaturalPerson person = new(userName, personName);

    /// <summary>
    /// Initialize a person builder.
    /// </summary>
    public PersonBuilder()
        : this(string.Empty, string.Empty)
    { }

    /// <summary>
    /// Create a builder use user name and full name
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="fullName"></param>
    public PersonBuilder(string userName, string fullName)
        : this(userName, new PersonNameInfo(fullName))
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public PersonBuilder SetUserName(string userName)
    {
        if (string.IsNullOrWhiteSpace(userName))
            throw new ArgumentException("User name is blank or empty");
        this.person.UserName = userName;
        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="personName"></param>
    /// <returns></returns>
    public PersonBuilder SetPersonName(PersonNameInfo personName)
    {
        this.person.PersonName = personName;
        return this;
    }

    /// <summary>
    /// Set mobile phone number.
    /// </summary>
    /// <param name="mobilePhoneNumber"></param>
    /// <param name="confirmed"></param>
    /// <returns></returns>
    public PersonBuilder SetMobile(MobilePhoneNumber? mobilePhoneNumber, bool confirmed = false)
    {
        this.person.PhoneNumber = mobilePhoneNumber?.ToString();
        this.person.PhoneNumberConfirmed = mobilePhoneNumber.HasValue && confirmed;
        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="email"></param>
    /// <param name="confirmed"></param>
    public PersonBuilder SetEmail(string? email, bool confirmed = false)
    {
        this.person.Email = email;
        this.person.EmailConfirmed = email != null && confirmed;
        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public NaturalPerson Build()
    {
        //Ensure person
        if (string.IsNullOrWhiteSpace(this.person.UserName))
            throw new InvalidOperationException("Can not build person because user name is null or blank.");
        if (string.IsNullOrWhiteSpace(this.person.PersonName.FullName))
            throw new InvalidOperationException("Can not build person because full name of person name is null or blank.");

        return this.person;
    }
}
