using Microsoft.Extensions.Localization;
using DineProX.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace DineProX;

[Dependency(ReplaceServices = true)]
public class DineProXBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<DineProXResource> _localizer;

    public DineProXBrandingProvider(IStringLocalizer<DineProXResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
