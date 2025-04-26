using DineProX.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace DineProX.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class DineProXController : AbpControllerBase
{
    protected DineProXController()
    {
        LocalizationResource = typeof(DineProXResource);
    }
}
