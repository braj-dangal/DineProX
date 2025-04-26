using System;
using System.Collections.Generic;
using System.Text;
using DineProX.Localization;
using Volo.Abp.Application.Services;

namespace DineProX;

/* Inherit your application services from this class.
 */
public abstract class DineProXAppService : ApplicationService
{
    protected DineProXAppService()
    {
        LocalizationResource = typeof(DineProXResource);
    }
}
