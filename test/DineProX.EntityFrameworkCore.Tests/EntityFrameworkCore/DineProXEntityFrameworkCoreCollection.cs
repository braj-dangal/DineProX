using Xunit;

namespace DineProX.EntityFrameworkCore;

[CollectionDefinition(DineProXTestConsts.CollectionDefinitionName)]
public class DineProXEntityFrameworkCoreCollection : ICollectionFixture<DineProXEntityFrameworkCoreFixture>
{

}
