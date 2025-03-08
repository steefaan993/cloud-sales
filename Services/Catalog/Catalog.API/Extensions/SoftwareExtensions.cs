using Catalog.API.Dtos;

namespace Catalog.API.Extensions;

public static class SoftwareExtensions
{
    public static List<Software> ToSoftwareList(this IEnumerable<SoftwareInformation> softwares)
    {
        if (!softwares.Any())
        {
            return [];
        }

        var config = new TypeAdapterConfig();
        config.NewConfig<SoftwareInformation, Software>()
              .Map(dest => dest.Name, src => src.SoftwareName)
              .Map(dest => dest.Price, src => src.PricePerMonth)
              .Map(dest => dest.Vendor, src => src.Vendor);

        return softwares.Adapt<List<Software>>(config);
    }

    public static IEnumerable<SoftwareInformation> ToSoftwareInformationList(this IEnumerable<Software> softwares)
    {
        if (!softwares.Any())
        {
            return Enumerable.Empty<SoftwareInformation>();
        }

        return softwares.Select(InformationFromSoftware);
    }

    public static SoftwareInformation ToSoftwareInformation(this Software software)
    {
        return InformationFromSoftware(software);
    }

    private static SoftwareInformation InformationFromSoftware(Software software)
    {
        return new SoftwareInformation(
                    SoftwareName: software.Name,
                    Vendor: software.Vendor,
                    PricePerMonth: software.Price
                );
    }
}
