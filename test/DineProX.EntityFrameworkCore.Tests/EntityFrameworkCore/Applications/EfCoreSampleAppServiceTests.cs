using DineProX.Samples;
using Xunit;

namespace DineProX.EntityFrameworkCore.Applications;

[Collection(DineProXTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<DineProXEntityFrameworkCoreTestModule>
{

}
