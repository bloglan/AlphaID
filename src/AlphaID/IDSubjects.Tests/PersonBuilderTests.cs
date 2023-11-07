﻿using IDSubjects.ChineseName;
using IDSubjects.Subjects;
using Xunit;

namespace IDSubjects.Tests;

public class PersonBuilderTests
{
    [Fact()]
    public void SetMobileTest()
    {
        var builder = new PersonBuilder("PersonName");
        builder.SetMobile(MobilePhoneNumber.Parse("15912341234"));
        var person = builder.Person;
        Assert.Equal("+8615912341234", person.PhoneNumber);

    }

    [Fact()]
    public void UseChinesePersonNameTest()
    {
        var builder = new PersonBuilder("PersonName");
        builder.UseChinesePersonName(new ChinesePersonName("张", "三", "ZHANG", "SAN"));

        var person = builder.Person;
        Assert.Equal("三", person.FirstName);
        Assert.Equal("张三", person.Name);
        Assert.Equal("ZHANG", person.PhoneticSurname);
        Assert.Equal("SAN", person.PhoneticGivenName);
    }

}