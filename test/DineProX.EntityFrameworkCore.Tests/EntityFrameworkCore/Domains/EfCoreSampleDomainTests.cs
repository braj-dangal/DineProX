using DineProX.Samples;
using Xunit;

namespace DineProX.EntityFrameworkCore.Domains;

[Collection(DineProXTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<DineProXEntityFrameworkCoreTestModule>
{

}
