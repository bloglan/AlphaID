using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdSubjects.DirectoryLogon.Tests;
[CollectionDefinition(nameof(ServiceProviderCollection))]
public class ServiceProviderCollection : ICollectionFixture<ServiceProviderFixture>
{
}
